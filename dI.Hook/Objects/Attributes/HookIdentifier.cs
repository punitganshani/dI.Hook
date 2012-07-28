using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dIHook.Objects.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class HookIdentifier : HookAttribute
    {
        public Guid Id { get; private set; }
        public HookIdentifier(string guid)
        {
            Id = new Guid(guid);
        }
    }
}
