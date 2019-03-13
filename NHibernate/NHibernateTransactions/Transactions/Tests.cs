using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using NHibernateTransactions.Model;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using NUnit.Framework.Internal;
using IsolationLevel = System.Data.IsolationLevel;

namespace NHibernateTransactions
{
    [TestFixture]
    public class TestTransactions
    {
        static readonly string CodebaseDir = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
        static readonly string SqliteFilePath = Path.Combine(CodebaseDir, "test.sqlite");

        private ISessionFactory _sessionFactory;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            File.Delete(SqliteFilePath);
        }

        [SetUp]
        public void Setup()
        {
            if (File.Exists(SqliteFilePath))
                File.Delete(SqliteFilePath);

            var connectionStringBuilder = new SQLiteConnectionStringBuilder
            {
                DataSource = SqliteFilePath,
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

            configuration.AddAssembly(typeof(TestTransactions).Assembly);

            new SchemaExport(configuration).Execute(true, true, false);

            _sessionFactory = configuration.BuildSessionFactory();
        }

        [Test]
        public void TestTransactions_Nested_OneSession()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tran1 = session.BeginTransaction())
            {
                var t1 = new TestEntity1 {Id = Guid.NewGuid(), TestProp1 = 1, TestProp2 = "asas"};
                session.Save(t1);
                

                using (var tran2 = session.BeginTransaction())
                {
                    var t2 = new TestEntity1 {Id = Guid.NewGuid(), TestProp1 = 1, TestProp2 = "asas"};
                    session.Save(t2);
                    tran2.Commit();
                }

                Assert.Throws<ObjectDisposedException>(() => tran1.Commit());
            }
        }

        [Test]
        public void TestTransactions_Nested_TwoSessions_IsolationLevelSerializable()
        {
            using (var session1 = _sessionFactory.OpenSession())
            using (var tran1 = session1.BeginTransaction())
            {
                var t1 = new TestEntity1 { Id = Guid.NewGuid(), TestProp1 = 1, TestProp2 = "asas" };
                Assert.DoesNotThrow(() => session1.Save(t1));

                using (var session2 = _sessionFactory.OpenSession())
                {
                    // trying to start a transaction on another session fails at isolation level of Serializable
                    Assert.Throws<NHibernate.TransactionException>(() => session2.BeginTransaction());
                }

                Assert.DoesNotThrow(() => tran1.Commit());
            }
        }

        [Test]
        public void TestTransactions_Nested_TwoSessions_IsolationLevelReadCommitted()
        {
            using (var session1 = _sessionFactory.OpenSession())
            using (var tran1 = session1.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                var t1 = new TestEntity1 { Id = Guid.NewGuid(), TestProp1 = 1, TestProp2 = "asas" };
                Assert.DoesNotThrow(() => session1.Save(t1));

                using (var session2 = _sessionFactory.OpenSession())
                // trying to start a transaction on another session should succeeds at isolation level of ReadComitted
                using (var tran2 = session2.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    var t2 = new TestEntity1 { Id = Guid.NewGuid(), TestProp1 = 2, TestProp2 = "gffg" };
                    Assert.DoesNotThrow(() => session2.Save(t2));
                    Assert.DoesNotThrow(() => tran2.Commit());
                }

                Assert.DoesNotThrow(() => tran1.Commit());
            }
        }


        [Test]
        public void Test_OneConnection_OneTransactionScope_Success()
        {
            var connString = $"Data Source={SqliteFilePath};Version=3;";

            using (var scope1 = new TransactionScope())
            {
                using (var connection1 = new SQLiteConnection(connString))
                {
                    connection1.Open();

                    // Do stuff with the open connection
                    using (var command = connection1.CreateCommand())
                    {
                        // note that we are not explicitly enlisting the command with the transaction
                        var commandText = "create table table1(c1 int)";
                        command.CommandText = commandText;
                        command.ExecuteNonQuery();
                    }
                }

                scope1.Complete();
            }

            using (var conn = new SQLiteConnection(connString))
            using (var command = conn.CreateCommand())
            {
                conn.Open();

                command.CommandText = $"select count(*) from table1";
                Assert.DoesNotThrow(() => command.ExecuteScalar());
            }
        }

        [Test]
        public void Test_OneConnection_OneTransactionScope_Failure()
        {
            var connString = $"Data Source={SqliteFilePath};Version=3;";

            using (var scope1 = new TransactionScope())
            {
                using (var connection1 = new SQLiteConnection(connString))
                {
                    connection1.Open();

                    // Do stuff with the open connection
                    using (var command = connection1.CreateCommand())
                    {
                        // note that we are not explicitly enlisting the command with the transaction
                        var commandText = "create table table1(c1 int)";
                        command.CommandText = commandText;
                        command.ExecuteNonQuery();
                    }
                }

            }

            using (var conn = new SQLiteConnection(connString))
            using (var command = conn.CreateCommand())
            {
                conn.Open();

                command.CommandText = $"select count(*) from table1";
                Assert.Throws<SQLiteException>(() => command.ExecuteScalar());
            }
        }

        [Test]
        /// shows that we can not use TransactionScope with SQLite when two connections are opened
        public void Test_TwoConnections_OneTransactionScope()
        {
            var connString = $"Data Source={SqliteFilePath};Version=3";

            using (var scop1 = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
            //using (var scope1 = new TransactionScope())
            {
                using (var connection1 = new SQLiteConnection(connString))
                {
                    connection1.Open();

                    // Do stuff with the open connection
                    using (var command = connection1.CreateCommand())
                    {
                        var commandText = "create table table1(c1 int)";
                        command.CommandText = commandText;
                        command.ExecuteNonQuery();
                    }
                }

                using (var connection2 = new SQLiteConnection(connString))
                {
                    Assert.DoesNotThrow(() => connection2.Open());

                    using (var command = connection2.CreateCommand())
                    {
                        // reads work...
                        var commandText = "select count(*) from sqlite_master";
                        command.CommandText = commandText;
                        Assert.DoesNotThrow(() => command.ExecuteScalar());

                        // writes do not work..
                        command.CommandText = "create table table2(c1 int)";
                        Assert.Throws<SQLiteException>(() => command.ExecuteNonQuery());
                    }
                }

                Assert.DoesNotThrow(() => scop1.Dispose());
            }
        }

        [Test]
        /// shows that we can not use nested TransactionScopes with SQLite when two connections are opened
        public void Test_TwoConnections_TwoNestedTransactionScopes()
        {
            var connString = $"Data Source={SqliteFilePath};Version=3";

            using (var scop1 = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions{IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted}))
            //using (var scope1 = new TransactionScope())
            {
                using (var connection1 = new SQLiteConnection(connString))
                {
                    connection1.Open();
                    using (var command = connection1.CreateCommand())
                    {
                        var commandText = "create table table1(c1 int)";
                        command.CommandText = commandText;
                        command.ExecuteNonQuery();
                    }
                }

                using (var scope2 = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                using (var connection2 = new SQLiteConnection(connString))
                {
                    Assert.DoesNotThrow(() => connection2.Open());

                    using (var command = connection2.CreateCommand())
                    {
                        // reads work...
                        var commandText = "select count(*) from sqlite_master";
                        command.CommandText = commandText;
                        Assert.DoesNotThrow(() => command.ExecuteScalar());

                        // writes do not work..
                        command.CommandText = "create table table2(c1 int)";
                        Assert.Throws<SQLiteException>(() => command.ExecuteNonQuery());
                    }
                    Assert.DoesNotThrow(() => scope2.Complete());
                }

                Assert.DoesNotThrow(() => scop1.Dispose());
            }
        }

        [Test]
        public void Test_OneConnection_TwoTransactions_IsolationLevelSerializable()
        {
            var connString = $"Data Source={SqliteFilePath};Version=3;";

            using (var connection1 = new SQLiteConnection(connString))
            {
                connection1.Open();
                using (var transaction1 = connection1.BeginTransaction())
                {
                    // Do stuff with the open connection
                    using (var command = connection1.CreateCommand())
                    {
                        // note that we are not explicitly enlisting the command with the transaction
                        var commandText = "create table table1(c1 int)";
                        command.CommandText = commandText;
                        command.ExecuteNonQuery();
                    }

                    using (var transaction2 = connection1.BeginTransaction())
                    {
                        // Do stuff with the open connection
                        using (var command = connection1.CreateCommand())
                        {
                            // note that we are not explicitly enlisting the command with the transaction
                            var commandText = "create table table2(c1 int)";
                            command.CommandText = commandText;
                            command.ExecuteNonQuery();
                        }

                        Assert.DoesNotThrow(() => transaction2.Commit());
                    }

                    Assert.DoesNotThrow(() => transaction1.Rollback());
                }
            }

            // test whether the table creation was in fact rolled back even if it was committed in the inner scope
            using (var conn = new SQLiteConnection(connString))
            using (var command = conn.CreateCommand())
            {
                conn.Open();

                command.CommandText = $"select count(*) from table1";
                Assert.Throws<SQLiteException>(() => command.ExecuteScalar());

                command.CommandText = $"select count(*) from table2";
                Assert.Throws<SQLiteException>(() => command.ExecuteScalar());

            }
        }

        [Test]
        public void Test_TwoConnections_TwoTransactions_IsolationLevelSerializable()
        {
            var connString = $"Data Source={SqliteFilePath};Version=3;";

            using (var connection1 = new SQLiteConnection(connString))
            {
                connection1.Open();
                //using (var transaction1 = connection1.BeginTransaction(IsolationLevel.ReadCommitted))
                using (var transaction1 = connection1.BeginTransaction())
                {
                    // Do stuff with the open connection
                    using (var command = connection1.CreateCommand())
                    {
                        // note that we are not explicitly enlisting the command with the transaction
                        var commandText = "create table table1(c1 int)";
                        command.CommandText = commandText;
                        command.ExecuteNonQuery();
                    }
                    

                    using (var connection2 = new SQLiteConnection(connString))
                    {
                        connection2.Open();

                        //using (var transaction2 = connection1.BeginTransaction(IsolationLevel.ReadCommitted))
                        using (var transaction2 = connection1.BeginTransaction())
                        {
                            // Do stuff with the open connection
                            using (var command = connection1.CreateCommand())
                            {
                                // note that we are not explicitly enlisting the command with the transaction
                                var commandText = "create table table2(c1 int)";
                                command.CommandText = commandText;
                                command.ExecuteNonQuery();
                            }

                            Assert.DoesNotThrow(() => transaction2.Commit());
                        }
                    }

                    Assert.DoesNotThrow(() => transaction1.Rollback());
                }
            }

            // test that the outer transaction rollsback changes committed by the inner transaction
            using (var conn = new SQLiteConnection(connString))
            using (var command = conn.CreateCommand())
            {
                conn.Open();

                command.CommandText = $"select count(*) from table1";
                Assert.Throws<SQLiteException>(() => command.ExecuteScalar());

                command.CommandText = $"select count(*) from table2";
                Assert.Throws<SQLiteException>(() => command.ExecuteScalar());

            }
        }
    }
}
