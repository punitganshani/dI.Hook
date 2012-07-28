using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dIHook.Objects;

namespace dIHook.Builder
{
    public class HookRepositoryFactory
    {
        public static IHookRepository<T> Create<T>(bool lazyLoad = false) where T : IHook
        {
            if (lazyLoad)
                return new LazyHookRepository<T>();
            else
                return new HookRepository<T>();
        }
    }
}
