using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;  
using Newtonsoft.Json;

namespace WebAPIClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            var fullAddress =  ConfigurationManager.AppSettings["baseAddress"] + "api/customer/GetAll";
            IEnumerable<Customer> _Customer = null;

            using (var client = new HttpClient())
            {
                using (var response = client.GetAsync(fullAddress).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var customerJsonString = await response.Content.ReadAsStringAsync();
                        _Customer = JsonConvert.DeserializeObject<IEnumerable<Customer>>(customerJsonString);
                    }
                    else
                    {
                        Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                        var ErrMsg = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
                        MessageBox.Show(ErrMsg.Message);

                    }
                }
            }

            dgCustomers.DataSource = null;

             if(_Customer!=null)
             {
                 dgCustomers.DataSource = _Customer;
             }

        }

        private async void btnFind_Click(object sender, EventArgs e)
        {
            var fullAddress = ConfigurationManager.AppSettings["baseAddress"] + "api/customer/GetByID/" + txtFind.Text;
            Customer _Customer = null;
            try
            {
                using (var client = new HttpClient())
                {
                    using (var response = client.GetAsync(fullAddress).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var customerJsonString = await response.Content.ReadAsStringAsync();
                            _Customer = JsonConvert.DeserializeObject<Customer>(customerJsonString);
                        }
                        else
                        {
                            Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content.ReadAsStringAsync().Result);
                            MessageBox.Show(dict["Message"]);
                        }
                    }
                }

                dgCustomers.DataSource = null;

                if (_Customer != null)
                {
                    var _CustList = new List<Customer> { _Customer };
                    dgCustomers.DataSource = _CustList;
                }
            }
            catch (HttpRequestException ex)
            {
                // catch any exception here
            }
        }

        private async void btnFindByCountry_Click(object sender, EventArgs e)
        {
            var fullAddress = ConfigurationManager.AppSettings["baseAddress"] + "api/customer/GetByCountryName/" + txtFind.Text;
            IEnumerable<Customer> _Customer = null;
            try
            {
                using (var client = new HttpClient())
                {
                    using (var response = client.GetAsync(fullAddress).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var customerJsonString = await response.Content.ReadAsStringAsync();
                            _Customer = JsonConvert.DeserializeObject<IEnumerable<Customer>>(customerJsonString);
                        }
                        else
                        {
                            Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content.ReadAsStringAsync().Result);
                            MessageBox.Show(dict["Message"]);
                        }
                    }
                }

                dgCustomers.DataSource = null;

                if (_Customer != null)
                {
                    dgCustomers.DataSource = _Customer;
                }
            }
            catch (HttpRequestException ex)
            {
                // catch any exception here
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Customer oCustomer = new Customer
            {
                CustomerID = "CUS01",
                CompanyName = "BBA-Reman",
                ContactName = "Tridip",
                ContactTitle = "Sr Developer",
                Address = "Salt lake",
                Region = "sect-5",
                PostalCode = "700009",
                City = "Kolkata",
                Country = "India",
                Phone = "033-8547-4789",
                Fax = "033-8547-4781"
            };

            var fullAddress = ConfigurationManager.AppSettings["baseAddress"] + "api/customer/AddCustomer";

            try
            {
                using (var client = new HttpClient())
                {
                    var serializedCustomer = JsonConvert.SerializeObject(oCustomer);
                    var content = new StringContent(serializedCustomer, Encoding.UTF8, "application/json");

                    using (var response =  client.PostAsync(fullAddress, content).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show(response.ReasonPhrase);
                        }
                        else
                        {
                            Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content.ReadAsStringAsync().Result);
                            MessageBox.Show(dict["Message"]);
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // catch any exception here
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Customer oCustomer = new Customer
            {
                CustomerID = "CUS01",
                CompanyName = "BBA-Reman",
                ContactName = "Tridip1",
                ContactTitle = "Sr Developer",
                Address = "Salt lake",
                Region = "sect-5",
                PostalCode = "700009",
                City = "Kolkata",
                Country = "India",
                Phone = "033-8547-4789",
                Fax = "033-8547-4781"
            };

            var uri = new Uri(ConfigurationManager.AppSettings["baseAddress"] + "/api/customer/UpdateCustomer?CustomerID=CUS01");


            try
            {
                using (var client = new HttpClient())
                {
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var serializedCustomer = JsonConvert.SerializeObject(oCustomer);
                    var content = new StringContent(serializedCustomer, Encoding.UTF8, "application/json");

                    using (var response = client.PostAsync(uri, content).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show(response.ReasonPhrase);
                        }
                        else
                        {
                            Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content.ReadAsStringAsync().Result);
                            MessageBox.Show(dict["Message"]);
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // catch any exception here
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var uri = new Uri(ConfigurationManager.AppSettings["baseAddress"] + "/api/customer/DeleteCustomer?customerID=CUS01");

            try
            {
                using (var client = new HttpClient())
                {
                    using (var response = client.PostAsync(uri, new StringContent("")).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show(response.ReasonPhrase);
                        }
                        else
                        {
                            Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content.ReadAsStringAsync().Result);
                            MessageBox.Show(dict["Message"]);
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // catch any exception here
            }

        }

        private void btnBulkAdd_Click(object sender, EventArgs e)
        {
            List<Customer> oCustomers = new List<Customer>
            {
                new Customer
                {
                    CustomerID = "CUS02",
                    CompanyName = "TCS",
                    ContactName = "Soumojit",
                    ContactTitle = "Sr Developer",
                    Address = "Salt lake",
                    Region = "sect-5",
                    PostalCode = "700009",
                    City = "Kolkata",
                    Country = "India",
                    Phone = "033-8547-4789",
                    Fax = "033-8547-4781"
                },
                new Customer
                {
                    CustomerID = "CUS03",
                    CompanyName = "IBM",
                    ContactName = "Rajat",
                    ContactTitle = "Lead Developer",
                    Address = "Salt lake",
                    Region = "sect-5",
                    PostalCode = "700009",
                    City = "Kolkata",
                    Country = "India",
                    Phone = "033-8547-4789",
                    Fax = "033-8547-4781"
                }

            };

            var fullAddress = ConfigurationManager.AppSettings["baseAddress"] + "api/customer/AddCustomers";

            try
            {
                using (var client = new HttpClient())
                {
                    var serializedCustomer = JsonConvert.SerializeObject(oCustomers);
                    var content = new StringContent(serializedCustomer, Encoding.UTF8, "application/json");

                    using (var response = client.PostAsync(fullAddress, content).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show(response.ReasonPhrase);
                        }
                        else
                        {
                            Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content.ReadAsStringAsync().Result);
                            MessageBox.Show(dict["Message"]);
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // catch any exception here
            }
        }

        
    }
}
