using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dIHook.Objects.Attributes
{
    public class RemoveHook : HookAttribute
    {
        public string[] HookName;

        public RemoveHook(string[] hookName)
        {
            HookName = hookName;
        }

        public RemoveHook(string hookName)
        {
            HookName = new[] { hookName };
        }
    }
}
