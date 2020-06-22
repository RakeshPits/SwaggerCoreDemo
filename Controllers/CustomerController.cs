using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationInCoreDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v2")]
    public class CustomerController : ControllerBase
    {
        private readonly IList<string> _customers;

        public CustomerController()
        {
            _customers = new List<string> { "Customer1", "Customer2", "Customer3" };
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _customers;
        }

        [HttpPost]
        public IEnumerable<string> Post(string customer)
        {
            _customers.Add(customer);
            return _customers;
        }

        [HttpPut]
        public IEnumerable<string> Put(string existingCustomer, string updateCustomer)
        {
            _customers[_customers.IndexOf(existingCustomer)] = updateCustomer;
            return _customers;
        }

        [HttpDelete]
        public IEnumerable<string> Delete(string customer)
        {
            _customers.Remove(customer);

            return _customers;
        }
    }
}
