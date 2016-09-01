using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CustomerApp.Models
{
    public class CustomerRepository : ICustomerRepository
    {
        private string ConnectionString = ""; // @"Server=192.168.6.2\BBAKOLKATA;Database=Northwind;User Id=sa;Password=tintin11#;";
        //NorthWindConnectionString
        public CustomerRepository()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NorthWindConnectionString"].ConnectionString;
        }

        public IEnumerable<Customer> GetAll()
        {
            List<Customer> customers = new List<Customer>();
            string query = string.Format("SELECT [CustomerID], [CompanyName], [ContactName], [ContactTitle], [Address], [City], [Region], [PostalCode], [Country], [Phone], [Fax] FROM [Customers]");

            using (SqlConnection con =
                    new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Customer customer = new Customer();
                        customer.CustomerID = reader.GetString(0);
                        customer.CompanyName = reader.GetString(1);
                        customer.ContactName = reader.GetString(2);
                        customer.ContactTitle = reader.GetString(3);
                        customer.Address = reader.GetString(4);
                        customer.City = reader.GetString(5);

                        if (reader.GetValue(6) != null)
                        {
                            customer.Region = reader.GetValue(6).ToString();
                        }
                        else
                        {
                            customer.Region = string.Empty;    
                        }

                        if (reader.GetValue(7) != null)
                        {
                            customer.PostalCode = reader.GetValue(7).ToString();
                        }
                        else
                        {
                            customer.PostalCode = string.Empty;
                        }

                        customer.Country = reader.GetString(8);
                        customer.Phone = reader.GetString(9);

                        if(reader.GetValue(10) != null)
                        {
                            customer.Fax = reader.GetValue(10).ToString() ;   
                        }
                        else
                        {
                            customer.Fax = string.Empty;   
                        }
                        
                        customers.Add(customer);
                    }
                    con.Close(); 
                }
            }
            return customers.ToArray();
        }

        public Customer Get(string customerID)
        {
            Customer customer = new Customer();
            string query = string.Format(" SELECT [CustomerID], [CompanyName], [ContactName], [ContactTitle], [Address], [City], [Region], [PostalCode], [Country], [Phone], [Fax] FROM [Customers] " +
                "  WHERE CustomerID LIKE '%"+ customerID + "%'");

            using (SqlConnection con =
                    new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        customer.CustomerID = reader.GetString(0);
                        customer.CompanyName = reader.GetString(1);
                        customer.ContactName = reader.GetString(2);
                        customer.ContactTitle = reader.GetString(3);
                        customer.Address = reader.GetString(4);
                        customer.City = reader.GetString(5);

                        if (reader.GetValue(6) != null)
                        {
                            customer.Region = reader.GetValue(6).ToString();
                        }
                        else
                        {
                            customer.Region = string.Empty;
                        }

                        if (reader.GetValue(7) != null)
                        {
                            customer.PostalCode = reader.GetValue(7).ToString();
                        }
                        else
                        {
                            customer.PostalCode = string.Empty;
                        }

                        customer.Country = reader.GetString(8);
                        customer.Phone = reader.GetString(9);

                        if (reader.GetValue(10) != null)
                        {
                            customer.Fax = reader.GetValue(10).ToString();
                        }
                        else
                        {
                            customer.Fax = string.Empty;
                        }
                        
                    }
                    con.Close(); 
                }
            }
            return customer;
        }

        public Customer Add(Customer item)
        {            
            string query = string.Format("INSERT INTO [Customers] " +
                        " ( [CustomerID], [CompanyName], [ContactName], [ContactTitle], [Address], [City], [Region], [PostalCode], [Country] " +
                        " ,[Phone], [Fax] ) VALUES " +
                       " ( '{0}', '{1}', '{2}', '{3}', '{4}',  '{5}', '{6}', '{7}', '{8}',  '{9}', '{10}' )", item.CustomerID, item.CompanyName, item.ContactName,
                       item.ContactTitle, item.Address, item.City, item.Region, item.PostalCode,  item.Country, item.Phone, item.Fax);

            try
            {
                using (SqlConnection con =
                        new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                item = null;
            }
            return item;
        }

        public bool BulkAdd(List<Customer> items)
        {
            bool isError = false;

            foreach (var item in items)
            {
                string query = string.Format("INSERT INTO [Customers] " +
                            " ( [CustomerID], [CompanyName], [ContactName], [ContactTitle], [Address], [City], [Region], [PostalCode], [Country] " +
                            " ,[Phone], [Fax] ) VALUES " +
                           " ( '{0}', '{1}', '{2}', '{3}', '{4}',  '{5}', '{6}', '{7}', '{8}',  '{9}', '{10}' )", item.CustomerID, item.CompanyName, item.ContactName,
                           item.ContactTitle, item.Address, item.City, item.Region, item.PostalCode, item.Country, item.Phone, item.Fax);

                try
                {
                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }

                    isError = false;
                }
                catch (Exception ex)
                {
                    isError = true;
                }
            }

            return isError;
        }

        public bool Remove(string customerID)
        {
            string query = string.Format("DELETE FROM [Customers] WHERE CustomerID = '{0}'", customerID);
            try
            {

                using (SqlConnection con =
                        new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return false;

            }
            return true;
        }

        public bool Update(Customer item)
        {
            string query = string.Format("UPDATE [Customers] " +
                    " SET [CustomerID] = '{0}'," +
                    " [CompanyName] = '{1}', " +
                    " [ContactName] = '{2}', " +
                    " [ContactTitle] = '{3}', " +
                    " [Address] = '{4}', " +
                    " [City] = '{5}', " +
                    " [Region] = '{6}', " +
                    " [PostalCode] = '{7}', " +
                    " [Country] = '{8}', " +
                    " [Phone] = '{9}', " +
                    " [Fax] = '{10}' " +
                    " WHERE CustomerID LIKE '{11}'", item.CustomerID, item.CompanyName, item.ContactName, item.ContactTitle, item.Address, item.City, item.Region,
                     item.PostalCode, item.Country, item.Phone, item.Fax, item.CustomerID);

            try
            {

                using (SqlConnection con =
                        new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
               return false;

            }
            return true;
        }
    }
}