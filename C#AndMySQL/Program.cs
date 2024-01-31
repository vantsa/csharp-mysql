using C_AndMySQL;
using System.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        //TestCustomerDAL();

        //TestCustomerSentEmail();

        DbTable dbTableAttribute = (DbTable)Attribute.GetCustomAttribute(typeof(Customer), typeof(DbTable));
        string tableName = dbTableAttribute?.TableName ?? "Unknown";
        Console.WriteLine($"Table Name: {tableName}");

        // Retrieve DbField attribute values for each property
        foreach (var propertyInfo in typeof(Customer).GetProperties())
        {
            DbField dbFieldAttribute = (DbField)Attribute.GetCustomAttribute(propertyInfo, typeof(DbField));
            string fieldName = dbFieldAttribute?.FieldName ?? propertyInfo.Name;
            Console.WriteLine($"{propertyInfo.Name} Field Name: {fieldName}");

        }
    }

    private static void TestCustomerDAL()
    {
        try
        {
            #region Insert
            Customer c1 = new Customer
            {
                Name = "Elek Vicc",
                Email = "viccelek@citromail.com",
                User_Name = "Vicc",
                Password = "nemvicc",
            };
            CustomerDAL.Insert(c1);
            #endregion

            #region Update
            Customer c2 = new Customer
            {
                ID = 5,
                Name = "Adorjan Attila",
                Email = "aattila@yahoo.com",
                User_Name = "keeper",
                Password = "sportklub",
                Is_Active = false,
            };
            CustomerDAL.Update(c2);
            #endregion

            #region Delete
            Customer c3 = new Customer
            {
                ID = 2,
            };
            CustomerDAL.Delete(c3);
            #endregion

            #region GetByID
            Customer c4 = new Customer
            {
                ID = 6,
            };
             CustomerDAL.GetByID(c4);
             Console.WriteLine($"Customer ID: {c4.ID}");
             Console.WriteLine($"Customer Name: {c4.Name}");
             Console.WriteLine($"Customer Email: {c4.Email}");
             Console.WriteLine($"Customer UserName: {c4.User_Name}");
             Console.WriteLine($"Customer password: {c4.Password}");
             Console.WriteLine($"Customer isactive: {c4.Is_Active}");
            #endregion

            #region DataSet and List<Customer>
            Customer c5 = new Customer
            {
                Name = "a",
                Email = "gmail"
            };
            DataSet resultData = CustomerDAL.Browse(c5);
            DisplayData(resultData);


            List<Customer> customerCollection = CustomerDAL.GetCollection(c5);
            foreach(var customer in customerCollection)
            {
                Console.WriteLine($"Name: {customer.Name}");
            }
            #endregion
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void TestCustomerSentEmail()
    {
        try
        {
            #region Insert
            Customer_Sent_Emails se = new Customer_Sent_Emails
            {
                Customer_ID = 2,
                From_Address = "pityipalko@gmail.com",
                CC_Address = "office@yahoo.com",
                BCC_Address = "notoffice@rnd.com",
                Subject = "Information",
                Message = "Your package will arrive today",
                Sent_When = DateTime.Now,
            };
            CustomerSentEmailDAL.Insert(se);
            #endregion

            #region Update
            Customer_Sent_Emails seUpdate = new Customer_Sent_Emails
            {
                ID = 3,
                Customer_ID = 6,
                From_Address = "alparvantsa@gmail.com",
                CC_Address = "mbali@yahoo.com",
                BCC_Address = "randomname@rnd.com",
                Subject = "Updated Application",
                Message = "I want to get this job",
                Sent_When = DateTime.Now,
            };
            CustomerSentEmailDAL.Update(seUpdate);
            #endregion

            #region Delete
            Customer_Sent_Emails seDelete = new Customer_Sent_Emails
            {
                ID = 4,
            };
            CustomerSentEmailDAL.Delete(seDelete);
            #endregion

            #region GetByID
            Customer_Sent_Emails seGetByID = new Customer_Sent_Emails
            {
                ID = 3,
            };
            CustomerSentEmailDAL.GetByID(seGetByID);
                Console.WriteLine($"From: {seGetByID.From_Address}");
                Console.WriteLine($"Subject: {seGetByID.Subject}");
                Console.WriteLine($"Message: {seGetByID.Message}");
                Console.WriteLine("Succes");
            #endregion

            #region DataSet and List<Customer_Sent_Email> 
            Customer_Sent_Emails seBrowse = new Customer_Sent_Emails
            {
                From_Address = "a",
                Subject = "a"
            };
            DataSet resultData = CustomerSentEmailDAL.Browse(seBrowse);
            DisplayData(resultData);
            List<Customer_Sent_Emails> emailCollection = CustomerSentEmailDAL.GetCollection(seBrowse);
            foreach (var email in emailCollection)
            {
                Console.WriteLine($"Subject: {email.Subject}");
            }
            #endregion
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    #region Method for display the DataSet
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