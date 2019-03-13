using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using log4net;
using NUnit.Framework;
using NHibernate;
using NHibernate.Proxy;
using NHibernate.Tool.hbm2ddl;

namespace Many_To_Many
{
    [TestFixture]
    public class Tests
    {
        private ISessionFactory _sessionFactory;
        private ILog _log;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            //log4net.Config.BasicConfigurator.Configure();
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            _log = LogManager.GetLogger(typeof(Tests));
        }

        [SetUp]
        public void Setup()
        {
            _log.Debug("Starting setup...");

            var sqlite = @"test.sqlite";
            if (File.Exists(sqlite))
                File.Delete(sqlite);

            var connectionStringBuilder = new SQLiteConnectionStringBuilder
            {
                DataSource = sqlite,
                Version = 3,
                BinaryGUID = false,
                DateTimeKind = DateTimeKind.Utc,
                ForeignKeys = true 
            };

            var configuration = new NHibernate.Cfg.Configuration();

            configuration.Properties["dialect"] = "NHibernate.Dialect.SQLiteDialect";
            configuration.Properties["connection.driver_class"] = "NHibernate.Driver.SQLite20Driver";
            configuration.Properties["connection.connection_string"] = connectionStringBuilder.ConnectionString;
            //configuration.Properties["show_sql"] = "true";
            configuration.Properties["generate_statistics"] = "false";

            configuration.AddAssembly(typeof(Tests).Assembly);

            new SchemaExport(configuration).Execute(true, true, false);

            _sessionFactory = configuration.BuildSessionFactory();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void TestSave()
        {
            Author author1, author2;
            int savedAuthor1Id, savedAuthor2Id;
            Work w1, w2;
            int savedWork1Id, savedWork2Id;

            // save a few relationships 
            // author1 -> (w1, w2)
            // author2 -> w1
            // w1 -> (author1, author2)
            // w2 -> author1
            using (var session = _sessionFactory.OpenSession())
            {
                w1 = new Work {Name = "sun screen"};
                savedWork1Id = (int) session.Save(w1);

                w2 = new Work {Name = "banjo"};
                savedWork2Id = (int)session.Save(w2);

                author1 = new Author {Name = "author1", Works = new List<Work> {w1, w2}};
                savedAuthor1Id = (int)session.Save(author1);

                author2 = new Author {Name = "author2", Works = new List<Work> {w1}};
                savedAuthor2Id = (int)session.Save(author2);

                w1.Authors = new List<Author> {author1, author2};
                session.Update(w1);

                w2.Authors = new List<Author> {author1};
                session.Update(w2);

                // Flush will sync the in-memory state with the persisted state by writing to the database
                session.Flush();
            }

            using (var session = _sessionFactory.OpenSession())
            {
                author1 = session.Get<Author>(savedAuthor1Id);
                author2 = session.Get<Author>(savedAuthor2Id);
                w1 = session.Get<Work>(savedWork1Id);
                w2 = session.Get<Work>(savedWork2Id);

                Assert.That(author1.Works, Contains.Item(w1));
                Assert.That(author1.Works, Contains.Item(w2));
                Assert.That(author1.Works.Count, Is.EqualTo(2));

                Assert.That(author2.Works.Contains(w1));
                Assert.That(author2.Works.Count, Is.EqualTo(1));

                Assert.That(w1.Authors.Contains(author1));
                Assert.That(w1.Authors.Contains(author2));
                Assert.That(w1.Authors.Count, Is.EqualTo(2));

                Assert.That(w2.Authors.Contains(author1));
                Assert.That(w2.Authors.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void TestDelete()
        {
            Author author1, author2;
            int savedAuthor1Id, savedAuthor2Id;
            Work w1, w2;
            int savedWork1Id, savedWork2Id;

            _log.Debug("save a few relationships..");
            using (var session = _sessionFactory.OpenSession())
            {
                _log.Debug("saving w1, w2");
                w1 = new Work { Name = "sun screen" };
                savedWork1Id = (int)session.Save(w1);
                w2 = new Work { Name = "banjo" };
                savedWork2Id = (int)session.Save(w2);

                _log.Debug("saving author1, and author1 -> (w1, w2) relationship");
                author1 = new Author { Name = "author1", Works = new List<Work> { w1, w2 } };
                savedAuthor1Id = (int)session.Save(author1);

                _log.Debug("saving author2, and author2 -> w1 relationship");
                author2 = new Author { Name = "author2", Works = new List<Work> { w1 } };
                savedAuthor2Id = (int)session.Save(author2);

                _log.Debug("saving w1 -> (author1, author2) relationship");
                w1.Authors = new List<Author> { author1, author2 };
                session.Update(w1);

                _log.Debug("saving w2 -> author1 relationship");
                w2.Authors = new List<Author> { author1 };
                session.Update(w2);

                // Flush will sync the in-memory state with the persisted state by writing to the database
                session.Flush();
            }

            using (var session = _sessionFactory.OpenSession())
            {
                _log.Debug("getting author1, author2, w1, w2");
                author1 = session.Get<Author>(savedAuthor1Id);
                author2 = session.Get<Author>(savedAuthor2Id);
                w1 = session.Get<Work>(savedWork1Id);
                w2 = session.Get<Work>(savedWork2Id);

                _log.Debug("removing the author1 -> w2 relationship..");
                author1.Works.Remove(w2);
                session.Update(author1);

                _log.Debug("trying to remove the w1 -> author2 relationship from the 'inverse' end will NOT work..");
                w1.Authors.Remove(author2);
                session.Update(w1);

                session.Flush();
            }

            using (var session = _sessionFactory.OpenSession())
            {
                _log.Debug("getting author1, author2, w1");
                author1 = session.Get<Author>(savedAuthor1Id);
                author2 = session.Get<Author>(savedAuthor2Id);
                w1 = session.Get<Work>(savedWork1Id);

                _log.Debug("testing that the author1 -> w2 relationship was removed properly..");
                Assert.That(author1.Works, Contains.Item(w1));
                Assert.That(author1.Works.Count.Equals(1));

                _log.Debug("testing that the w1 -> author2 relationship was NOT removed because it was removed from the 'inverse' end..");
                Assert.That(w1.Authors.Contains(author1));
                Assert.That(w1.Authors.Contains(author2));
                Assert.That(w1.Authors.Count.Equals(2));
            }
        }

        [Test]
        public void TestCascadeSaveUpdate()
        {
            Author author1;
            int savedAuthor1Id, savedAuthor2Id;
            Work w1;
            int savedWork1Id, savedWork2Id;

            // save a relationship
            // author1 -> w1
            using (var session = _sessionFactory.OpenSession())
            {
                w1 = new Work { Name = "sun screen" };
                author1 = new Author { Name = "author1", Works = new List<Work> { w1 } };
                savedAuthor1Id = (int)session.Save(author1);

                // Flush will sync the in-memory state with the persisted state by writing to the database
                session.Flush();
            }

            //using (var session = _sessionFactory.OpenSession())
            //{
            //    w1 = session.Get<Work>(savedWork1Id);
            //    author1 = session.Get<Author>(savedAuthor1Id);
            //    Assert.That(w1.Authors.Contains(author1));
            //}
        }
    }
}
