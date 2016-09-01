using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApp.Models
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();
        Customer Get(string customerID);
        bool BulkAdd(List<Customer> items);
        Customer Add(Customer item);
        bool Remove(string customerID);
        bool Update(Customer item);
    }
}
