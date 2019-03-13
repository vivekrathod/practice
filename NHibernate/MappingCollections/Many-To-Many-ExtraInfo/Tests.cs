using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Many_To_Many_ExtraInfo
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
            _log = LogManager.GetLogger(typeof(TestFixture));
        }

        [SetUp]
        public void Setup()
        {
            _log.Debug("Starting setup...");

            var sqlite = @"c:\temp\test.sqlite";
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
        public void Test1()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var person = new Person{Name = "Person1", Addresses = new List<Address>()};
                var personId = (int) session.Save(person);

                var address = new Address
                {
                    AddressString = "AddressString1",
                    IsDefault = true
                };
                
                //person.Addresses.Add(address);
                
                session.Save(address);
            }
        }
    }
}
