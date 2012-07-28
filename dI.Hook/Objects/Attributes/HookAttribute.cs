using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dIHook.Objects.Attributes
{
    [AttributeUsage( AttributeTargets.Method, AllowMultiple= true, Inherited=true)]
    public class HookAttribute : Attribute
    {
    }
}
