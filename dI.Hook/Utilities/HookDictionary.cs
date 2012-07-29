using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dIHook.Builder;
using dIHook.Objects;
using dIHook.Objects.Attributes;

namespace dIHook.Utilities
{
    internal class HookDictionary<T> : Dictionary<Guid, T>, IDisposable
                                    where T : IHook
    {
        private Type _typeT;

        internal HookDictionary()
        {
            _typeT = typeof(T);
        }

        public void Add(T currentHook)
        {
            if (currentHook == null) return;

            Guid id = currentHook.GetIdOfHook();

            if (this.ContainsKey(id) == false)
            {
                lock (this)
                {
                    if (this.ContainsKey(currentHook.Id) == false)
                    {
                        this.Add(currentHook.Id, currentHook);
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
                            this.Add(id, currentHook);
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
            foreach (var item in this.Keys)
            {
                using (this[item]) ;
            }
        }

        internal void ClearAll()
        {
            this.Clear();
        }
    }
}
