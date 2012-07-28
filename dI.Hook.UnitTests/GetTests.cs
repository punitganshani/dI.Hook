using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dIHook.Objects;
using dIHook.UnitTests.Helper;
using dIHook.UnitTests.Hooks;

namespace dIHook.UnitTests
{
    [TestClass]
    public class GetTests
    {
        IHookRepository<IHook> hookRepository;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            hookRepository = HookHelper.GetRepository<IHook>();
            hookRepository.Add(typeof(LogHook));
            hookRepository.Add(typeof(DiagnosticsHook));
        }

        [TestMethod]
        public void Test_GetHookByType()
        {
            var hooksRetrieved = hookRepository.Get(x => x.GetType() == typeof(LogHook));
            Assert.AreEqual(1, hooksRetrieved.Length);
        }

        [TestMethod]
        public void Test_GetHookByName()
        {
            var hooksRetrieved = hookRepository.Get(x => x.Name != null && x.Name.Contains("Log"));
            Assert.AreEqual(1, hooksRetrieved.Length);
        }

        [TestMethod]
        public void Test_GetAllHooks()
        {
            var hooksRetrieved = hookRepository.Hooks;
            Assert.AreEqual(2, hooksRetrieved.Length);
        }
    }
}
