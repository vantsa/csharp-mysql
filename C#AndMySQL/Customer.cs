using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_AndMySQL
{
    [DbTable("customers")]
    internal class Customer
    {
        #region Fields
        [DbField("ID")]
        public int ID { get; set; }

        [DbField("Name")]
        public string? Name { get; set; }

        [DbField("Email")]
        public string? Email { get; set; }

        [DbField("User_Name")]
        public string? UserName { get; set; }

        [DbField("Password")]
        public string? Password { get; set; }

        [DbField("Is_Active")]
        public bool IsActive { get; set; }
        #endregion
    }
}
