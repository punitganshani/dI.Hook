using System;
using dIHook.Objects;
using dIHook.UnitTests.Helper;
using dIHook.UnitTests.Hooks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dIHook.UnitTests.HookRepositoryTests
{
    [TestClass]
    public class InvokeParallelTest
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
        public void Test_Standard_InvokeAllAsParallel()
        {
            int invokedCount = hookRepository.InvokeAllAsParallel("CalledByTestWithDifferentValues", 2);
            Assert.AreEqual(2, invokedCount);
        }

        [TestMethod]
        public void Test_Standard_InvokeAllAsParallelWithoutParameters()
        {
            int invokedCount = hookRepository.InvokeAllAsParallel();
            Assert.AreEqual(2, invokedCount);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void Test_Standard_InvokeAllAsParallelWithExceptionRaisedInFirstHook()
        {
            // will not block diagnostichook as it is parallel and will throw exception
            int invokedCount = hookRepository.InvokeAllAsParallel("RaiseLogException");
            Assert.AreEqual(1, invokedCount);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void Test_Standard_InvokeAllAsParallelWithExceptionRaisedInSecondHook()
        {
            // will not block LogHook as it is parallel but should throw exception
            int invokedCount = hookRepository.InvokeAllAsParallel("RaiseDiagnosticException");
            Assert.AreEqual(1, invokedCount);
        }

        [TestMethod]
        public void Test_Standard_InvokeWhere()
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
        public void Test_Standard_InvokeWhenWithExternalFilterTrue()
        {
            int invokedCount = hookRepository.InvokeWhen(() =>
            {
                return true;
            });
            Assert.AreEqual(2, invokedCount);
        }

        [TestMethod]
        public void Test_Standard_InvokeWhenWithExternalFilterFalse()
        {
            int invokedCount = hookRepository.InvokeWhen(() =>
            {
                return false;
            });
            Assert.AreEqual(0, invokedCount);
        }

        [TestMethod]
        public void Test_Standard_InvokeWhenWithExternalFilterTrueAndHookFilter()
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
        public void Test_Standard_InvokeWhenWithExternalFilterFalseAndHookFilter()
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
        public void Test_Standard_InvokeWhenWithExternalFilterTrueAndHookFilterWithParams()
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
