using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace dIHook.UnitTests.Helper
{
    public class ECommerce
    {
        public void Process(OnlineOrder order)
        {
            _BillingProcessor.ProcessPayment(order);
            _Customer.UpdateCustomerOrder(order.CustomerId,
                order.Product);
            _Notifier.SendReceipt(order);

            Debug.WriteLine("Process called");
        }

        public ECommerce(IBillingProcessor billingProcessor,
                    ICustomer customer,
                    INotifier notifier)
        {
            _BillingProcessor = billingProcessor;
            _Customer = customer;
            _Notifier = notifier;

        }

        IBillingProcessor _BillingProcessor;
        ICustomer _Customer;
        INotifier _Notifier;

    }
}
