using System;
using System.Data.SQLite;
using System.IO;

namespace DAL
{
    public class SqliteAdapter : IDisposable
    {
        private const string DatabaseFilename = "game.sqlite";
        protected readonly SQLiteConnection Connection;

        protected SqliteAdapter()
        {
            if (!File.Exists(DatabaseFilename))
            {
                SQLiteConnection.CreateFile(DatabaseFilename);
            }
            Connection = new SQLiteConnection($"Data Source={DatabaseFilename};Version=3;");
            Connection.Open();
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}
