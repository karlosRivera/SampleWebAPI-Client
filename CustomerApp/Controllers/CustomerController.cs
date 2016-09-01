using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CustomerApp.Models;


namespace CustomerApp.Controllers
{
    [RoutePrefix("api/customer")]
    public class CustomerController : ApiController
    {
        static readonly ICustomerRepository repository = new CustomerRepository();

        [HttpGet, Route("GetAll")]
        public HttpResponseMessage GetAllCustomers()
        {
            IEnumerable<Customer> customers = repository.GetAll();
            if (customers == null)
            {
                var message = string.Format("No customers found");
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, customers);
            }


        }

        [HttpGet, Route("GetByID/{customerID?}")]
        public HttpResponseMessage GetCustomer(string customerID = null)
        {
            HttpResponseMessage retObject = null;
            bool IsError = false;
            Customer customer = null;
            var message="";

            if (string.IsNullOrEmpty(customerID))
            {
                 message = string.Format("Customer ID is empty or null");
                 HttpError err = new HttpError(message);
                 retObject = Request.CreateErrorResponse(HttpStatusCode.NotFound, err);
                 retObject.ReasonPhrase = message;
                IsError = true;
            }
            else
            {
                customer = repository.Get(customerID);
                if (customer.CustomerID == null)
                {
                     message = string.Format("Customer with id [{0}] not found", customerID);
                     HttpError err = new HttpError(message);
                     retObject = Request.CreateErrorResponse(HttpStatusCode.NotFound, err);
                     retObject.ReasonPhrase = message;
                     IsError = true;

                }
            }

            if (IsError)
                return retObject;
            else
                return Request.CreateResponse(HttpStatusCode.OK, customer);
        }

        [HttpGet, Route("GetByCountryName/{country?}")]
        public HttpResponseMessage GetCustomersByCountry(string country = null)
        {
            HttpResponseMessage retObject = null;

            if (!string.IsNullOrEmpty(country))
            {
                IEnumerable<Customer> customers = repository.GetAll().Where(
                    c => string.Equals(c.Country, country, StringComparison.OrdinalIgnoreCase));

                if (customers.Count() <= 0)
                {
                    var message = string.Format("No customers found by the country [{0}]", country);
                    HttpError err = new HttpError(message);
                    retObject = Request.CreateErrorResponse(HttpStatusCode.NotFound, err);
                    retObject.ReasonPhrase = message;
                }
                else
                {
                    retObject = Request.CreateResponse(HttpStatusCode.OK, customers);
                }
            }
            else
            {
                var message = string.Format("No country name provided");
                HttpError err = new HttpError(message);
                retObject = Request.CreateErrorResponse(HttpStatusCode.NotFound, err);
                retObject.ReasonPhrase = message;

            }
            return retObject;
        }

        // for adding new multiple customers
        [HttpPost, Route("AddCustomers")]
        public HttpResponseMessage PostCustomers(List<Customer> customer)
        {
            HttpResponseMessage response = null;
            var isError = repository.BulkAdd(customer);

            if (isError)
            {
                var message = string.Format("Due to error no customers added");
                HttpError err = new HttpError(message);
                response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, err);
                response.ReasonPhrase = message;
            }
            else
            {
                response = Request.CreateResponse<List<Customer>>(HttpStatusCode.Created, customer);
                response.ReasonPhrase = "Customers successfully added";
                //string uri = Url.Link("DefaultApi", new { customerID = customer.CustomerID });
                //response.Headers.Location = new Uri(uri);
            }

            return response;
        }

        // for adding new customer
        [HttpPost, Route("AddCustomer")]
        public HttpResponseMessage PostCustomer(Customer customer)
        {
            HttpResponseMessage response = null;
            customer = repository.Add(customer);

            if (customer == null)
            {
                var message = string.Format("Due to error no customer added");
                HttpError err = new HttpError(message);
                response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, err);
                response.ReasonPhrase = message;
            }
            else
            {
                response = Request.CreateResponse<Customer>(HttpStatusCode.Created, customer);
                response.ReasonPhrase = "Customer successfully added";
                //string uri = Url.Link("DefaultApi", new { customerID = customer.CustomerID });
                //response.Headers.Location = new Uri(uri);
            }

            return response;
        }

        // for updating existing customer based on customer id
        [HttpPost, Route("UpdateCustomer")]
        public HttpResponseMessage PutProduct(string customerID, Customer customer)
        {
            HttpResponseMessage response = null;
            customer.CustomerID = customerID;

            if (!repository.Update(customer))
            {
                var message = string.Format("Due to error no customer modified");
                HttpError err = new HttpError(message);
                response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, err);
                response.ReasonPhrase = message;
            }
            else
            {
                response = Request.CreateResponse<Customer>(HttpStatusCode.Created, customer);
                response.ReasonPhrase = "Customer successfully modified";
            }
            return response;
        }

        // for deleting existing customer based on customer id
        [HttpPost, Route("DeleteCustomer")]
        public HttpResponseMessage DeleteProduct(string customerID)
        {
            HttpResponseMessage response = null;
            Customer customer = repository.Get(customerID);
            if (customer == null)
            {
                var message = string.Format("No customer found by the ID {0}", customerID);
                HttpError err = new HttpError(message);
                response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, err);
                response.ReasonPhrase = message;
            }
            else
            {
                if(repository.Remove(customerID))
                {
                    response = Request.CreateResponse<Customer>(HttpStatusCode.Created, customer);
                    response.ReasonPhrase = "Customer successfully deleted";
                }
                else
                {
                    var message = string.Format("Due to some error customer not removed");
                    HttpError err = new HttpError(message);
                    response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, err);
                    response.ReasonPhrase = message;
                }
            }
            

            return response;
        }

    }

}