using System;
using dIHook.Objects;
using dIHook.UnitTests.Helper;
using dIHook.UnitTests.Hooks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dIHook.UnitTests.LazyHookRepositoryTests
{
    [TestClass]
    public class InvokeTest
    {
        IHookRepository<IHook> hookRepository;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            hookRepository = HookHelper.GetRepository<IHook>(true);
            hookRepository.Add(typeof(LogHook));
            hookRepository.Add(typeof(DiagnosticsHook));
        }

        [TestMethod]
        public void Test_Lazy_InvokeAll()
        {
            int invokedCount = hookRepository.InvokeAll("CalledByTestWithDifferentValues", 2);
            Assert.AreEqual(2, invokedCount);
        }

        [TestMethod]
        public void Test_Lazy_InvokeAllWithoutParameters()
        {
            int invokedCount = hookRepository.InvokeAll();
            Assert.AreEqual(2, invokedCount);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_Lazy_InvokeAllWithExceptionRaisedInFirstHook()
        {
            // should block diagnostichook as well and throw exception
            int invokedCount = hookRepository.InvokeAll("RaiseLogException");
            Assert.AreEqual(0, invokedCount);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_Lazy_InvokeAllWithExceptionRaisedInSecondHook()
        {
            // should not block LogHook but should throw exception
            int invokedCount = hookRepository.InvokeAll("RaiseDiagnosticException");
            Assert.AreEqual(1, invokedCount);
        }

        [TestMethod]
        public void Test_Lazy_InvokeWhere()
        {
            int invokedCount = hookRepository.InvokeWhere(x =>
            {
                if (x is IAdditionalInterface)
                {
                    return (x as IAdditionalInterface).IQ == "IQ.001";
                }
                return false;
            });
            Assert.AreEqual(1, invokedCount);
        }

        [TestMethod]
        public void Test_Lazy_InvokeWhenWithExternalFilterTrue()
        {
            int invokedCount = hookRepository.InvokeWhen(() =>
            {
                return true;
            });
            Assert.AreEqual(2, invokedCount);
        }

        [TestMethod]
        public void Test_Lazy_InvokeWhenWithExternalFilterFalse()
        {
            int invokedCount = hookRepository.InvokeWhen(() =>
            {
                return false;
            });
            Assert.AreEqual(0, invokedCount);
        }

        [TestMethod]
        public void Test_Lazy_InvokeWhenWithExternalFilterTrueAndHookFilter()
        {
            int invokedCount = hookRepository.InvokeWhen(() => { return true; },
                                                         (x) =>
                                                         {
                                                             if (x is IAdditionalInterface)
                                                             {
                                                                 return (x as IAdditionalInterface).IQ == "IQ.001";
                                                             }
                                                             return false;
                                                         });
            Assert.AreEqual(1, invokedCount);
        }

        [TestMethod]
        public void Test_Lazy_InvokeWhenWithExternalFilterFalseAndHookFilter()
        {
            int invokedCount = hookRepository.InvokeWhen(() => { return false; },
                                                         (x) =>
                                                         {
                                                             if (x is IAdditionalInterface)
                                                             {
                                                                 return (x as IAdditionalInterface).IQ == "IQ.001";
                                                             }
                                                             return false;
                                                         });
            Assert.AreEqual(0, invokedCount);
        }

        [TestMethod]
        public void Test_Lazy_InvokeWhenWithExternalFilterTrueAndHookFilterWithParams()
        {
            int invokedCount = hookRepository.InvokeWhen(() => { return true; },
                                                         (x) =>
                                                         {
                                                             if (x is IAdditionalInterface)
                                                             {
                                                                 return (x as IAdditionalInterface).IQ == "IQ.001";
                                                             }
                                                             return false;
                                                         },
                                                         "TestWithParams", 2);
            Assert.AreEqual(1, invokedCount);
        }
    }
}
