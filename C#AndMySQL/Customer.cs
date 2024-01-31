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
        [DbField("customer_id")]
        public int ID { get; set; }

        [DbField("customer_name")]
        public string? Name { get; set; }

        [DbField("customer_email")]
        public string? Email { get; set; }

        [DbField("customer_username")]
        public string? User_Name { get; set; }

        [DbField("customer_password")]
        public string? Password { get; set; }

        [DbField("customer_isactive")]
        public bool Is_Active { get; set; }
        #endregion
    }
}
