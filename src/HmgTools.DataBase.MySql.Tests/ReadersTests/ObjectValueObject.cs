using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HmgTools.DataBase.MySql.Tests.ReadersTests
{
    public record ObjectValueObject
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }

    }
}
