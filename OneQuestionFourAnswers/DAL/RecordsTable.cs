using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    class RecordsTable
    {
        public List<Record> Records;

        public RecordsTable(List<Record> records)
        {
            Records = records;
        }
    }
}
