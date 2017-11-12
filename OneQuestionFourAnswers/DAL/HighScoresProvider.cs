using System;
using LibraryClass;

namespace DAL
{
    public class HighScoresProvider : IHighScoresProvider
    {
        private IHighScoresProvider _provider;

        public HighScoresProvider()
        {
            _provider = new SqliteHighScoresProvider();
        }

        public bool Add(Record highScore)
        {
            return _provider.Add(highScore);
        }

        public void Dispose()
        {
            _provider.Dispose();
        }

        public RecordsTable GetTable()
        {
            return _provider.GetTable();
        }
    }
}
