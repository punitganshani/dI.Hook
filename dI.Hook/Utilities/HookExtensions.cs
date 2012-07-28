using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dIHook.Objects.Attributes;
using dIHook.Builder;
using dIHook.Objects;

namespace dIHook.Utilities
{
    internal static class HookExtensions
    {
        internal static Func<T, bool> FormatSearchClause<T>(SearchBy searchBy, Operator op, string searchText) where T : IHook
        {
            // default value of the predicate
            Func<T, bool> predicate = new Func<T, bool>(x => true);

            if (string.IsNullOrEmpty(searchText) || searchText == "*")
                return predicate;

            switch (searchBy)
            {
                case SearchBy.Name:
                    if (op == Operator.Equals)
                        predicate = new Func<T, bool>(x => x.Name.Equals(searchText, StringComparison.InvariantCulture));
                    else if (op == Operator.Like)
                        predicate = new Func<T, bool>(x => x.Name.Contains(searchText));
                    else if (op == Operator.NotLike)
                        predicate = new Func<T, bool>(x => !x.Name.Contains(searchText));
                    break;
                case SearchBy.Guid:
                    Guid searchGuid = new Guid(searchText);
                    if (op == Operator.Equals)
                        predicate = new Func<T, bool>(x => x.Id == searchGuid);
                    else if (op == Operator.Like)
                        predicate = new Func<T, bool>(x => x.Id == searchGuid);
                    else if (op == Operator.NotLike)
                        predicate = new Func<T, bool>(x => x.Id != searchGuid);
                    break;
                default:
                    throw new InvalidOperationException("Invalid SearchBy value" + searchBy);
            }

            return predicate;
        }

        internal static List<T> GetHooksForCurrentMethod<T>(this HookRepository<T> repository) where T : IHook
        {
            int level = 3;
            if (ReflectionHelper.GetAttributeIfAny<RemoveAllHooks>(level))
                return new List<T>();

            List<T> source = repository.Hooks.ToList();

            if (ReflectionHelper.GetAttributeIfAny<AddHookType>(level))
            {
                var addHooks = ReflectionHelper.GetAttributeHookType<AddHookType, T>(level);
                source.AddRange(addHooks);
            }

            if (ReflectionHelper.GetAttributeIfAny<RemoveHook>(level))
            {
                var removeHooks = ReflectionHelper.GetAttributeOfType<RemoveHook>(level).Where(x => x.HookName != null);
                List<string> removeNames = new List<string>();
                foreach (var item in removeHooks)
                {
                    if (item.HookName != null)
                        removeNames.AddRange(item.HookName);
                }
                if (removeNames.Count != 0)
                    source.RemoveAll(x => removeNames.Contains(x.Name));
            }

            if (ReflectionHelper.GetAttributeIfAny<RemoveHookType>(level))
            {
                var removeHooksTypes = ReflectionHelper.GetAttributeHookType<RemoveHookType, T>(level);
                foreach (var item in removeHooksTypes)
                    source.RemoveAll(x => x.GetType() == item.GetType());
            }
            return source;
        }

        internal static List<Lazy<T>> GetHooksForCurrentMethodLazy<T>(this LazyHookRepository<T> repository) where T : IHook
        {
            int level = 3;
            if (ReflectionHelper.GetAttributeIfAny<RemoveAllHooks>(level))
                return new List<Lazy<T>>();

            List<Lazy<T>> source = repository.LazyHooks;

            if (ReflectionHelper.GetAttributeIfAny<AddHookType>(level))
            {
                var addHooks = ReflectionHelper.GetAttributeHookTypeLazy<AddHookType, T>(level);
                source.AddRange(addHooks);
            }

            if (ReflectionHelper.GetAttributeIfAny<RemoveHook>(level))
            {
                var removeHooks = ReflectionHelper.GetAttributeOfType<RemoveHook>(level).Where(x => x.HookName != null);
                List<string> removeNames = new List<string>();
                foreach (var item in removeHooks)
                {
                    if (item.HookName != null)
                        removeNames.AddRange(item.HookName);
                }
                if (removeNames.Count != 0)
                    source.RemoveAll(x => x.IsValueCreated && removeNames.Contains(x.Value.Name));
            }

            if (ReflectionHelper.GetAttributeIfAny<RemoveHookType>(level))
            {
                var removeHooksTypes = ReflectionHelper.GetAttributeHookTypeLazy<RemoveHookType, T>(level);
                foreach (var item in removeHooksTypes)
                    source.RemoveAll(x => x.IsValueCreated && x.GetType() == item.GetType());
            }
            return source;
        }
    }
}
