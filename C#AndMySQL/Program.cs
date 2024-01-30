using C_AndMySQL;
using System.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        #region CUSTOMER_DAL
        Customer c1 = new Customer
        {
            Name = "Molnar Balazs",
            Email = "mbali@yahoo.com",
            User_Name = "MLnRRR",
            Password = "r6fort",
        };
        ///CustomerDAL.Insert(c1);
        #endregion

        Customer c2= new Customer
        {
            ID = 5,
            Name = "Simo Joel",
            Email = "simoj@yahoo.com",
            User_Name = "joel",
            Password = "sportklub",
            Is_Active = false,
        };
        ///CustomerDAL.Update(c2);

        Customer c3 = new Customer
        {
            ID = 4,
        };
        ///CustomerDAL.Delete(c3); 

        Customer c4 = new Customer
        {
            ID = 6,
        };
        /*
         CustomerDAL.GetByID(c4);
         Console.WriteLine($"Customer ID: {c4.ID}");
         Console.WriteLine($"Customer Name: {c4.Name}");
         Console.WriteLine($"Customer Email: {c4.Email}");
         Console.WriteLine($"Customer UserName: {c4.User_Name}");
         Console.WriteLine($"Customer password: {c4.Password}");
         Console.WriteLine($"Customer isactive: {c4.Is_Active}");
         */

        Customer c5 = new Customer
        {
            Name = "a",
            Email = "gmail"
        };
        /*DataSet resultData = CustomerDAL.Browse(c5);
        DisplayData(resultData);*/


        /*List<Customer> customerCollection = CustomerDAL.GetCollection(c5);
        foreach(var customer in customerCollection)
        {
            Console.WriteLine($"Name: {customer.Name}");
        }*/



        #region CUSTOMER_SENT_EMAILS DAL
        Customer_Sent_Emails se = new Customer_Sent_Emails
        {
            Customer_ID = 6,
            From_Address = "alparvantsa@gmail.com",
            CC_Address = "mbali@yahoo.com",
            BCC_Address = "randomname@rnd.com",
            Subject = "Application",
            Message = "I want to get this job",
            Sent_When = DateTime.Now,
        };
        ///CustomerSentEmailDAL.Insert(se);

        Customer_Sent_Emails seUpdate = new Customer_Sent_Emails
        {
            ID = 3,
            Customer_ID = 6,
            From_Address = "alparvantsa@gmail.com",
            CC_Address = "mbali@yahoo.com",
            BCC_Address = "randomname@rnd.com",
            Subject = "Appliaction",
            Message = "I want to get this job",
            Sent_When = DateTime.Now,
        };
        ///CustomerSentEmailDAL.Update(seUpdate);

        Customer_Sent_Emails seDelete = new Customer_Sent_Emails
        {
            ID = 4,
        };
        ///CustomerSentEmailDAL.Delete(seDelete);

        Customer_Sent_Emails seGetByID = new Customer_Sent_Emails
        {
            ID = 3,
        };
        /*CustomerSentEmailDAL.GetByID(seGetByID);
            Console.WriteLine($"From: {seGetByID.From_Address}");
            Console.WriteLine($"Subject: {seGetByID.Subject}");
            Console.WriteLine($"Message: {seGetByID.Message}");
            Console.WriteLine("Succes");*/

        Customer_Sent_Emails seBrowse = new Customer_Sent_Emails
        {
            From_Address = "a",
            Subject = "a"
        };
        /*DataSet resultData = CustomerSentEmailDAL.Browse(seBrowse);
        DisplayData(resultData);*/
        

        #endregion
        try
        {
            List<Customer_Sent_Emails> emailCollection = CustomerSentEmailDAL.GetCollection(seBrowse);
            foreach (var email in emailCollection)
            {
                Console.WriteLine($"Subject: {email.Subject}");
            }
        } catch(Exception ex)
        {
            Console.WriteLine($"Error adding customer: {ex.Message}");
        }

    }

    #region Method for CustomerBrowse procedure
    static void DisplayData(DataSet dataSet)
    {
        foreach (DataTable table in dataSet.Tables)
        {
            Console.WriteLine($"Table: {table.TableName}");

            // Display column names
            foreach (DataColumn column in table.Columns)
            {
                Console.Write($"{column.ColumnName}\t");
            }
            Console.WriteLine();

            // Display data rows
            foreach (DataRow row in table.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    Console.Write($"{item}\t");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
    #endregion
}