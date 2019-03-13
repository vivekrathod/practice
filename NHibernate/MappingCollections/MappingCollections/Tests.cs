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
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using NUnit.Framework;
using NHibernate;
using NHibernate.Proxy;
using NHibernate.Tool.hbm2ddl;

namespace MappingCollections
{
    [TestFixture]
    public class TestFixture
    {
        private ISessionFactory _sessionFactory;
        private ILog _log;
        private string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var appender = new FileAppender
            {
                AppendToFile = false,
                File = Path.Combine(currentDirectory, "ut.log"),
                Layout = new SimpleLayout(),
                ImmediateFlush = true
            };
            appender.ActivateOptions();
            log4net.Config.BasicConfigurator.Configure(appender);
            ((Hierarchy)log4net.LogManager.GetRepository()).Root.Level = Level.Error;

            var log = LogManager.GetLogger("NHibernate.SQL");
            ((log4net.Repository.Hierarchy.Logger)log.Logger).Level = Level.Debug;
            
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
        public void TestParentChildRelationship()
        {
            Order order;
            int savedOrderId;
            OrderLine ol1;
            OrderLine ol2;
            using (var session = _sessionFactory.OpenSession())
            // using (var tran = session.BeginTransaction())
            {
                ol1 = new OrderLine { Amount = 12, ProductName = "sun screen"};//, Order = order };
                ol2 = new OrderLine { Amount = 13, ProductName = "banjo"};//, Order = order };
                // if 'all' or 'save-update' option is not specified then save the OrderLine objects individually
                session.Save(ol1);
                session.Save(ol2);

                order = new Order { OrderNumber = 1, OrderDate = DateTime.Today };
                order.OrderLines = new List<OrderLine>{ ol1, ol2 };

                // if the cascade="all" or "save-update" option is specified in the collection mapping then saving the Order 
                // will save the OrderLine instances added to OrderLines collection as well
                savedOrderId = (int)session.Save(order);
                

                // Flush will sync the in-memory state with the persisted state by writing to the database
                session.Flush();

                // if a transcation was opened then it has to be committed to persist the changes - no need to call Flush in that case
                //tran.Commit();
            }

            using (var session = _sessionFactory.OpenSession())
            {
                order = session.Load<Order>(savedOrderId);

                Assert.True(order.OrderLines.Count == 2);
                OrderLine loadedOl1 = order.OrderLines.FirstOrDefault(ol => ol.Id == ol1.Id);
                Assert.NotNull(loadedOl1);
                Assert.True(loadedOl1.Amount == ol1.Amount);
                Assert.True(loadedOl1.ProductName == ol1.ProductName);
                Assert.True(loadedOl1.Order == order);
            }
        }

