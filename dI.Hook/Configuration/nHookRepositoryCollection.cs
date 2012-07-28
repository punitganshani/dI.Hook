using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace dIHook.Configuration
{
    public class dIHookRepositoryCollection : ConfigurationElementCollection
    {
        private List<dIHookRepositoryElement> _repositories;

        protected override ConfigurationElement CreateNewElement()
        {
            return new dIHookRepositoryElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((dIHookRepositoryElement)element).Name;
        }

        public dIHookRepositoryElement this[int index]
        {
            get
            {
                return (dIHookRepositoryElement)base.BaseGet(index);
            }
        }

        public new dIHookRepositoryElement this[string name]
        {
            get
            {
                return (dIHookRepositoryElement)base.BaseGet(name);
            }
        }

        public List<dIHookRepositoryElement> ToList()
        {
            _repositories = new List<dIHookRepositoryElement>();
            for (int i = 0; i < base.Count; i++)
            {
                _repositories.Add(this[i]);
            }
            return _repositories;
        }

        public Dictionary<string, bool> ToDictionary()
        {
            Dictionary<string, bool> param = new Dictionary<string, bool>();
            dIHookRepositoryElement paramElement;
            for (int i = 0; i < base.Count; i++)
            {
                paramElement = this[i];
                param.Add(paramElement.Name, paramElement.IsEnabled);
            }

            return param;
        }
    }
}