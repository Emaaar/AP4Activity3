using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AP4Activity3
{
   public class Person
    {

        [PrimaryKey, AutoIncrement]
        public int MeterNo { get; set; }
        public string PrincipalAmount { get; set; }
        public string AmountPayable { get; set; }
    }
}


