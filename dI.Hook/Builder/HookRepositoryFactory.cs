using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dIHook.Objects;

namespace dIHook.Builder
{
    public class HookRepositoryFactory
    {
        public static IHookRepository<T> Create<T>() where T: IHook
        {
            return new HookRepository<T>();
        }
    }
}
