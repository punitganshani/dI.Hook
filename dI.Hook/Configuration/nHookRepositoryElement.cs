using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace dIHook.Configuration
{
    public class dIHookRepositoryElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey=true, IsRequired=true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("enabled")]
        public bool IsEnabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        #region Collection
        [ConfigurationProperty("hooks", IsDefaultCollection = false, IsKey = false)]
        [ConfigurationCollection(typeof(dIHookElement), AddItemName = "hook", ClearItemsName = "clear", RemoveItemName = "remove")]
        public dIHookCollection Hooks
        {
            get
            {
                return (dIHookCollection)base["hooks"];
            }
        }
        #endregion
    }
}