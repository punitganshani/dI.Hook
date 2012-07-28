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
    internal static class ReflectionHelper
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

        internal static bool GetAttributeIfAny<T>(int frameNumber = 2) where T : HookAttribute
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

        internal static List<X> GetAttributeHookType<T, X>(int frameNumber = 2)
            where T : HookAttribute
            where X : IHook
        {
            // get call stack
            StackTrace stackTrace = new StackTrace();
            MethodBase callingMethodInfo = stackTrace.GetFrame(frameNumber).GetMethod();

            List<X> hooks = new List<X>();

            Type TypeT = typeof(T);
            var attributes = callingMethodInfo.GetCustomAttributes(typeof(T), true);

            if (attributes != null && attributes.Length > 0) // some ignore attribute found
            {
                foreach (var attribute in attributes)
                {
                    if (attribute is AddHookType)
                    {
                        var addHook = (AddHookType)attribute;
                        if (addHook.HookType.GetInterface("IHook", false) == null)
                            throw new InvalidCastException("AddHook attribute has a non IHook type in method:" + callingMethodInfo.Name);

                        var hookObject = Activator.CreateInstance(addHook.HookType);
                        hooks.Add((X)hookObject);
                    }
                    else if (attribute is RemoveHookType)
                    {
                        var removeHook = (RemoveHookType)attribute;
                        if (removeHook.HookType.GetInterface("IHook", false) == null)
                            throw new InvalidCastException("RemoveHook attribute has a non IHook type in method:" + callingMethodInfo.Name);

                        var hookObject = Activator.CreateInstance(removeHook.HookType);
                        hooks.Add((X)hookObject);
                    }
                }
            }

            return hooks;
        }

        internal static List<Lazy<X>> GetAttributeHookTypeLazy<T, X>(int frameNumber = 2)
            where T : HookAttribute
            where X : IHook
        {
            // get call stack
            StackTrace stackTrace = new StackTrace();
            MethodBase callingMethodInfo = stackTrace.GetFrame(frameNumber).GetMethod();

            List<Lazy<X>> hooks = new List<Lazy<X>>();

            Type TypeT = typeof(T);
            var attributes = callingMethodInfo.GetCustomAttributes(typeof(T), true);

            if (attributes != null && attributes.Length > 0) // some ignore attribute found
            {
                foreach (var attribute in attributes)
                {
                    if (attribute is AddHookType)
                    {
                        var addHook = (AddHookType)attribute;
                        if (addHook.HookType.GetInterface("IHook", false) == null)
                            throw new InvalidCastException("AddHook attribute has a non IHook type in method:" + callingMethodInfo.Name);
                        hooks.Add(new Lazy<X>(() => { return (X)Activator.CreateInstance(addHook.HookType); }));
                    }
                    else if (attribute is RemoveHookType)
                    {
                        var removeHook = (RemoveHookType)attribute;
                        if (removeHook.HookType.GetInterface("IHook", false) == null)
                            throw new InvalidCastException("RemoveHook attribute has a non IHook type in method:" + callingMethodInfo.Name);
                        hooks.Add(new Lazy<X>(() => { return (X)Activator.CreateInstance(removeHook.HookType); ; }));
                    }
                }
            }

            return hooks;
        }

        internal static List<T> GetAttributeOfType<T>(int frameNumber = 2) where T : HookAttribute
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

        internal static bool HasIdentifierAttribute(this Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(HookIdentifier), true);
            if (attributes != null && attributes.Length > 0)
                return true;

            return false;
        }

        internal static Guid GetHookIdenfier(this Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(HookIdentifier), true);
            if (attributes != null && attributes.Length > 0)
            {
                if (attributes[0] is HookIdentifier)
                {
                    var hookIdentifierAttribute = attributes[0] as HookIdentifier;
                    return hookIdentifierAttribute.Id;
                }
            }
            return Guid.Empty;
        }
    }
}
