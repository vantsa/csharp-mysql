using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_AndMySQL
{
    internal class DbTable : Attribute
    {
        public string? TableName { get; }

        public DbTable (string tableName)
        {
            TableName = tableName;
        }
    }
}
