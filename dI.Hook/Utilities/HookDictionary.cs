using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dIHook.Builder;
using dIHook.Objects;

namespace dIHook.Utilities
{
    public class HookDictionary<T> : Dictionary<Guid, T>, IDisposable where T: IHook
    {
        public void Add(T currentHook)
        {
            if (this.ContainsKey(currentHook.Id) == false)
            {
                lock (this)
                {
                    if (this.ContainsKey(currentHook.Id) == false)
                    {
                        this.Add(currentHook.Id, currentHook);
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
                            this.Add(currentHook.Id, currentHook);
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
                using (this[item]) ;
            }
        }

        internal void ClearAll()
        {
            Clear();
        }
    }
}
