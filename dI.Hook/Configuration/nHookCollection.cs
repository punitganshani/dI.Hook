using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace dIHook.Configuration
{
    public class dIHookCollection: ConfigurationElementCollection
    {
        private List<dIHookElement> _hooks;

        protected override ConfigurationElement CreateNewElement()
        {
            return new dIHookElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((dIHookElement)element).Name;
        }

        public dIHookElement this[int index]
        {
            get
            {
                return (dIHookElement)base.BaseGet(index);
            }
        }

        public new dIHookElement this[string name]
        {
            get
            {
                return (dIHookElement)base.BaseGet(name);
            }
        }

        public List<dIHookElement> ToList()
        {
            _hooks = new List<dIHookElement>();
            for (int i = 0; i < base.Count; i++)
            {
                _hooks.Add(this[i]);
            }
            return _hooks;
        }

        public Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            dIHookElement paramElement;
            for (int i = 0; i < base.Count; i++)
            {
                paramElement = this[i];
                param.Add(paramElement.Name, paramElement.TypeString);
            }

            return param;
        }
    }
}
