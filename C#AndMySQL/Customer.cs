using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_AndMySQL
{
    internal class Customer
    {
        #region Fields
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? User_Name { get; set; }
        public string? Password { get; set; }
        public bool Is_Active { get; set; }
        #endregion
    }
}
