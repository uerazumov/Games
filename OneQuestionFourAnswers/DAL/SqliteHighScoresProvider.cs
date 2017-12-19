using LibraryClass;
using System.Collections.Generic;
using System.Data.SQLite;
using LoggingService;
using System;

namespace DAL
{
    public class SqliteHighScoresProvider : SqliteAdapter, IHighScoresProvider
    {
        public SqliteHighScoresProvider()
        {
            const string sql = "CREATE TABLE IF NOT EXISTS high_scores " +
                               "(name VARCHAR(15), score INT)";
            var command = new SQLiteCommand(sql, Connection);
            command.ExecuteNonQuery();
        }

        public bool Add(Record item)
        {
            try
            {
                var sql = "INSERT INTO high_scores (name, score) " +  $"values ('{item.Name}', {item.Score})";
                var command = new SQLiteCommand(sql, Connection);
                command.ExecuteNonQuery();
                sql = "SELECT COUNT(*) FROM high_scores";
                command = new SQLiteCommand(sql, Connection);
                var count = (long) command.ExecuteScalar();
                if (count <= 3)
                {
                    return true;
                }
                sql = "SELECT score FROM high_scores ORDER BY score ASC LIMIT 1";
                command = new SQLiteCommand(sql, Connection);
                var lowest_score = (int) command.ExecuteScalar();
                sql = $"DELETE FROM high_scores WHERE rowid = (SELECT rowid FROM high_scores WHERE score = {lowest_score} LIMIT 1)";
                command = new SQLiteCommand(sql, Connection);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                GlobalLogger.Instance.Error("Вызвана ошибка " + e.Message + " при добавлении рекорда в БД");
                return false;
            }
            return true;
        }

        public RecordsTable GetTable()
        {
            var results = new List<Record>();
            try
            {
                const string sql = "SELECT * FROM high_scores ORDER BY score DESC";
                var command = new SQLiteCommand(sql, Connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new Record((string)reader["name"], (int)reader["score"]));
                }
            }
            catch (Exception e)
            {
                GlobalLogger.Instance.Error("Произошла ошибка " + e.Message + " при получении таблицы рекордов из БД");
            }
            return new RecordsTable(results);
        }
    }
}
