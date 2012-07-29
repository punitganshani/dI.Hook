using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dIHook.Objects
{
    public interface IHook : IDisposable
    {
        string Name { get; set; }
        Guid Id { get; set; }

        void OnInvoke(params object[] inputParams);
    }
}
