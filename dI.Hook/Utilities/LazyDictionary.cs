using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dIHook.Builder;
using dIHook.Objects;

namespace dIHook.Utilities
{
    internal class LazyDictionary<T> : Dictionary<Guid, Lazy<T>>, IDisposable
                                    where T : IHook
    {
        public void Add(Type type)
        {
            Guid id = Guid.Empty;
            if (type.HasIdentifierAttribute() == false)
                throw new NotImplementedException("Hook does not have a HookIdentifier attribute which is necessary for Lazy loading when adding a Type");
            else
                id = type.GetHookIdenfier();

            if (this.ContainsKey(id) == false)
            {
                lock (this)
                {
                    if (this.ContainsKey(id) == false)
                    {
                        var lazyObject = new Lazy<T>(() => { return (T)Activator.CreateInstance(type); });
                        this.Add(id, lazyObject);
                    }
                }
            }
        }

        public void Add(T currentHook)
        {
            Guid id = currentHook.GetIdOfHook();
            if (this.ContainsKey(id) == false)
            {
                lock (this)
                {
                    if (this.ContainsKey(id) == false)
                    {
                        var lazyObject = new Lazy<T>(() => { return currentHook; });
                        this.Add(id, lazyObject);
                    }
                }
            }
        }

        public void Remove(T currentHook)
        {
            Guid id = currentHook.GetIdOfHook();

            if (this.ContainsKey(id))
            {
                lock (this)
                {
                    if (this.ContainsKey(id))
                    {
                        if (this.Remove(id)) { }
                    }
                }
            }
        }

        public void AddRange(IEnumerable<T> hooks)
        {
            foreach (var currentHook in hooks)
            {
                Guid id = currentHook.GetIdOfHook();

                if (this.ContainsKey(id) == false)
                {
                    lock (this)
                    {
                        if (this.ContainsKey(id) == false)
                        {
                            var lazyObject = new Lazy<T>(() => { return currentHook; });
                            this.Add(id, lazyObject);
                        }
                    }
                }
            }
        }

        internal void Remove(IEnumerable<T> hooks)
        {
            foreach (var currentHook in hooks)
            {
                Guid id = currentHook.GetIdOfHook();

                if (this.ContainsKey(id))
                {
                    lock (this)
                    {
                        if (this.ContainsKey(id))
                        {
                            if (this.Remove(id)) { }
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            foreach (var item in Keys)
            {
                using (this[item].Value) ;
            }
        }

        internal void ClearAll()
        {
            Clear();
        }
    }
}
