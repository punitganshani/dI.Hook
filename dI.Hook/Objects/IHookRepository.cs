using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dIHook.Builder;

namespace dIHook.Objects
{
    /// <summary>
    /// This interface can be used for multiple implementations of IHookRepository.
    /// </summary>
    public interface IHookRepository<T> : IDisposable where T: IHook
    {
        #region Properties
        /// <summary>
        /// Hooks in the repository
        /// </summary>
        T[] Hooks { get; }
        #endregion

        #region Add Hooks
        /// <summary>
        /// Add a hook object into repository
        /// </summary>
        /// <param name="hook">An implementation of type IHook</param>
        void Add(T hook);

        /// <summary>
        /// Add multiple hook objects into repository
        /// </summary>
        /// <param name="hooks">An array, or List, or any collection of hooks of type IEnumerable</param>
        void Add(IEnumerable<T> hooks);

        /// <summary>
        /// Add multiple hook objects into repository
        /// </summary>
        /// <param name="hooks">An array of hooks</param>
        void Add(T[] hooks);

        /// <summary>
        /// Add a hook type into repository
        /// </summary>
        /// <typeparam name="T">Type of IHook</typeparam>
        void Add(Type type);

        /// <summary>
        /// Add hooks by searching through hook objects in an assembly or current directory with specific name
        /// </summary>
        /// <param name="scope">Directory or an Assembly</param>
        /// <param name="searchBy">Currently, SearchBy only Name</param>
        /// <param name="op">Basic operations like Equals, Like and NotLike</param>
        /// <param name="searchText">Search text</param>
        void Add(SearchScope scope, SearchBy searchBy, Operator op, string searchText);
        #endregion

        #region Load Configuration
        /// <summary>
        /// Load repository and hooks from a configuration file
        /// </summary>
        /// <param name="sectionName">Section Name in the configuration file</param>
        /// <param name="repositoryName">Repository Name</param>
        /// <param name="throwExceptionOnError">If true, throws error on any exception occured while loading configuration</param>
        void LoadConfiguration(string sectionName = "dIHookConfiguration", string repositoryName = "default", bool throwExceptionOnError = true);
        #endregion

        #region Invoke
        /// <summary>
        /// Invoke all the hooks int the repository.  If any of the hook execution, method OnInvoke(), fails other hooks are not executed.
        /// </summary>
        /// <returns>Return number of hooks invoked successfully</returns>
        /// <param name="inputParams">Parameters to the OnInvoke of a hook</param>
        int InvokeAll(params object[] inputParams);

        /// <summary>
        /// Invoke hooks that satisfy the condition mentioned in the predicate.
        /// </summary>
        /// <param name="predicate">Search criteria. If true for a hook, the OnInvoke() method of that hook will be invoked</param>
        /// <returns>Return number of hooks invoked successfully</returns>
        /// <param name="inputParams">Parameters to the OnInvoke of a hook</param>
        int InvokeWhere(Func<T, bool> predicate, params object[] inputParams);

        /// <summary>
        /// Invoke hooks only if the predicate condition is true.
        /// </summary>
        /// <param name="predicate">Search criteria. If true, the OnInvoke() method of all the hooks in repository will be invoked</param>
        /// <returns>Return number of hooks invoked successfully</returns>
        /// <param name="inputParams">Parameters to the OnInvoke of a hook</param>
        int InvokeWhen(Func<bool> predicate, params object[] inputParams);

        /// <summary>
        /// Invoke hooks only if the predicate and hook predicate condition are true.
        /// </summary>
        /// <param name="predicate">Search criteria that is not specific to a hook</param>
        /// <param name="hookPredicate">Search criteria that is specific to a hook</param>
        /// <param name="inputParams">Parameters to the OnInvoke of a hook</param>
        /// <returns>Return number of hooks invoked successfully</returns>
        int InvokeWhen(Func<bool> predicate, Func<T, bool> hookPredicate, params object[] inputParams);
        #endregion        

        #region Remove Hooks
        /// <summary>
        /// Removes all hooks from repository
        /// </summary>
        void RemoveAll();

        /// <summary>
        /// Removes a specific hook from the repository
        /// </summary>
        /// <param name="hook">Object of the hook to be removed</param>
        void Remove(T hook);

        /// <summary>
        /// Removes a set of hooks from the repository
        /// </summary>
        /// <param name="hooks">Collection of hooks to be removed</param>
        void Remove(IEnumerable<T> hooks);

        /// <summary>
        /// Removes a set of hooks from the repository
        /// </summary>
        /// <param name="hooks">Collection of hooks to be removed</param>
        void Remove(T[] hooks);

        /// <summary>
        /// Removes a particular hook type from the repository
        /// </summary>
        /// <typeparam name="T">Hook Type</typeparam>
        void Remove(Type type);
        #endregion

        #region Get Hook Instance
        /// <summary>
        /// Retrieves all hooks that meet the search criteria
        /// </summary>
        /// <param name="predicate">Search criteria to be executed on the repository</param>
        /// <returns>Array of hook objects</returns>
        T[] Get(Func<T, bool> predicate);

        /// <summary>
        /// Retrieves all hooks of a specified type
        /// </summary>
        /// <param name="type">Type of Hook</param>
        /// <returns>Array of hook objects</returns>
        T[] Get(Type type);
        #endregion
    }
}
