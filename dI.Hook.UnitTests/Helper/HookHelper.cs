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
        public static IHookRepository<T> GetRepository<T>() where T : IHook
        {
            return HookRepositoryFactory.Create<T>();
        }
    }
}
