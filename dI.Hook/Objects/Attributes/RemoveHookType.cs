using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dIHook.Objects.Attributes
{
    public class RemoveHookType : HookAttribute
    {
        public Type HookType;
        public RemoveHookType(Type typeOfHook)
        {
            HookType = typeOfHook;
        }
    }
}
