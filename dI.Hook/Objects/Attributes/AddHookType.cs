using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dIHook.Builder;

namespace dIHook.Objects.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AddHookType : HookAttribute
    {
        public Type HookType;
        public AddHookType(Type typeOfHook)
        {
            HookType = typeOfHook;
        }
    }
}
