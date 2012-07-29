using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using dIHook.Objects.Attributes;
using dIHook.Configuration;
using dIHook.Objects;
using dIHook.Utilities;
using System.Threading.Tasks;

namespace dIHook.Builder
{
    /// <summary>
    /// Default Hook Repository that stores hooks into a dictionary format and uses IHook.Name as the key in the dictionary.
    /// </summary>
    internal class HookRepository<T> : IHookRepository<T> where T: IHook
    {
        private HookDictionary<T> _hook;
         
        public T[] Hooks
        {
            get
            {
                if (_hook != null)
                    return _hook.Values.ToArray();

                return null;
            }
        }

        public HookRepository()
        {
            _hook = new HookDictionary<T>();
        }

        #region Add Hooks
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

            var objT = (T) Activator.CreateInstance(type);
            _hook.Add(objT);
        }

        public void Add(SearchScope scope, SearchBy searchBy, Operator op, string searchText)
        {
            var searchPredicate = HookExtensions.FormatSearchClause<T>(searchBy, op, searchText);
            switch (scope)
            {
                case SearchScope.ExecutingAssembly:
                    _hook.AddRange(Assembly.GetExecutingAssembly().GetHooks<T>(searchPredicate));
                    break;
                case SearchScope.EntryAssembly:
                    _hook.AddRange(Assembly.GetEntryAssembly().GetHooks<T>(searchPredicate));
                    break;
                case SearchScope.CallingAssembly:
                    _hook.AddRange(Assembly.GetCallingAssembly().GetHooks<T>(searchPredicate));
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
        #endregion

        #region Invoke Hooks
        public int InvokeAll(params object[] inputParams)
        {
            List<T> hookForThisInstance = HookExtensions.GetHooksForCurrentMethod(this);

            int count = 0;
            foreach (var hook in hookForThisInstance)
            {
                hook.OnInvoke(inputParams);
                count++;
            }

            return count;
        }

        public int InvokeWhere(Func<T, bool> predicate, params object[] inputParams)
        {
            List<T> hookForThisInstance = HookExtensions.GetHooksForCurrentMethod(this);
            int count = 0;
            foreach (var hook in hookForThisInstance)
            {
                // If predicate evaluates to 'true' execute the OnInvoke method
                if (predicate.Invoke(hook))
                {
                    hook.OnInvoke(inputParams);
                    count++;
                }
            }

            return count;
        }

        public int InvokeWhen(Func<bool> predicate, params object[] inputParams)
        {
            List<T> hookForThisInstance = HookExtensions.GetHooksForCurrentMethod(this);
            int count = 0;
            // If predicate evaluates to 'true' execute the OnInvoke method
            if (predicate.Invoke())
            {
                foreach (var hook in hookForThisInstance)
                {
                    hook.OnInvoke(inputParams);
                    count++;
                }
            }

            return count;
        }

        public int InvokeWhen(Func<bool> predicate, Func<T, bool> hookPredicate, params object[] inputParams)
        {
            List<T> hookForThisInstance = HookExtensions.GetHooksForCurrentMethod(this);
            int count = 0;
            // If predicate evaluates to 'true' execute the OnInvoke method
            if (predicate.Invoke())
            {
                foreach (var hook in hookForThisInstance)
                {
                    if (hookPredicate.Invoke(hook))
                    {
                        hook.OnInvoke(inputParams);
                        count++;
                    }
                }
            }

            return count;
        }
        #endregion

        #region Invoke Hooks As Parallel
        public int InvokeAllAsParallel(params object[] inputParams)
        {
            List<T> hookForThisInstance = HookExtensions.GetHooksForCurrentMethod(this);

            int count = 0;
            Parallel.ForEach(hookForThisInstance, hook =>
            {
                hook.OnInvoke(inputParams);
                count++;
            });

            return count;
        }

        public int InvokeWhereAsParallel(Func<T, bool> predicate, params object[] inputParams)
        {
            List<T> hookForThisInstance = HookExtensions.GetHooksForCurrentMethod(this);
            int count = 0;
            Parallel.ForEach(hookForThisInstance, hook =>
            {
                // If predicate evaluates to 'true' execute the OnInvoke method
                if (predicate.Invoke(hook))
                {
                    hook.OnInvoke(inputParams);
                    count++;
                }
            });

            return count;
        }

        public int InvokeWhenAsParallel(Func<bool> predicate, params object[] inputParams)
        {
            List<T> hookForThisInstance = HookExtensions.GetHooksForCurrentMethod(this);
            int count = 0;
            // If predicate evaluates to 'true' execute the OnInvoke method
            if (predicate.Invoke())
            {
                Parallel.ForEach(hookForThisInstance, hook =>
                {
                    hook.OnInvoke(inputParams);
                    count++;
                });
            }

            return count;
        }

        public int InvokeWhenAsParallel(Func<bool> predicate, Func<T, bool> hookPredicate, params object[] inputParams)
        {
            List<T> hookForThisInstance = HookExtensions.GetHooksForCurrentMethod(this);
            int count = 0;
            // If predicate evaluates to 'true' execute the OnInvoke method
            if (predicate.Invoke())
            {
                Parallel.ForEach(hookForThisInstance, hook =>
                {
                    if (hookPredicate.Invoke(hook))
                    {
                        hook.OnInvoke(inputParams);
                        count++;
                    }
                });
            }

            return count;
        }
        #endregion

        #region Load Hooks from config
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
                                var instance = x.CreateInstance<T>();
                                if (instance != null)
                                    Add(instance);
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
        #endregion

        #region Dispose
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

        ~HookRepository()
        {
            Dispose(true);
        }
        #endregion

        #region Remove Hooks
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
        #endregion

        #region Get Hooks
        public T[] Get(Func<T, bool> predicate)
        {
            List<T> hookForThisInstance = HookExtensions.GetHooksForCurrentMethod(this);
            List<T> hookObjects = new List<T>();
            foreach (var hook in hookForThisInstance)
            {
                if (predicate.Invoke(hook))
                    hookObjects.Add(hook);
            }
            return hookObjects.ToArray();
        }

        public T[] Get(Type type)
        {
            if (type.GetInterface("IHook") == null)
                throw new InvalidCastException("Only data types inheriting from IHook are permitted in this repository");

            List<T> hookForThisInstance = HookExtensions.GetHooksForCurrentMethod(this);
            List<T> hookObjects = new List<T>();
            foreach (var hook in hookForThisInstance)
            {
                if (hook.GetType() == type)
                    hookObjects.Add(hook);
            }
            return hookObjects.ToArray();
        }
        #endregion
    }
}
