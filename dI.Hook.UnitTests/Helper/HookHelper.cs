using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dIHook.Builder;
using dIHook.Objects;

namespace dIHook.UnitTests.Helper
{
    public static class HookHelper
    {
        public static IHookRepository<T> GetRepository<T>(bool lazyLoad = false) where T : IHook
        {
            return HookRepositoryFactory.Create<T>(lazyLoad);
        }
    }
}
