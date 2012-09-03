using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace dIHook.UnitTests.Helper
{
    public interface IBillingProcessor
    {
        void ProcessPayment(OnlineOrder order);
    }

    public class BillingProcessor : IBillingProcessor
    {
        private PaymentType _paymentType;

        public BillingProcessor(PaymentType paymentType)
        {
            _paymentType = paymentType;
            Debug.WriteLine("Object of Billing Processor created");
        }

        public void ProcessPayment(OnlineOrder order)
        {
            Debug.WriteLine("Payment processed with type: " + _paymentType);

        }
    }

    public enum PaymentType
    {
        Cash,
        CreditCard
    }
}
