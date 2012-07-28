using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dIHook.Objects;
using dIHook.UnitTests.Helper;

namespace dIHook.UnitTests.LazyHookRepositoryTests
{
    [TestClass]
    public class LoadConfigTests
    {
        IHookRepository<IHook> hookRepository;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            hookRepository = HookHelper.GetRepository<IHook>(true);
        }

        [TestMethod]
        public void Test_Lazy_LoadFromConfigFile()
        {
            hookRepository.RemoveAll();
            hookRepository.LoadConfiguration();

            Assert.AreEqual(2, hookRepository.Hooks.Length);
        }

        [TestMethod]
        public void Test_Lazy_LoadFromConfigFileWithRepositoryName()
        {
            hookRepository.RemoveAll();
            hookRepository.LoadConfiguration("dIHookConfiguration", "productionRepository");

            Assert.AreEqual(1, hookRepository.Hooks.Length);
        }

        [TestMethod]
        public void Test_Lazy_LoadFromConfigFileWithRepositoryNameDisabled()
        {
            hookRepository.RemoveAll();
            hookRepository.LoadConfiguration("dIHookConfiguration", "productionRepositoryDisabled");

            Assert.AreEqual(0, hookRepository.Hooks.Length);
        }
    }
}
