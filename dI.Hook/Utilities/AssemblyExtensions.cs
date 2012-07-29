using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using dIHook.Builder;
using dIHook.Objects;

namespace dIHook.Utilities
{
    internal static class AssemblyExtensions
    {
        internal static List<T> GetHooks<T>(this Assembly assembly, Func<T, bool> predicate) where T : IHook
        {
            List<T> hooks = new List<T>();

            try
            {
                var types = assembly.GetTypes().Where(x => x.GetInterface("IHook", true) != null);
                foreach (var item in types)
                {
                    var hook = (T)Activator.CreateInstance(item);
                    if (predicate.Invoke(hook))
                    {
                        hooks.Add(hook);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return hooks;
        }

        internal static List<Lazy<T>> GetHooksLazy<T>(this Assembly assembly, Func<T, bool> predicate) where T : IHook
        {
            List<Lazy<T>> hooks = new List<Lazy<T>>();

            try
            {
                var types = assembly.GetTypes().Where(x => x.GetInterface("IHook", true) != null);
                foreach (var item in types)
                {
                    var hook = (T)Activator.CreateInstance(item);
                    if (predicate.Invoke(hook))
                    {
                        hooks.Add(new Lazy<T>(() => { return hook; }));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return hooks;
        }
    }
}
