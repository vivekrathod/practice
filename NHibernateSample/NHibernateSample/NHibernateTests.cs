using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Cfg;
using NHibernate;
using System.Data.SqlClient;
using NUnit.Framework;
using SysConfig = System.Configuration;

namespace NHibernateSample
{
    [TestFixture]
    class NHibernateTests
    {
        static readonly Configuration Configuration = ConfigureNHibernate();
        static readonly ISessionFactory SessionFactory = Configuration.BuildSessionFactory();

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            CreateDatabase();
            DeploySchema();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            CleanUp();
        }

        /// <summary>
        /// 'Update' is to be used in different sessions with detached entities -
        // save in one sessoin and update in another
        /// </summary>
        [Test]
        public void TestUpdate()
        {
            #region update
            Cat cat1 = null;
            using (ISession session = SessionFactory.OpenSession())
            {
                cat1 = new Cat { Name = "cat1", Sex = 'F', Weight = 10 };
                Int64 cid1 = (Int64)session.Save(cat1);
                
                // \note 'update'ing in the same session using the same instance does not seem to do anything (you see no sql output on console,
                cat1.Sex = 'M';
                Assert.DoesNotThrow(() => session.Update(cat1));

                // \note if you 'update' in the same session with a different instance you will get NonUniqueObjectException
                Cat cat2 = new Cat {Name = "cat2", Sex = 'F', Weight = 11};
                cat2.Id = cid1;
                Assert.Throws<NonUniqueObjectException>(() => session.Update(cat2));  //another object with same id is already associated with the session  
            }

            //update the same instance in another session
            using (ISession session = SessionFactory.OpenSession())
            {
                // if another persistent instance was already loaded then calling 'update' 
                // will fail with an exception - for example, if you do this, before 'update'
                // Cat catLoaded = session.Load<Cat>(cid1); 
                cat1.Weight = 12;

                session.Update(cat1);

                Cat cat1Loaded = session.Load<Cat>(cat1.Id);
                Assert.That(cat1, Is.EqualTo(cat1Loaded));
            }

            // update using another instance with same id
            using (ISession session = SessionFactory.OpenSession())
            {
                Cat cat4 = new Cat { Name = "cat4", Sex = 'F', Weight = 11 };
                cat4.Id = cat1.Id;

                session.Update(cat4);

                Cat cat4Loaded = session.Load<Cat>(cat4.Id);
                Assert.That(cat4, Is.EqualTo(cat4Loaded));
            }
            #endregion
        }

        
        /// <summary>
        /// 'Merge' is for merging/updating the 'state' of the persistent instance with that of the specified transient instance
        /// The target persistent instance is identified by the Id property of the transient instance.
        /// \note Use update() if you are certain that the session does not contain an already persistent instance with the same identifier. Use merge() if you want to merge your modifications at any time without consideration of the state of the session. 
        /// \ref http://docs.jboss.org/hibernate/orm/3.3/reference/en/html/objectstate.html 
        /// </summary>
        [Test]
        public void TestMerge()
        {
            #region merge in same session
            using (ISession session = SessionFactory.OpenSession())
            {
                Cat cat2 = new Cat { Name = "cat2", Sex = 'M', Weight = 13 };
                Int64 cid2 = (Int64)session.Save(cat2);

                Cat cat3 = new Cat { Name = "cat3", Sex = 'F', Weight = 20 };
                cat3.Id = cid2;

                Cat catMerged = session.Merge(cat3);
                Assert.True(ReferenceEquals(catMerged, cat2));
                Assert.False(ReferenceEquals(catMerged, cat3));
                Assert.That(catMerged, Is.EqualTo(cat3));
                

                // \note that 'merge' is for merging the state only - it will not work if you try to update the type itself
                // \note it will save a new instance in that case, and return the new persistent instance
                Kitten kitten1 = new Kitten { Name = "kitten1", Sex = 'M', Weight = 1 };
                kitten1.Id = catMerged.Id;
                Kitten kittenMerged = session.Merge(kitten1);
                Assert.False(ReferenceEquals(kittenMerged, kitten1));
                Assert.That(kittenMerged.Id, Is.Not.EqualTo(kitten1.Id));
                
            }
            #endregion
        }

        /// <summary>
        /// Lazy loading will cause the proxy to be loaded instead of the real type. 
        /// </summary>
        [Test]
        public void TestLazyLoading()
        {
            // first save a Cat
            Int64 cid = 0;
            using (ISession session = SessionFactory.OpenSession())
            {
                Cat cat = new Cat {Name = "cat2", Sex = 'M', Weight = 13};
                cid = (Int64) session.Save(cat);
            }

            using (ISession session = SessionFactory.OpenSession())
            {
                Cat cat = session.Load<Cat>(cid);
                // becasue proxy is loaded
                Assert.That(cat.GetType() , Is.Not.TypeOf<Cat>());
            }
        }

        static private void CreateDatabase()
        {
            using (SqlConnection sqlConnection = new SqlConnection(SysConfig.ConfigurationManager.AppSettings["SqlServerConnectionString"]))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    string database = SysConfig.ConfigurationManager.AppSettings["OperationalDatabase"].Replace("'", "''");
                    sqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = @database) CREATE DATABASE " + database;
                    sqlCommand.Parameters.AddWithValue("@database", database);
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        static private void DeploySchema()
        {
            SchemaUpdate update = new SchemaUpdate(Configuration);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("---- start of update script ----");
            update.Execute(e => sb.AppendLine(e), true);
            sb.AppendLine("---- end of update script ----");
            sb.AppendLine();

            for (int i = 0; i < update.Exceptions.Count; i++)
            {
                sb.AppendLine();
                if (i == 0)
                    sb.AppendLine("---- one or more statements in the update script failed to execute!! ----");
                sb.AppendLine(update.Exceptions[i].ToString());
            }

            Console.WriteLine(sb.ToString());
        }

        static Configuration ConfigureNHibernate()
        {
            var configuration = new Configuration();

            configuration.Properties["dialect"] = "NHibernate.Dialect.MsSql2008Dialect";
            configuration.Properties["connection.provider"] = "NHibernate.Connection.DriverConnectionProvider";
            configuration.Properties["connection.driver_class"] = "NHibernate.Driver.SqlClientDriver";
            string operationalDatabase = SysConfig.ConfigurationManager.AppSettings["OperationalDatabase"].Replace("'", "''");
            string connString = 
                string.Format(SysConfig.ConfigurationManager.AppSettings["OperationalDatabaseConnectionString"], operationalDatabase);
            configuration.Properties["connection.connection_string"] = connString;
            configuration.Properties["show_sql"] = "true";
            configuration.Properties["command_timeout"] = "300";
            configuration.Properties["hbm2ddl.keywords"] = "none";
            configuration.Properties["adonet.batch_size"] = "0";

            configuration.AddAssembly(typeof(NHibernateTests).Assembly);

            return configuration;
        }

        static void CleanUp()
        {
            SchemaExport se = new SchemaExport(Configuration);
            se.Drop(true, true);
        }

    }
}
