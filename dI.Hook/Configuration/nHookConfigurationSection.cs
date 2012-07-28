using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace dIHook.Configuration
{
    public class dIHookConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("repositories", IsDefaultCollection = false, IsKey=false)]
        [ConfigurationCollection(typeof(dIHookRepositoryElement), AddItemName = "repository", ClearItemsName = "clear", RemoveItemName = "remove")]
        public dIHookRepositoryCollection Repositories
        {
            get
            {
                return (dIHookRepositoryCollection)base["repositories"];
            }
        }
    }
}
