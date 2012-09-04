using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dIHook.Objects;

namespace dIHook.Utilities
{
    internal static class ContainerItemExtensions
    {
        internal static bool Contains(this List<ContainerItem> objects, Type type)
        {
            return objects.Any(x => x.InterfaceType == type);
        }

        internal static Lazy<Object> GetByType(this List<ContainerItem> objects, Type type)
        {
            if (objects.Any(x => x.InterfaceType == type))
            {
                return objects.FirstOrDefault(x => x.InterfaceType == type).Object;
            }
            return null;
        }

        internal static Lazy<Object> GetByTypeKey(this List<ContainerItem> objects, Type type, Guid key)
        {
            if (objects.Any(x => x.InterfaceType == type && x.Key == key))
            {
                return objects.FirstOrDefault(x => x.InterfaceType == type && x.Key == key).Object;
            }
            return null;
        }
    }
}
