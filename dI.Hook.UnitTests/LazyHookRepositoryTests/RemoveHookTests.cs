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
    public class RemoveHookTests
    {
        IHookRepository<IHook> hookRepository;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            hookRepository = HookHelper.GetRepository<IHook>(true);
            hookRepository.Add(new IHook[] { new LogHook(), new DiagnosticsHook() });
        }

        [TestMethod]
        [RemoveAllHooks]
        public void Test_Lazy_RemoveHooks()
        {
            hookRepository.RemoveAll();
            hookRepository.Add(new IHook[] { new LogHook(), new DiagnosticsHook() });
            int invokedHooks = hookRepository.InvokeAll();
            Assert.AreEqual(0, invokedHooks);
        }

        [TestMethod]
        [RemoveHook(new[] { "LogHook" })]
        public void Test_Lazy_RemoveHooksWithName()
        {
            hookRepository.RemoveAll();
            hookRepository.Add(new IHook[] { new LogHook(), new DiagnosticsHook() });
            int invokedHooks = hookRepository.InvokeAll();
            Assert.AreEqual(1, invokedHooks);
        }

        [TestMethod]
        [RemoveHook(new[] { "LogHook", "DiagnosticsHook" })]
        public void Test_Lazy_RemoveHooksWithName_MultipleHooks()
        {
            hookRepository.RemoveAll();
            hookRepository.Add(new IHook[] { new LogHook(), new DiagnosticsHook() });

            int invokedHooks = hookRepository.InvokeAll();
            Assert.AreEqual(0, invokedHooks);
        }

        [TestMethod]
        [RemoveHook("LogHook")]
        [RemoveHook("DiagnosticsHook")]
        public void Test_Lazy_RemoveHooksWithName_MultipleAttributesInMethod()
        {
            hookRepository.RemoveAll();
            hookRepository.Add(new IHook[] { new LogHook(), new DiagnosticsHook() });

            int invokedHooks = hookRepository.InvokeAll();
            Assert.AreEqual(0, invokedHooks);
        }

        [TestMethod]
        [RemoveHookType(typeof(LogHook))]
        public void Test_Lazy_RemoveHooksWithType()
        {
            hookRepository.RemoveAll();
            hookRepository.Add(typeof(LogHook));
            hookRepository.Add(typeof(DiagnosticsHook));
            int invokedHooks = hookRepository.InvokeAll();
            Assert.AreEqual(1, invokedHooks);
        }

        [TestMethod]
        [RemoveHookType(typeof(LogHook))]
        [RemoveHookType(typeof(DiagnosticsHook))]
        [RemoveHookType(typeof(LogHook))]
        public void Test_Lazy_AddRemoveHookCombination()
        {
            hookRepository.RemoveAll();
            hookRepository.Add(new IHook[] { new LogHook(), new DiagnosticsHook() });

            int invokedHooks = hookRepository.InvokeAll();
            Assert.AreEqual(0, invokedHooks);
        }

        [TestMethod]
        public void Test_Lazy_RemoveHookObject()
        {
            hookRepository.RemoveAll();
            hookRepository.Add(new IHook[] { new LogHook(), new DiagnosticsHook() });

            hookRepository.Remove(new LogHook());
            Assert.AreEqual(1, hookRepository.Hooks.Length);
        }

        [TestMethod]
        public void Test_Lazy_RemoveHookList()
        {
            hookRepository.RemoveAll();
            hookRepository.Add(new IHook[] { new LogHook(), new DiagnosticsHook() });

            hookRepository.Remove(new List<IHook> { new LogHook(), new DiagnosticsHook() });

            Assert.AreEqual(0, hookRepository.Hooks.Length);
        }

        [TestMethod]
        public void Test_Lazy_RemoveHookArray()
        {
            hookRepository.RemoveAll();
            hookRepository.Add(new IHook[] { new LogHook(), new DiagnosticsHook() });

            hookRepository.Remove(new IHook[] { new LogHook(), new DiagnosticsHook() });

            Assert.AreEqual(0, hookRepository.Hooks.Length);
        }

        [TestMethod]
        public void Test_Lazy_RemoveHookType()
        {
            hookRepository.RemoveAll();
            hookRepository.Add(new IHook[] { new LogHook(), new DiagnosticsHook() });

            hookRepository.Remove(typeof(LogHook));
            hookRepository.Remove(typeof(DiagnosticsHook));

            Assert.AreEqual(0, hookRepository.Hooks.Length);
        }
    }
}
