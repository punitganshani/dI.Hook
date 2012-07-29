using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using dIHook.Configuration;
using dIHook.Objects;
using dIHook.Objects.Attributes;
using dIHook.Utilities;

namespace dIHook.Builder
{
    internal class LazyHookRepository<T> : IHookRepository<T> where T : IHook
    {
        private LazyDictionary<T> _hook;

        internal List<Lazy<T>> LazyHooks
        {
            get { return _hook.Values.ToList(); }
        }

        public T[] Hooks
        {
            get { return _hook.Values.Select(x => x.Value).ToArray(); }
        }

        public LazyHookRepository()
        {
            _hook = new LazyDictionary<T>();
        }

        public void Add(T hook)
        {
            ArgumentHelper.ValidateNotNull<T>(hook, "hook");
            _hook.Add(hook);
        }

        public void Add(IEnumerable<T> hooks)
        {
            ArgumentHelper.ValidateNotNull<IEnumerable<T>>(hooks, "hooks");
            _hook.AddRange(hooks);
        }

        public void Add(T[] hooks)
        {
            ArgumentHelper.ValidateNotNull<T[]>(hooks, "hooks");
            _hook.AddRange(hooks);
        }

        public void Add(Type type)
        {
            if (type.GetInterface("IHook") == null)
                throw new InvalidCastException("Invalid data type.");

            _hook.Add(type);
        }

        public void Add(SearchScope scope, SearchBy searchBy, Operator op, string searchText)
        {
            var searchPredicate = HookExtensions.FormatSearchClause<T>(searchBy, op, searchText);
            switch (scope)
            {
                case SearchScope.ExecutingAssembly:
                    _hook.AddRange(Assembly.GetExecutingAssembly().GetHooks(searchPredicate));
                    break;
                case SearchScope.EntryAssembly:
                    _hook.AddRange(Assembly.GetEntryAssembly().GetHooks(searchPredicate));
                    break;
                case SearchScope.CallingAssembly:
                    _hook.AddRange(Assembly.GetCallingAssembly().GetHooks(searchPredicate));
                    break;
                case SearchScope.CurrentDirectory:
                    List<Assembly> assemblies = DirectoryExtensions.GetAssemblies(Environment.CurrentDirectory, "*.dll");
                    assemblies.AddRange(DirectoryExtensions.GetAssemblies(Environment.CurrentDirectory, "*.exe"));
                    foreach (var item in assemblies)
                    {
                        _hook.AddRange(item.GetHooks(searchPredicate));
                    }
                    break;
                default:
                    throw new InvalidOperationException("Invalid SearchScope: " + scope);
            }
        }

        public void LoadConfiguration(string sectionName = "repositories", string repositoryName = "default", bool throwExceptionOnError = true)
        {
            try
            {
                dIHookConfigurationSection serviceConfigSection = ConfigurationManager.GetSection(sectionName) as dIHookConfigurationSection;
                if (serviceConfigSection == null && throwExceptionOnError)
                    throw new ConfigurationErrorsException(String.Format("dIHookConfigurationSection with name {0} not found in configuration file", sectionName));
                else
                {
                    var repository = serviceConfigSection.Repositories[repositoryName];
                    if (repository == null && throwExceptionOnError)
                        throw new ConfigurationErrorsException(String.Format("Repository with name {0} not found in dIHookConfigurationSection", repositoryName));
                    else
                    {
                        try
                        {
                            if (!repository.IsEnabled)
                                return;

                            var hookList = repository.Hooks.ToList();
                            hookList.ForEach(x =>
                            {
                                _hook.Add(x.GetHookType());
                            });
                        }
                        catch (Exception)
                        {
                            if (throwExceptionOnError) throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                if (throwExceptionOnError) throw;
            }
        }

        public int InvokeAll(params object[] inputParams)
        {
            List<Lazy<T>> hookForThisInstance = HookExtensions.GetHooksForCurrentMethodLazy(this);

            int count = 0;
            foreach (var hook in hookForThisInstance)
            {
                hook.Value.OnInvoke(inputParams);
                count++;
            }

            return count;
        }

        public int InvokeWhere(Func<T, bool> predicate, params object[] inputParams)
        {
            List<Lazy<T>> hookForThisInstance = HookExtensions.GetHooksForCurrentMethodLazy(this);

            int count = 0;
            foreach (var hook in hookForThisInstance)
            {
                // If predicate evaluates to 'true' execute the OnInvoke method
                if (predicate.Invoke(hook.Value))
                {
                    hook.Value.OnInvoke(inputParams);
                    count++;
                }
            }

            return count;
        }

        public int InvokeWhen(Func<bool> predicate, params object[] inputParams)
        {
            List<Lazy<T>> hookForThisInstance = HookExtensions.GetHooksForCurrentMethodLazy(this);

            int count = 0;
            // If predicate evaluates to 'true' execute the OnInvoke method
            if (predicate.Invoke())
            {
                foreach (var hook in hookForThisInstance)
                {
                    hook.Value.OnInvoke(inputParams);
                    count++;
                }
            }

            return count;
        }

        public int InvokeWhen(Func<bool> predicate, Func<T, bool> hookPredicate, params object[] inputParams)
        {
            List<Lazy<T>> hookForThisInstance = HookExtensions.GetHooksForCurrentMethodLazy(this);

            int count = 0;
            // If predicate evaluates to 'true' execute the OnInvoke method
            if (predicate.Invoke())
            {
                foreach (var hook in hookForThisInstance)
                {
                    if (hookPredicate.Invoke(hook.Value))
                    {
                        hook.Value.OnInvoke(inputParams);
                        count++;
                    }
                }
            }

            return count;
        }

        public void RemoveAll()
        {
            _hook.ClearAll();
        }

        public void Remove(T hook)
        {
            _hook.Remove(hook);
        }

        public void Remove(IEnumerable<T> hooks)
        {
            _hook.Remove(hooks);
        }

        public void Remove(T[] hooks)
        {
            _hook.Remove(hooks);
        }

        public void Remove(Type type)
        {
            if (type.GetInterface("IHook") == null)
                throw new InvalidCastException("Invalid data type.");

            var objT = (T)Activator.CreateInstance(type);
            _hook.Remove(objT);
        }

        public T[] Get(Func<T, bool> predicate)
        {
            List<Lazy<T>> hookForThisInstance = HookExtensions.GetHooksForCurrentMethodLazy(this);
            List<T> hookObjects = new List<T>();
            foreach (var hook in hookForThisInstance)
            {
                if (predicate.Invoke(hook.Value))
                    hookObjects.Add(hook.Value);
            }
            return hookObjects.ToArray();
        }

        public T[] Get(Type type)
        {
            if (type.GetInterface("IHook") == null)
                throw new InvalidCastException("Only data types inheriting from IHook are permitted in this repository");

            List<Lazy<T>> hookForThisInstance = HookExtensions.GetHooksForCurrentMethodLazy(this);
            List<T> hookObjects = new List<T>();
            foreach (var hook in hookForThisInstance)
            {
                if (hook.Value.GetType() == type)
                    hookObjects.Add(hook.Value);
            }
            return hookObjects.ToArray();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposse)
        {
            // Dispose all hooks
            using (_hook) ;
        }

        ~LazyHookRepository()
        {
            Dispose(true);
        }
    }
}
