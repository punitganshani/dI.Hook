using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dIHook.Builder;
using dIHook.Objects;

namespace dIHook.Utilities
{
    public class LazyDictionary<T> : Dictionary<Guid, Lazy<T>>, IDisposable
                                    where T : IHook
    {
        public void Add(T currentHook)
        {
            if (this.ContainsKey(currentHook.Id) == false)
            {
                lock (this)
                {
                    if (this.ContainsKey(currentHook.Id) == false)
                    {
                        var lazyObject = new Lazy<T>(() => { return currentHook; });
                        this.Add(currentHook.Id, lazyObject);
                        currentHook.OnAdded();
                    }
                }
            }
        }

        public void Remove(T currentHook)
        {
            if (this.ContainsKey(currentHook.Id))
            {
                lock (this)
                {
                    if (this.ContainsKey(currentHook.Id))
                    {
                        if (this.Remove(currentHook.Id))
                        {
                            currentHook.OnRemoved();
                        }
                    }
                }
            }
        }

        public void AddRange(IEnumerable<T> hooks)
        {
            foreach (var currentHook in hooks)
            {
                if (this.ContainsKey(currentHook.Id) == false)
                {
                    lock (this)
                    {
                        if (this.ContainsKey(currentHook.Id) == false)
                        {
                            var lazyObject = new Lazy<T>(() => { return currentHook; });
                            this.Add(currentHook.Id, lazyObject);
                            currentHook.OnAdded();
                        }
                    }
                }
            }
        }

        internal void Remove(IEnumerable<T> hooks)
        {
            foreach (var currentHook in hooks)
            {
                if (this.ContainsKey(currentHook.Id))
                {
                    lock (this)
                    {
                        if (this.ContainsKey(currentHook.Id))
                        {
                            if (this.Remove(currentHook.Id))
                                currentHook.OnRemoved();
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
