using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dIHook.Objects.Attributes;
using dIHook.Builder;
using dIHook.Objects;
using dIHook.UnitTests.Helper;
using dIHook.UnitTests.Hooks;

namespace dIHook.UnitTests.LazyHookRepositoryTests
{
    [TestClass]
    public class AddHookTests
    {
        IHookRepository<IHook> hookRepository;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            hookRepository = HookHelper.GetRepository<IHook>(true);
        }

        [TestMethod]
        public void Test_Lazy_AddHookObject()
        {
            hookRepository.Add(new LogHook());
            Assert.AreEqual(1, hookRepository.Hooks.Length);
        }

        [TestMethod]
        public void Test_Lazy_AddHookList()
        {
            hookRepository.Add(new List<IHook> { new LogHook(), new DiagnosticsHook()}) ;

            Assert.AreEqual(2, hookRepository.Hooks.Length);
        }

        [TestMethod]
        public void Test_Lazy_AddHookArray()
        {
            hookRepository.Add(new IHook [] { new LogHook(), new DiagnosticsHook() });

            Assert.AreEqual(2, hookRepository.Hooks.Length);
        }

        [TestMethod]
        public void Test_Lazy_AddHookType()
        {
            hookRepository.Add(typeof(LogHook));
            hookRepository.Add(typeof(DiagnosticsHook));

            Assert.AreEqual(2, hookRepository.Hooks.Length);
        }

        [TestMethod]
        public void Test_Lazy_AddSameHookWithDifferentGuid()
        {
            LogHook logHook1 = new LogHook(); /* Default Guid */
            LogHook logHook2 = new LogHook();
            logHook2.Id = new Guid("B9D2D1E3-6BAD-47C3-AEFC-3ABFAAB210F8");

            hookRepository.Add(new[] { logHook1, logHook2 });

            Assert.AreEqual(2, hookRepository.Hooks.Length);
        }

        [TestMethod]
        public void Test_Lazy_AddSameHookWithDifferentName()
        {
            LogHook logHook1 = new LogHook(); /* Default Guid, Name */
            LogHook logHook2 = new LogHook();
            logHook2.Name = "LogHook2";

            hookRepository.Add(new[] { logHook1, logHook2 });

            Assert.AreNotEqual(2, hookRepository.Hooks.Length); /* Comparison is done based on Guid and not Name */
        }

        [TestMethod]
        public void Test_Lazy_AddHookBySearch()
        {
            hookRepository.Add(SearchScope.CallingAssembly, SearchBy.Name, Operator.Like, "Log");
            Assert.AreEqual(1, hookRepository.Hooks.Length);
        }

        [TestMethod]
        [AddHookType(typeof(LogHook))]
        public void Test_Lazy_AddHookByAttribute()
        {
            hookRepository.RemoveAll();
            Assert.AreEqual(0, hookRepository.Hooks.Length);
        }

    }
}
