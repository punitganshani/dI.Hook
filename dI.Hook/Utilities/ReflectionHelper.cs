using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using dIHook.Objects.Attributes;
using dIHook.Builder;
using dIHook.Objects;

namespace dIHook.Utilities
{
    public static class ReflectionHelper
    {
        static ReflectionHelper()
        {
            TypeAddHook = typeof(AddHookType);
            TypeRemoveHook = typeof(RemoveHookType);
            TypeIHook = typeof(IHook);
            TypeRemoveHookWithName = typeof(RemoveHook);
        }

        static Type TypeRemoveHookWithName;
        static Type TypeAddHook;
        static Type TypeRemoveHook;
        static Type TypeIHook;

        public static bool GetAttributeIfAny<T>(int frameNumber = 2) where T : HookAttribute
        {
            // get call stack
            StackTrace stackTrace = new StackTrace();
            MethodBase callingMethodInfo = stackTrace.GetFrame(frameNumber).GetMethod();

            var attributes = callingMethodInfo.GetCustomAttributes(typeof(T), true);

            if (attributes != null && attributes.Length > 0) // some ignore attribute found
                return true;
            else
                return false;
        }

        public static List<X> GetAttributeHookType<T, X>(int frameNumber = 2) where T : HookAttribute 
                                                                              where X : IHook
        {
            // get call stack
            StackTrace stackTrace = new StackTrace();
            MethodBase callingMethodInfo = stackTrace.GetFrame(frameNumber).GetMethod();

            List<X> hooks = new List<X>();

            Type TypeT = typeof(T);
            bool isAddHook = TypeT == TypeAddHook;
            bool isRemoveHook = TypeT == TypeRemoveHook;

            var attributes = callingMethodInfo.GetCustomAttributes(typeof(T), true);

            if (attributes != null && attributes.Length > 0) // some ignore attribute found
            {
                foreach (var attribute in attributes)
                {
                    if (isAddHook)
                    {
                        var addHook = (AddHookType)attributes[0];
                        if (addHook.HookType.GetInterface("IHook", false) == null)
                            throw new InvalidCastException("AddHook attribute has a non IHook type in method:" + callingMethodInfo.Name);

                        var hookObject = Activator.CreateInstance(addHook.HookType);

                        hooks.Add((X)hookObject);
                    }
                    else if (isRemoveHook)
                    {
                        var removeHook = (RemoveHookType)attributes[0];
                        if (removeHook.HookType.GetInterface("IHook", false) == null)
                            throw new InvalidCastException("RemoveHook attribute has a non IHook type in method:" + callingMethodInfo.Name);

                        var hookObject = Activator.CreateInstance(removeHook.HookType);

                        hooks.Add((X)hookObject);
                    }
                }
            }

            return hooks;
        }

        public static List<T> GetAttributeOfType<T>(int frameNumber = 2) where T : HookAttribute
        {
            Type typeT = typeof(T);

            // get call stack
            StackTrace stackTrace = new StackTrace();
            MethodBase callingMethodInfo = stackTrace.GetFrame(frameNumber).GetMethod();

            var attributes = callingMethodInfo.GetCustomAttributes(typeof(T), true);

            if (attributes != null && attributes.Length > 0) // some ignore attribute found
                return attributes.Cast<T>().ToList();

            return new List<T>();
        }
    }
}
