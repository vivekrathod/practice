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

namespace NHibernateLazyLoad
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
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            _log = LogManager.GetLogger(typeof(TestFixture));
        }

        [SetUp]
        public void Setup()
        {
            _log.Debug("Starting setup...");

            var sqlite = @"C:\Work\Code\NHibernateLazyLoad\NHibernateLazyLoad\test.sqlite";
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
            configuration.Properties["show_sql"] = "true";
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
                order = new Order { OrderNumber = 1, OrderDate = DateTime.Today };
                ol1 = new OrderLine { Amount = 12, ProductName = "sun screen", Order = order };
                ol2 = new OrderLine { Amount = 13, ProductName = "banjo", Order = order };
                order.OrderLines = new List<OrderLine>{ ol1, ol2 };

                // if the cascade="all" option is specified in the <set> mapping then saving the Order 
                // will save the OrderLine instances added to OrderLines collection as well
                savedOrderId = (int)session.Save(order);
                // if 'all' option is not specified then save the OrderLine objects individually
                session.Save(ol1);
                session.Save(ol2);

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
                
                Assert.True(session.Merge(newOrder).Id == savedOrderId);

                _log.Info("flushing the session...");
                session.Flush();
            }
        }

        [Test]
        public void TestMergePreviouslyLoaded()
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
                // load the same Order in the Session
                order = session.Load<Order>(savedOrderId);
                // by default we get a proxy - initialize it so that the underlying object is loaded into the Session
                NHibernateUtil.Initialize(order);

                Order newOrder = new Order { Id = savedOrderId };
                newOrder.OrderDate = DateTime.Today.AddDays(1);

                // the object with the same id was loaded previously, so we get exception 
                Assert.Throws<NonUniqueObjectException>(() => session.SaveOrUpdate(newOrder));

                Assert.True(session.Merge(newOrder).Id == savedOrderId);
                Assert.True(order.OrderDate == DateTime.Today.AddDays(1));
            } 
        }

        [Test]
        public void TestMergeLazyLoadedCollection()
        {
            
            int savedOrderId;
            Order order;
            int orderLinesCount = 0;
            // create an order and fill the OrderLines collection with some order line items
            using (var session = _sessionFactory.OpenSession())
            {
                order = new Order { OrderNumber = 1, OrderDate = DateTime.Today };
                
                OrderLine o1 = new OrderLine {Amount = 1, ProductName = "p1"};
                order.OrderLines = new List<OrderLine>{o1};
                session.Save(o1);
                
                orderLinesCount = order.OrderLines.Count;

                savedOrderId = (int)session.Save(order);
                session.Flush();
            }


            // simply Get the order - assert that the collection is not initialized (becuase of lazy loading)
            using (var session = _sessionFactory.OpenSession())
            {
                order = session.Get<Order>(savedOrderId);
                Assert.IsFalse(NHibernateUtil.IsInitialized(order.OrderLines));
            }

            using (var session = _sessionFactory.OpenSession())
            {
                
                Assert.Throws<LazyInitializationException>(() =>
                {
                    int count = order.OrderLines.Count;
                });

                Order mergedOrder = session.Merge(order);
                Assert.True(mergedOrder.Id == savedOrderId);
                Assert.IsFalse(NHibernateUtil.IsInitialized(mergedOrder.OrderLines));
                Assert.True(mergedOrder.OrderLines.Count == 1);
            }
        }
        
        [Test]
        public void TestLazyLoading()
        {
            Order order;
            int savedOrderId;
            using (var session = _sessionFactory.OpenSession())
            {
                order = new Order { OrderNumber = 1, OrderDate = DateTime.Today };
                OrderLine ol1 = new OrderLine { Amount = 12, ProductName = "sun screen", Order = order };
                OrderLine ol2 = new OrderLine { Amount = 13, ProductName = "banjo", Order = order };
                order.OrderLines = new List<OrderLine> { ol1, ol2 };

                savedOrderId = (int)session.Save(order);
                session.Save(ol1);
                session.Save(ol2);

                session.Flush();
            }

            // another session
            using (var session = _sessionFactory.OpenSession())
            {
                order = session.Load<Order>(savedOrderId);
                //log.DebugFormat("loaded order id: {0}", order.Id);
                _log.Debug("About to load order number... which is a non-lazy property (default mode for properties)");
                _log.DebugFormat("loaded order number: {0}", order.OrderNumber);

                // test that the lazy property OrderDate is not loaded/initialized
                Assert.False(NHibernateUtil.IsPropertyInitialized(order, "OrderDate"));

                // test that the lazy collection OrderLines is also not initialized (collections are lazy loaded by default)
                Assert.False(NHibernateUtil.IsInitialized(order.OrderLines));
            }
        }

        [Test]
        public void TestCastToSubtypeThrows()
        {
            Order order;
            int savedOrderId;
            int savedOrderLineId1;
            int savedOrderLineId2;
            using (var session = _sessionFactory.OpenSession())
            {
                order = new SpecialOrder() { OrderNumber = 1, OrderDate = DateTime.Today, Special = "special"};
                OrderLine ol1 = new OrderLine { Amount = 12, ProductName = "sun screen", Order = order };
                SpecialOrderLine dol1 = new SpecialOrderLine { Amount = 13, ProductName = "banjo", Order = order, Special = "special banjo" };
                order.OrderLines = new List<OrderLine> { ol1, dol1 };

                savedOrderId = (int)session.Save(order);
                savedOrderLineId1 = (int)session.Save(ol1);
                savedOrderLineId2 = (int)session.Save(dol1);

                session.Flush();
            }

            // another session
            using (var session = _sessionFactory.OpenSession())
            {
                var ol1 = session.Load<OrderLine>(savedOrderLineId1);
                var ol2 = session.Load<OrderLine>(savedOrderLineId2);
                Assert.False(NHibernateUtil.IsInitialized(ol1));
                Assert.False(NHibernateUtil.IsInitialized(ol2));
                
                Assert.True(ol1.IsProxy());
                Assert.True(ol1 is OrderLine);
                _log.DebugFormat("Type is: {0}", ol1.GetType().Name);
                Assert.DoesNotThrow(() =>
                {
                    var castToConcreteType = (OrderLine) ol1;
                });

                Assert.True(ol2.IsProxy());
                Assert.True(ol2 is OrderLine);
                _log.DebugFormat("Type is: {0}", ol2.GetType().Name);
                Assert.Throws<InvalidCastException>(()=>
                {
                    var castToConcreteType = (SpecialOrderLine) ol2;
                });

                IList<OrderLine> lines = new List<OrderLine>{ol1, ol2};
                Assert.False(lines.OfType<SpecialOrderLine>().Any());

                // what happens in case of overriden method? does the proxy call the correct subtype method?
                _log.DebugFormat(ol1.MyName());
                _log.DebugFormat(ol2.MyName());

                // if it does, what is the type reported now?
                Assert.True(ol2.IsProxy());
                Assert.True(ol2 is OrderLine);
                _log.DebugFormat("Type is: {0}", ol2.GetType().Name);
                Assert.Throws<InvalidCastException>(() =>
                {
                    var castToConcreteType = (SpecialOrderLine)ol2;
                });

                //reflect on the proxy's properties
                var specialProperty = ol2.GetType().GetProperty("Special");
                Assert.Null(specialProperty);
            }
        }

        [Test] // not really a test - just a demo for SELECT N+1 problem
        public void DemoSelectNPlus1Issue()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                Order order = new Order { OrderNumber = 1, OrderDate = DateTime.Today };
                OrderLine ol1 = new OrderLine { Amount = 12, ProductName = "sun screen", Order = order };
                OrderLine ol2 = new OrderLine { Amount = 13, ProductName = "banjo", Order = order };
                OrderLine ol3 = new OrderLine { Amount = 14, ProductName = "tomato", Order = order };
                order.OrderLines = new List<OrderLine> { ol1, ol2, ol3 };

                int savedOrderId = (int)session.Save(order);
                session.Save(ol1);
                session.Save(ol2);
                session.Save(ol3);

                session.Flush();
            }

            // SELECT N+1 issue: all Orders ae loaded using 1 SELECT but for each Order 1 more SELECT is needed to get the associated OrderLines. 
            // This results in N+1 total SELECTs. If we had used direct SQL then we would need only 2 SELECTS (select * from Order and select * from OrderLines)
            //_log.InfoFormat("SELECT N+1 issue");
            //using (var session = _sessionFactory.OpenSession())
            //{
            //    _log.InfoFormat("About to load Orders");
            //    foreach (var order in session.CreateCriteria<Order>().List<Order>())
            //    {
            //        _log.InfoFormat("Order Id: {0}", order.Id);
            //        _log.InfoFormat("About to load OrderLines");
            //        foreach (var orderLine in order.OrderLines)
            //        {
            //            _log.InfoFormat("OrderLine Id: {0}", orderLine.Id);
            //        }
            //    }
            //}


            // solution
            _log.InfoFormat("SELECT N+1 solution");
            using (var session = _sessionFactory.OpenSession())
            {
                _log.InfoFormat("About to load Orders");
                //var orders = session.CreateCriteria<Order>().SetFetchMode("OrderLines", FetchMode.Eager).List<Order>();
                var orders = session.CreateQuery("from Order o left join fetch o.OrderLines").List<Order>();
                foreach (var order in orders)
                {
                    _log.InfoFormat("Order Id: {0}", order.Id);
                    _log.InfoFormat("About to load OrderLines");
                    foreach (var orderLine in order.OrderLines)
                    {
                        _log.InfoFormat("OrderLine Id: {0}", orderLine.Id);
                    }
                }
            }
        }

        [Test]
        public void TestNonUniqueObjectException()
        {
            // just create a sample order with 3 orderline items
            int savedOrderId;
            int o1Id;
            using (var session = _sessionFactory.OpenSession())
            {
                Order order = new Order { OrderNumber = 1, OrderDate = DateTime.Today };
                OrderLine ol1 = new OrderLine { Amount = 12, ProductName = "sun screen", Order = order };
                OrderLine ol2 = new OrderLine { Amount = 13, ProductName = "banjo", Order = order };
                OrderLine ol3 = new OrderLine { Amount = 14, ProductName = "tomato", Order = order };
                order.OrderLines = new List<OrderLine> { ol1, ol2, ol3 };

                savedOrderId = (int)session.Save(order);
                o1Id = (int)session.Save(ol1);
                session.Save(ol2);
                session.Save(ol3);

                session.Flush();
            }

            using (var session = _sessionFactory.OpenSession())
            {
                // load one of the order lines
                OrderLine o1 = session.Get<OrderLine>(o1Id);
                Console.WriteLine("Got orderline for product: {0}", o1.ProductName);
                Order order = session.Get<Order>(savedOrderId);

                //Assert.DoesNotThrow(() =>
                //{
                OrderLine lazyLoadedO1 = null;
                    foreach (OrderLine orderLine in order.OrderLines)
                    {
                        Console.WriteLine("Got orderline for product: {0}", orderLine.ProductName);
                        if (orderLine.Id == o1Id)
                            lazyLoadedO1 = orderLine;
                    }
                //});

                    Assert.AreEqual(o1, lazyLoadedO1);
            }

        }
    }
}
