using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dIHook.Builder;
using dIHook.Objects;
using dIHook.Objects.Attributes;

namespace dIHook.UnitTests.Hooks
{
    [HookIdentifier("CB75FCCF-593B-4BCF-871B-298087CDE741")]
    public class DiagnosticsHook : IHook
    {
        public string Name { get; set; }
        public Guid Id { get; set; }

        public DiagnosticsHook()
        {
            Name = "DiagnosticsHook";
            Id = new Guid("CB75FCCF-593B-4BCF-871B-298087CDE741");
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
