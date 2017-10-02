using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
