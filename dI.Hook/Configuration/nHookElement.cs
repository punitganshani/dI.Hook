using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using dIHook.Builder;
using dIHook.Objects;

namespace dIHook.Configuration
{
    public class dIHookElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string TypeString
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        public Type GetHookType()
        {
            if (string.IsNullOrEmpty(TypeString))
                throw new ConfigurationErrorsException("Hook name is empty. Please check configuration file");

            Type hookType = Type.GetType(TypeString);
            if (hookType.GetInterface("IHook", false) == null)
                throw new InvalidOperationException("Hook does not inherit the interface IHook:" + Name);
            return hookType;
        }

        public T CreateInstance<T>() where T : IHook
        {
            if (string.IsNullOrEmpty(TypeString))
                throw new ConfigurationErrorsException("Hook name is empty. Please check configuration file");

            Type hookType = Type.GetType(TypeString);
            if (hookType.GetInterface("IHook", false) == null)
                throw new InvalidOperationException("Hook does not inherit the interface IHook:" + Name);
            return (T)Activator.CreateInstance(hookType);
        }
    }
}