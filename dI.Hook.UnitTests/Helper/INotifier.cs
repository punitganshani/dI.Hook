using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace dIHook.UnitTests.Helper
{
    public interface INotifier
    {
        void SendReceipt(OnlineOrder order);
    }

    public class EmailNotifer : INotifier
    {
        public EmailNotifer()
        {
            Debug.WriteLine("Object of Email notification created");
        }

        public void SendReceipt(OnlineOrder order)
        {
            Debug.WriteLine("Send receipt");

        }
    }
}
