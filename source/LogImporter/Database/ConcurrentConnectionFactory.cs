using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace LogImporter.Database
{
    /// <summary>
    /// Manages seperate database connecions for each thread.
    /// </summary>
    internal class ConcurrentConnectionFactory : IDisposable
    {
        private readonly string connectionString;

        private readonly bool openOnCreate;

        private readonly ConcurrentDictionary<int, IDbConnection> connections = new ConcurrentDictionary<int, IDbConnection>();

        private bool disposed;

        public ConcurrentConnectionFactory(string connectionString, bool openOnCreate)
        {
            this.connectionString = connectionString;
            this.openOnCreate = openOnCreate;
        }

        /// <summary>
        /// Returns a connection for the current thread. If <c>openOnCreate</c> is set, the connection is already opened.
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetConnection()
        {
            return this.connections.GetOrAdd(
                Thread.CurrentThread.ManagedThreadId,
                (i) => this.CreateAndOpenConnection());
        }

        /// <summary>
        /// Create and (optionall) opens a new connection.
        /// </summary>
        /// <returns></returns>
        private IDbConnection CreateAndOpenConnection()
        {
            var connection = new SqlConnection(this.connectionString);

            if (this.openOnCreate)
            {
                connection.Open();
            }

            return connection;
        }


        /// <summary>
        /// Disposes all created connections.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes all created connections (if <paramref name="disposing"/> is set to <c>true</c>).
        /// </summary>
        /// <param name="disposing">Set to true, to dispose object and connections.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                foreach (var connection in this.connections.Values)
                {
                    if (connection != null)
                    {
                        connection.Dispose();
                    }
                }

                this.connections.Clear();

                this.disposed = true;
            }
        }
    }
}
