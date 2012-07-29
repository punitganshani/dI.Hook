using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dIHook.Objects.Attributes;
using dIHook.Builder;
using dIHook.Objects;
using dIHook.UnitTests.Helper;
using dIHook.UnitTests.Hooks;

namespace dIHook.UnitTests.HookRepositoryTests
{
    [TestClass]
    public class RemoveHookTests
    {
        IHookRepository<IHook> hookRepository;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            hookRepository = HookHelper.GetRepository<IHook>();
            hookRepository.Add(new IHook[] { new LogHook(), new DiagnosticsHook() });
        }

        [TestMethod]
        [RemoveAllHooks]
        public void Test_Standard_RemoveHooks()
        {
            int invokedHooks = hookRepository.InvokeAll();
            Assert.AreEqual(0, invokedHooks);
        }

        [TestMethod]
        [RemoveHook(new[] { "LogHook" })]
        public void Test_Standard_RemoveHooksWithName()
        {
            int invokedHooks = hookRepository.InvokeAll();

            Assert.AreEqual(1, invokedHooks);
        }

        [TestMethod]
        [RemoveHook(new[] { "LogHook", "DiagnosticsHook" })]
        public void Test_Standard_RemoveHooksWithName_MultipleHooks()
        {
            int invokedHooks = hookRepository.InvokeAll();
            Assert.AreEqual(0, invokedHooks);
        }

        [TestMethod]
        [RemoveHook("LogHook")]
        [RemoveHook("DiagnosticsHook")]
        public void Test_Standard_RemoveHooksWithName_MultipleAttributesInMethod()
        {
            int invokedHooks = hookRepository.InvokeAll();
            Assert.AreEqual(0, invokedHooks);
        }

        [TestMethod]
        [RemoveHookType(typeof(LogHook))]
        public void Test_Standard_RemoveHooksWithType()
        {
            int invokedHooks = hookRepository.InvokeAll();
            Assert.AreEqual(hookRepository.Hooks.Length - 1, invokedHooks);
        }

        [TestMethod]
        [RemoveHookType(typeof(LogHook))]
        [RemoveHookType(typeof(DiagnosticsHook))]
        [RemoveHookType(typeof(LogHook))]
        public void Test_Standard_AddRemoveHookCombination()
        {
            int invokedHooks = hookRepository.InvokeAll();
            Assert.AreEqual(0, invokedHooks);
        }

        [TestMethod]
        public void Test_Standard_RemoveHookObject()
        {
            hookRepository.Remove(new LogHook());
            Assert.AreEqual(1, hookRepository.Hooks.Length);
        }

        [TestMethod]
        public void Test_Standard_RemoveHookList()
        {
            hookRepository.Remove(new List<IHook> { new LogHook(), new DiagnosticsHook() });

            Assert.AreEqual(0, hookRepository.Hooks.Length);
        }

        [TestMethod]
        public void Test_Standard_RemoveHookArray()
        {
            hookRepository.Remove(new IHook[] { new LogHook(), new DiagnosticsHook() });

            Assert.AreEqual(0, hookRepository.Hooks.Length);
        }

        [TestMethod]
        public void Test_Standard_RemoveHookType()
        {
            hookRepository.Remove(typeof(LogHook));
            hookRepository.Remove(typeof(DiagnosticsHook));

            Assert.AreEqual(0, hookRepository.Hooks.Length);
        }

        [TestMethod]
        public void Test_Lazy_RemoveAll()
        {
            hookRepository.RemoveAll();
            Assert.AreEqual(0, hookRepository.Hooks.Length);
        }
    }
}
