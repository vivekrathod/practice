using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using BasicCommands.Model;
using log4net;
using NUnit.Framework;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Proxy;
using NHibernate.Tool.hbm2ddl;

namespace BasicCommands
{
    [TestFixture]
    public class TestFixture
    {
        private ISessionFactory _sessionFactory;
        private ILog _log;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            //log4net.Config.BasicConfigurator.Configure();
            //log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            _log = LogManager.GetLogger(typeof(TestFixture));
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
            configuration.Properties["show_sql"] = "false";
            configuration.Properties["generate_statistics"] = "false";

            configuration.AddAssembly(typeof(TestFixture).Assembly);

            new SchemaExport(configuration).Execute(true, true, false);

            _sessionFactory = configuration.BuildSessionFactory();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void TestUpdate()
        {
            int savedOrderId;
            Order order;
            using (var session = _sessionFactory.OpenSession())
            {
                order = new Order {OrderNumber = 1, OrderDate = DateTime.Today};
                _log.Info("issuing Save on Order...");
                savedOrderId = (int)session.Save(order);
                session.Flush();
            }

            using (var session = _sessionFactory.OpenSession())
            {
                order.OrderDate = DateTime.Today.AddDays(1);

                _log.Info("issuing Update on Order...");
                session.Update(order);

                _log.Info("flushing the session...");
                session.Flush();
            }

            using (var session = _sessionFactory.OpenSession())
            {
                _log.Info("Loading the Order...");
                order = session.Load<Order>(savedOrderId);
                Assert.True(order.OrderDate == DateTime.Today.AddDays(1));
            }

        }

        [Test]
        public void TestIncorrectUseOfSaveOrUpdate()
        {
            // create assigned identity
            TestEntity1 testEntity1 = new TestEntity1 {Id = Guid.NewGuid()};
            using (var session = _sessionFactory.OpenSession())
            {
                // calling SaveOrUpdate will issue a SELECT first in order to determine if a database row exists for the entity 
                //NHibernate: SELECT testentity_.Id, testentity_.TestProp1 as TestPr3_2_, testentity_.TestProp2 as TestPr4_2_ FROM TestEntity1 testentity_ WHERE testentity_.Id = @p0; @p0 = d8119df4 - aeee - 467e-ba5e - ab53e64aed88[Type: Guid(0:0:0)]
                //NHibernate: INSERT INTO TestEntity1(TestProp1, TestProp2, Type, Id) VALUES(@p0, @p1, 'BasicCommands.Model.TestEntity1', @p2); @p0 = 0[Type: Int32(0:0:0)], @p1 = NULL[Type: String(0:0:0)], @p2 = d8119df4 - aeee - 467e-ba5e - ab53e64aed88[Type: Guid(0:0:0)]
                session.SaveOrUpdate(testEntity1);
                session.Flush();


                // calling just Save will 'save' the extra SELECT
                //NHibernate: INSERT INTO TestEntity1 (TestProp1, TestProp2, Type, Id) VALUES (@p0, @p1, 'BasicCommands.Model.TestEntity1', @p2);@p0 = 0 [Type: Int32 (0:0:0)], @p1 = NULL [Type: String (0:0:0)], @p2 = 020c1f08-297d-4d98-bea5-1c236d767cb9 [Type: Guid (0:0:0)]
                //testEntity1 = new TestEntity1 {Id = Guid.NewGuid()};
                //var savedTestEntity1Id = session.Save(testEntity1);
                session.Flush();
            };
            
        }

        [Test]
        public void TestSaveOrUpdate()
        {
            Order order;
            int savedOrderId;
            using (var session = _sessionFactory.OpenSession())
            {
                order = new Order { OrderNumber = 1, OrderDate = DateTime.Today };
                savedOrderId = (int)session.Save(order);
                session.Flush();
            }

            OrderLine o1;
            using (var session = _sessionFactory.OpenSession())
            {
                order.OrderDate = DateTime.Today.AddDays(1);
                o1 = new OrderLine {Amount = 1, ProductName = "Product 1"};
                order.OrderLines = new List<OrderLine> {o1};

                _log.Info("issuing update on Order...");
                session.SaveOrUpdate(order); // same as just calling session.Update(order) in this case

                _log.Info("issuing save on OrderLine...");
                session.SaveOrUpdate(o1); // same as just calling session.Save(o1) in this case

                _log.Info("flushing the session...");
                session.Flush();
            }

            using (var session = _sessionFactory.OpenSession())
            {
                order = session.Load<Order>(savedOrderId);
                Assert.True(order.OrderDate == DateTime.Today.AddDays(1));
                o1 = session.Load<OrderLine>(o1.Id);
                Assert.True(order.OrderLines.First() == o1);
            }
        }

        [Test]
        public void TestMerge()
        {
            Order order;
            int savedOrderId;
            using (var session = _sessionFactory.OpenSession())
            {
                order = new Order { OrderNumber = 1, OrderDate = DateTime.Today };
                savedOrderId = (int)session.Save(order);
                session.Flush();
            }

            using (var session = _sessionFactory.OpenSession())
            {
                //create a new Order with the same Id 
                Order newOrder = new Order { Id = savedOrderId };
                newOrder.OrderDate = DateTime.Today.AddDays(1);

                Order mergedOrder = session.Merge(newOrder);
                Assert.That(mergedOrder, Is.Not.SameAs(newOrder));
                Assert.True(mergedOrder.Id == savedOrderId);
                Assert.True(mergedOrder.OrderDate == newOrder.OrderDate);

                _log.Info("flushing the session...");
                session.Flush();
            }
        }

        [Test]
        public void TestDateSaveRetrieve()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Save(new TestEntity1 {Id = Guid.NewGuid(), Today = DateTime.Today});
            }
            _log.Error("testing...");
        }

        [Test]
        public void TestTransactions()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                
                Stopwatch s = new Stopwatch();
                session.BeginTransaction(IsolationLevel.ReadCommitted);
                for (int i = 0; i < 60000; i++)
                {
                    session.Save(new TestEntity1 { Id = Guid.NewGuid(), TestProp1 = i, TestProp2 = $"Abc{i}Xyz", Today = DateTime.UtcNow });
                }
                session.Transaction.Commit();

                for (int i = 0; i < 3; i++)
                {

                    s.Restart();
                    session.BeginTransaction(IsolationLevel.ReadCommitted);
                    var result = session.Query<TestEntity1>().Where(k => k.TestProp2.Contains("")).Skip(0).Take(100).ToList(); //session.QueryOver<TestEntity1>();
                    session.Transaction.Commit();
                    s.Stop();
                    Console.WriteLine(s.ElapsedMilliseconds); // >150ms
                }
            }
        }
    }
}
