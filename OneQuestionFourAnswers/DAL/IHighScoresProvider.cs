using LibraryClass;
using System;

namespace DAL
{
    public interface IHighScoresProvider : IDisposable
    {
        bool Add(Record highScore);
        RecordsTable GetTable();

    }
}
