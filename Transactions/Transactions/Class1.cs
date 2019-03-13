using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using NUnit.Framework;
using IsolationLevel = System.Data.IsolationLevel;

namespace Transactions
{
    [TestFixture]
    public class TestTransactions
    {
        static readonly string CodebaseDir = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
        static readonly string SqliteFilePath = Path.Combine(CodebaseDir, "test.sqlite");

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            File.Delete(SqliteFilePath);
        }

        [Test]
        public void TestAutomaticEnlistment()
        {
            var connString = $"Data Source={SqliteFilePath};Version=3;";
            using (var conn = new SQLiteConnection(connString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                using (var command = conn.CreateCommand())
                {// note that we are not explicitly enlisting the command with the transaction
                    var commandText = "create table table1(c1 int)";
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();

                    for (int i = 0; i < 1000; i++)
                    {
                        commandText = $"insert into table1 values('dummy{i}');";
                        command.CommandText = commandText;
                        command.ExecuteNonQuery();
                    }
                    
                    tran.Rollback();
                }
            }

            //verify that the changes are rolled back
            Assert.That(!File.Exists(SqliteFilePath) || new FileInfo(SqliteFilePath).Length == 0) ;
        }

        [Test]
        /// shows that we can not use TransactionScope with SQLite when two connections are opened
        public void TestTransactionScopeWithSQLite_2Connections()
        {
            var connString = $"Data Source={SqliteFilePath};Version=3;";

            //using (var transaction1 = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions{IsolationLevel = IsolationLevel.ReadCommitted}))
            using (var transaction1 = new TransactionScope())
            {
                using (var connection1 = new SQLiteConnection(connString))
                {
                    connection1.Open();

                    // Do stuff with the open connection
                    using (var command = connection1.CreateCommand())
                    {// note that we are not explicitly enlisting the command with the transaction
                        var commandText = "create table table1(c1 int)";
                        command.CommandText = commandText;
                        command.ExecuteNonQuery();
                    }
                } // Closes the connection (so we think …)

                //using (var transaction2 = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                using (var transaction2 = new TransactionScope(TransactionScopeOption.Required))
                using (var connection2 = new SQLiteConnection(connString))
                {
                    // This line of code throws a SQLiteException with an error 
                    // code 'Busy' after a certain timeout has been expired
                    Assert.Throws<SQLiteException>(() => connection2.Open());

                    //// Do stuff with the open connection
                    //using (var command = connection2.CreateCommand())
                    //{// note that we are not explicitly enlisting the command with the transaction
                    //    var commandText = "create table table2(c1 int)";
                    //    command.CommandText = commandText;
                    //    command.ExecuteNonQuery();
                    //}

                    transaction2.Complete();
                }

                transaction1.Complete();
            }
        }

        [Test]
        public void TestNestedTransactionsWithSQLite()
        {
            var connString = $"Data Source={SqliteFilePath};AllowNestedTransactions=false;Version=3;";

            using (var connection1 = new SQLiteConnection(connString))
            {
                connection1.Open();
                if ((connection1.Flags & SQLiteConnectionFlags.AllowNestedTransactions) != 0)
                    Console.WriteLine("enabled");
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

                        transaction2.Commit();
                    }

                    transaction1.Rollback();
                }
            }

            // test whether the table creation was in fact rolled back even if it was committed in the inner scope
            using (var conn = new SQLiteConnection(connString))
            using (var command = conn.CreateCommand())
            {
                conn.Open();
                command.CommandText = $"select count(*) from table2";
                Assert.Throws<SQLiteException>(() => command.ExecuteScalar());

            }
        }

        [Test]
        public void Test_TwoConnections_TwoTransactionsWithSQLite()
        {
            var connString = $"Data Source={SqliteFilePath};AllowNestedTransactions=false;Version=3;";

            using (var connection1 = new SQLiteConnection(connString))
            {
                connection1.Open();
                if ((connection1.Flags & SQLiteConnectionFlags.AllowNestedTransactions) != 0)
                    Console.WriteLine("enabled");
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
                    transaction1.Commit();

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

                            transaction2.Rollback();
                        }
                    }

                    
                }
            }

            // test whether the table creation was in fact rolled back even if it was committed in the inner scope
            using (var conn = new SQLiteConnection(connString))
            using (var command = conn.CreateCommand())
            {
                conn.Open();

                command.CommandText = $"select count(*) from table1";
                Assert.DoesNotThrow(() => command.ExecuteScalar());

                command.CommandText = $"select count(*) from table2";
                Assert.Throws<SQLiteException>(() => command.ExecuteScalar());

            }
        }
    }
}
