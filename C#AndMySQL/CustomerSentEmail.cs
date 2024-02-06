using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_AndMySQL
{
    internal class CustomerSentEmail
    {
        #region Fields
        public int ID { get; set; }
        public int Customer_ID { get; set; }
        public string From_Address { get; set;}
        public string CC_Address { get; set;}
        public string BCC_Address { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime Sent_When { get; set; }
        #endregion
    }
}
