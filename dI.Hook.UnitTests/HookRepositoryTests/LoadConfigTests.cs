using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dIHook.Objects;
using dIHook.UnitTests.Helper;

namespace dIHook.UnitTests.HookRepositoryTests
{
    [TestClass]
    public class LoadConfigTests
    {
        IHookRepository<IHook> hookRepository;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            hookRepository = HookHelper.GetRepository<IHook>();
        }

        [TestMethod]
        public void Test_Standard_LoadFromConfigFile()
        {
            hookRepository.RemoveAll();
            hookRepository.LoadConfiguration();

            Assert.AreEqual(2, hookRepository.Hooks.Length);
        }

        [TestMethod]
        public void Test_Standard_LoadFromConfigFileWithRepositoryName()
        {
            hookRepository.RemoveAll();
            hookRepository.LoadConfiguration("dIHookConfiguration", "productionRepository");

            Assert.AreEqual(1, hookRepository.Hooks.Length);
        }

        [TestMethod]
        public void Test_Standard_LoadFromConfigFileWithRepositoryNameDisabled()
        {
            hookRepository.RemoveAll();
            hookRepository.LoadConfiguration("dIHookConfiguration", "productionRepositoryDisabled");

            Assert.AreEqual(0, hookRepository.Hooks.Length);
        }
    }
}
