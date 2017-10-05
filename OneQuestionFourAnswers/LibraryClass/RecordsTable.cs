using System.Collections.Generic;

namespace LibraryClass
{
    public class RecordsTable
    {
        public RecordsTable(List<Record> records)
        {
            Records = records;
        }
        public List<Record> Records { get; set; }
        
    }
}
