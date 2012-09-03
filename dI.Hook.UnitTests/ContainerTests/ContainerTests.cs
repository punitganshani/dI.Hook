using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dIHook.Containers;
using dIHook.UnitTests.Helper;

namespace dIHook.UnitTests.ContainerTests
{
    [TestClass]
    public class ContainerTests
    {
        [TestMethod]
        public void Test_Register()
        {
            Container container = new Container();
            container.Register<IBillingProcessor, BillingProcessor>();
            container.Register<ICustomer, InternetCustomer>();
            container.Register<INotifier, EmailNotifer>();

            Assert.IsTrue(container.RegisteredTypes.Count == 3);
        }

        [TestMethod]
        public void Test_RegisterWithResolve()
        {
            Container container = new Container();
            container.Register<IBillingProcessor, BillingProcessor>();
            container.Register<ICustomer, InternetCustomer>();
            container.Register<INotifier, EmailNotifer>();

            OnlineOrder onlineOrder = new OnlineOrder()
            {
                CustomerId = 12212,
                EmailAddress = "dihook@ganshani.com",
                Price = 400,
                Product = "NewProduct"
            };

            ECommerce commerce = container.CreateInstance<ECommerce>();
            commerce.Process(onlineOrder);

            Assert.IsNotNull(commerce);
        }

        [TestMethod]
        public void Test_RegisterWithResolveExplicitValues()
        {
            Container container = new Container();

            BillingProcessor billingProcessor = new BillingProcessor(PaymentType.CreditCard);

            container.Register<IBillingProcessor>(billingProcessor);
            container.Register<ICustomer, InternetCustomer>();
            container.Register<INotifier, EmailNotifer>();

            OnlineOrder onlineOrder = new OnlineOrder()
            {
                CustomerId = 12212,
                EmailAddress = "dihook@ganshani.com",
                Price = 400,
                Product = "NewProduct"
            };

            ECommerce commerce = container.CreateInstance<ECommerce>();
            commerce.Process(onlineOrder);

            Assert.IsNotNull(commerce);
        }
    }
}
