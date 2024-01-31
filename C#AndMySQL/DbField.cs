using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_AndMySQL
{
    internal class DbField : Attribute
    {
        public string? FieldName { get; }

        public DbField (string fieldName)
        {
            FieldName = fieldName;
        }
    }
}
