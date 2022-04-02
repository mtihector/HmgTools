using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HmgTools.DataBase.MySql.Tests.ReadersTests
{
    internal record CustomerValueObject
    {
        public int CustomerNumber { get; set; }
        public string CustomerName { get; set; }    
        public string ContactLastName { get; set; }
        public string ContactFirstName { get; set; }    
        public string RepEmployee { get; set; }

    }
}
