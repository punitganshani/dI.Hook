using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace dIHook.UnitTests.Helper
{
    public interface ICustomer
    {
        void UpdateCustomerOrder(int customerId, string product);
    }

    public class InternetCustomer : ICustomer
    {
        public InternetCustomer()
        {
            Debug.WriteLine("Object of Internet Customer created");
        }

        public void UpdateCustomerOrder(int customerId, string product)
        {
            Debug.WriteLine("Customer information updated");
        }
    }
}
