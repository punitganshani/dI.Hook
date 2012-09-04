using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dIHook.Objects;
using System.Collections.ObjectModel;
using dIHook.Utilities;

namespace dIHook.Containers
{
    public sealed class Container : IContainer
    {
        private List<ContainerItem> _objects;

        public ReadOnlyCollection<Type> Types
        {
            get
            {
                return _objects.Select(x => x.InterfaceType).ToList().AsReadOnly();
            }
        }

        public ReadOnlyCollection<Guid> Keys
        {
            get
            {
                return _objects.Select(x => x.Key).ToList().AsReadOnly();
            }
        }

        public ReadOnlyCollection<ContainerItem> Collection
        {
            get
            {
                return _objects.AsReadOnly();
            }
        }

        public Container()
        {
            _objects = new List<ContainerItem>();
        }

        #region Register

        public void Register<TClass>() where TClass : class
        {
            Type typeClass = typeof(TClass);
            Register(typeClass, typeClass, null, false);
        }

        public void Register<TClass>(Guid key) where TClass : class
        {
            Type typeClass = typeof(TClass);
            Register(typeClass, typeClass, key, false);
        }

        public void Register<TInterface>(TInterface objectValue)
        {
            Type typeInterface = typeof(TInterface);
            _objects.Add(new ContainerItem(typeInterface, () => { return objectValue; }));
        }

        public void Register<TInterface>(Guid key, TInterface objectValue)
        {
            Type typeInterface = typeof(TInterface);
            _objects.Add(new ContainerItem(key, typeInterface, () => { return objectValue; }));
        }

        public void Register<TInterface, TImplementation>()
                    where TImplementation : class, TInterface
        {
            Type typeInterface = typeof(TInterface);
            Type typeImplementation = typeof(TImplementation);

            Register(typeInterface, typeImplementation);
        }

        public void Register<TInterface, TImplementation>(Guid guid)
             where TImplementation : class, TInterface
        {
            Type typeInterface = typeof(TInterface);
            Type typeImplementation = typeof(TImplementation);

            Register(typeInterface, typeImplementation, guid);
        }

        #endregion

        #region Resolve
        public TInterface Resolve<TInterface>() 
            where TInterface : class
        {
            Type typeInterface = typeof(TInterface);
            if (_objects.Contains(typeInterface))
                return (TInterface)_objects.GetByType(typeInterface).Value;
            else
            {
                Register<TInterface>(Guid.NewGuid());
                return (TInterface)_objects.GetByType(typeInterface).Value;
            }
        }

        public TInterface Resolve<TInterface>(Guid key)
            where TInterface : class
        {
            Type typeInterface = typeof(TInterface);
            if (_objects.Contains(typeInterface))
                return (TInterface)_objects.GetByTypeKey(typeInterface, key).Value;
            else
            {
                Register<TInterface>(key); // all related objects will have same key
                return (TInterface)_objects.GetByTypeKey(typeInterface, key).Value;
            }
        }
        #endregion

        [Obsolete("Use Resolve method instead of CreateInstance")]
        public TInterface CreateInstance<TInterface>() 
            where TInterface : class
        {
            Type typeInterface = typeof(TInterface);
            if (_objects.Contains(typeInterface))
                return (TInterface)_objects.GetByType(typeInterface).Value;
            else
            {
                Register<TInterface>(Guid.NewGuid()); // all related objects will have same key
                return (TInterface)_objects.GetByType(typeInterface).Value;
            }
        }

        #region Private Methods

        private Lazy<Object> GetOrCreateLazyObjectByKey(Type type, Guid? key, params object[] parameters)
        {
            if (_objects.Contains(type))
            {
                if (key.HasValue)
                    return _objects.GetByTypeKey(type, key.Value);
                else
                    return _objects.GetByType(type);
            }
            else
            {
                return new Lazy<object>(() =>
                {
                    if (parameters != null && parameters.Length > 0)
                        return Activator.CreateInstance(type, parameters);
                    else
                        return Activator.CreateInstance(type);
                });
            }
        }

        private Lazy<Object> GetOrCreateLazyObjectOfType(Type type, Guid? key, params object[] parameters)
        {
            if (_objects.Contains(type))
            {
                if (key.HasValue)
                    return _objects.GetByType(type);
                else
                    return _objects.GetByTypeKey(type, key.Value);
            }
            else
            {
                return new Lazy<object>(() =>
                {
                    if (parameters != null && parameters.Length > 0)
                        return Activator.CreateInstance(type, parameters);
                    else
                        return Activator.CreateInstance(type);
                });
            }
        }


        private Object GetOrCreateObjectOfType(Type type, Guid? key, params object[] parameters)
        {
            if (_objects.Contains(type))
            {
                if (key.HasValue)
                    return _objects.GetByTypeKey(type, key.Value).Value;
                else
                    return _objects.GetByType(type).Value;
            }
            else
            {
                if (parameters != null && parameters.Length > 0)
                    return Activator.CreateInstance(type, parameters);
                else
                    return Activator.CreateInstance(type);
            }
        }

        private void Register(Type typeInterface, Type typeImplementation, Guid? key = null, bool checkInheritance = true)
        {
            if (checkInheritance)
            {
                bool doesImplementInterface = typeImplementation.GetInterface(typeInterface.Name) == null;

                if (!typeInterface.IsInterface)
                    throw new InvalidOperationException("The first parameter to the method Register is not an interface");

                if (doesImplementInterface)
                    throw new InvalidOperationException("The second parameter does not implement interface:" + typeInterface.Name);
            }

            if (typeImplementation.GetConstructors().Any(x => !x.GetParameters().Any())) // default constructor
            {
                #region Default Constructor
                ContainerItem containerItem = null;
                if (key.HasValue)
                    containerItem = new ContainerItem(key.Value, typeInterface, GetOrCreateLazyObjectOfType(typeImplementation, key));
                else
                    containerItem = new ContainerItem(typeInterface, GetOrCreateLazyObjectOfType(typeImplementation, key));
                _objects.Add(containerItem);
                #endregion
            }
            else // default constructor not found
            {
                #region Overloaded Constructor
                var constructor = typeImplementation.GetConstructors().FirstOrDefault();

                if (constructor != null)
                {
                    var parameters = constructor.GetParameters();
                    object[] parameterObjects = new object[parameters.Length];

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        Type currentParameterType = parameters[i].ParameterType;
                        parameterObjects[i] = GetOrCreateObjectOfType(currentParameterType, null);
                    }

                    ContainerItem containerItem = null;

                    if (key.HasValue)
                        containerItem = new ContainerItem(key.Value, typeInterface, typeImplementation, parameterObjects);
                    else
                        containerItem = new ContainerItem(typeInterface, typeImplementation, parameterObjects);

                    _objects.Add(containerItem);
                }
                #endregion
            }
        }
        #endregion
    }
}