        [Test]
        public void TestUpdate()
        {
            int savedOrderId;
            Order order;
            using (var session = _sessionFactory.OpenSession())
            {
                order = new Order {OrderNumber = 1, OrderDate = DateTime.Today};
                savedOrderId = (int)session.Save(order);
                session.Flush();
            }

            using (var session = _sessionFactory.OpenSession())
            {
                order.OrderDate = DateTime.Today.AddDays(1);

                _log.Info("issuing update on Order...");
                session.Update(order);

                _log.Info("flushing the session...");
                session.Flush();
            }

            using (var session = _sessionFactory.OpenSession())
            {
                order = session.Load<Order>(savedOrderId);
                Assert.True(order.OrderDate == DateTime.Today.AddDays(1));
            }

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


        /// <summary>
        ///test the many-to-one side of the relationship
        ///Order's OrderLines collection not marked (i.e. inverse=false) and OrderLine's Order many-to-one association marked with insert/update=false 
        ///DEBUG - INSERT INTO OrderLine(Amount, ProductName, OrderId, Type) VALUES(@p0, @p1, @p2, 'MappingCollections.OrderLine'); select last_insert_rowid(); @p0 = 12 [Type: Int32(0:0:0)], @p1 = 'sun screen' [Type: String(0:0:0)], @p2 = NULL[Type: Int32(0:0:0)]
        ///DEBUG - INSERT INTO Orders(OrderNumber, OrderDate, Type) VALUES(@p0, @p1, 'MappingCollections.Order'); select last_insert_rowid(); @p0 = 1 [Type: Int32(0:0:0)], @p1 = 2018-10-26T00:00:00.0000000 [Type: DateTime(0:0:0)]
        ///DEBUG - UPDATE OrderLine SET Amount = @p0, ProductName = @p1, OrderId = @p2 WHERE Id = @p3; @p0 = 12 [Type: Int32(0:0:0)], @p1 = 'sun screen' [Type: String(0:0:0)], @p2 = 1 [Type: Int32(0:0:0)], @p3 = 1 [Type: Int32(0:0:0)]

        ///Order's OrderLines collection marked with inverse=true and OrderLine's Order many-to-one association is not marked with insert/update=false 
        ///DEBUG - INSERT INTO OrderLine(Amount, ProductName, OrderId, Type) VALUES(@p0, @p1, @p2, 'MappingCollections.OrderLine'); select last_insert_rowid(); @p0 = 12 [Type: Int32(0:0:0)], @p1 = 'sun screen' [Type: String(0:0:0)], @p2 = NULL[Type: Int32(0:0:0)]
        ///DEBUG - INSERT INTO Orders(OrderNumber, OrderDate, Type) VALUES(@p0, @p1, 'MappingCollections.Order'); select last_insert_rowid(); @p0 = 1 [Type: Int32(0:0:0)], @p1 = 2018-10-26T00:00:00.0000000 [Type: DateTime(0:0:0)]
        ///DEBUG - UPDATE OrderLine SET Amount = @p0, ProductName = @p1, OrderId = @p2 WHERE Id = @p3; @p0 = 12 [Type: Int32(0:0:0)], @p1 = 'sun screen' [Type: String(0:0:0)], @p2 = 1 [Type: Int32(0:0:0)], @p3 = 1 [Type: Int32(0:0:0)]

        /// </summary>
        [Test]
        public void TestChildToParentRelationship()
        {
            Order order;
            int savedOrderId;
            int savedOrderLineId;
            OrderLine ol1;
            using (var session = _sessionFactory.OpenSession())
            //using (var tran = session.BeginTransaction())
            {
                ol1 = new OrderLine { Amount = 12, ProductName = "sun screen" };//, Order = order };
                //savedOrderLineId = (int)session.Save(ol1);
                session.Persist(ol1);

                order = new Order { OrderNumber = 1, OrderDate = DateTime.Today };
                order.OrderLines.Add(ol1);
                
                //savedOrderId = (int)session.Save(order);
                session.Persist(order);

                ol1.Order = order;

                //ol1.Amount = 13;
                //session.Update(ol1);

                // Flush will sync the in-memory state with the persisted state by writing to the database
                session.Flush();

                // if a transcation was opened then it has to be committed to persist the changes - no need to call Flush in that case
                //tran.Commit();
            }

            //using (var session = _sessionFactory.OpenSession())
            //{
            //    ol1 = session.Load<OrderLine>(savedOrderLineId);

            //    Assert.True(order.OrderLines.Count == 2);
            //    OrderLine loadedOl1 = order.OrderLines.FirstOrDefault(ol => ol.Id == ol1.Id);
            //    Assert.NotNull(loadedOl1);
            //    Assert.True(loadedOl1.Amount == ol1.Amount);
            //    Assert.True(loadedOl1.ProductName == ol1.ProductName);
            //    Assert.True(loadedOl1.Order == order);
            //}
        }


        [Test]
        public void TestOrphans()
        {
            Order order;
            int savedOrderId;
            int savedOrderLineId;
            OrderLine ol1;
            using (var session = _sessionFactory.OpenSession())
            {
                ol1 = new OrderLine {Amount = 12, ProductName = "sun screen"};
                savedOrderLineId = (int) session.Save(ol1);

                order = new Order {OrderNumber = 1, OrderDate = DateTime.Today};
                ol1.Order = order;
                order.OrderLines.Add(ol1);
                savedOrderId = (int) session.Save(order);


                // Flush will sync the in-memory state with the persisted state by writing to the database
                session.Flush();
            }

            using (var session = _sessionFactory.OpenSession())
            {
                ol1 = session.Get<OrderLine>(savedOrderLineId);
                order = session.Get<Order>(savedOrderId);

                // when cascade is set to all-delete-orphan removing the object from the collection 
                // deletes the object
                order.OrderLines.Remove(ol1);

                session.Save(order);
                session.Flush();

                // verify the order line got deleted
                Assert.That(session.Get<OrderLine>(savedOrderLineId), Is.Null);
                
            }
        }
    }
}
