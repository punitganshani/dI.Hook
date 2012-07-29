using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dIHook.Builder;
using dIHook.Objects;
using dIHook.Objects.Attributes;

namespace dIHook.UnitTests.Hooks
{
    [HookIdentifier("B3D75F63-F7DA-4939-8777-1A354202B9D2")]
    public class LogHook : IHook
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public LogHook()
        {
            Name = "LogHook";
            Id = new Guid("B3D75F63-F7DA-4939-8777-1A354202B9D2"); 
        }

        public void OnInvoke(params object[] inputParams)
        {
            Console.WriteLine("Hook Called"); 
        }

        public void Dispose()
        {
            Name = null;
            Console.WriteLine("Woah! Hook Disposed!"); 
        }
    }
}
