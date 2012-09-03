using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dIHook.Objects;

namespace dIHook.Containers
{
    public class Container : IContainer
    {
        private Dictionary<Type, Lazy<Object>> _objects;

        public List<Type> RegisteredTypes
        {
            get
            {
                return _objects.Keys.ToList();
            }
        }

        public Container()
        {
            _objects = new Dictionary<Type, Lazy<object>>();
        }

        public void Register<TClass>(bool throwErrorIfExists = false) where TClass : class
        {
            Type typeClass = typeof(TClass);

            if (_objects.ContainsKey(typeClass) && throwErrorIfExists)
                throw new InvalidOperationException("A similar type has already been registered in the container.");
            else
            {
                Register(typeClass, typeClass, false);
            }
        }

        public void Register<TInterface, TImplementation>()
                            where TImplementation : class
        {
            Type typeInterface = typeof(TInterface);
            Type typeImplementation = typeof(TImplementation);

            Register(typeInterface, typeImplementation);
        }

        private Lazy<Object> GetLazyObjectOfType(Type type, params object[] parameters)
        {
            if (_objects.ContainsKey(type))
            {
                return _objects[type];
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


        private Object GetObjectOfType(Type type, params object[] parameters)
        {
            if (_objects.ContainsKey(type))
            {
                return _objects[type].Value;
            }
            else
            {
                if (parameters != null && parameters.Length > 0)
                    return Activator.CreateInstance(type, parameters);
                else
                    return Activator.CreateInstance(type);
            }
        }

        private void Register(Type typeInterface, Type typeImplementation, bool checkInheritance = true)
        {
            if (checkInheritance)
            {
                if (!typeInterface.IsInterface)
                    throw new InvalidOperationException("The first parameter to the method Register is not an interface");

                bool doesImplementInterface = typeImplementation.GetInterface(typeInterface.Name) == null;
                if (doesImplementInterface)
                    throw new InvalidOperationException("The second parameter does not implement interface:" + typeInterface.Name);
            }
            if (_objects.ContainsKey(typeInterface))
                throw new InvalidOperationException("A similar type has already been registered in the container.");
            else
            {
                if (typeImplementation.GetConstructors().Any(x => !x.GetParameters().Any())) // default constructor
                {
                    _objects.Add(typeInterface, GetLazyObjectOfType(typeImplementation));
                }
                else // default constructor not found
                {
                    var constructor = typeImplementation.GetConstructors().FirstOrDefault();
                    if (constructor != null)
                    {
                        var parameters = constructor.GetParameters();
                        object[] parameterObjects = new object[parameters.Length];

                        for (int i = 0; i < parameters.Length; i++)
                        {
                            Type currentParameterType = parameters[i].ParameterType;
                            parameterObjects[i] = GetObjectOfType(currentParameterType);
                        }

                        _objects.Add(typeInterface, new Lazy<object>(() =>
                        {
                            return Activator.CreateInstance(typeImplementation, parameterObjects);
                        }));
                    }
                }
            }
        }

        public TInterface CreateInstance<TInterface>() where TInterface : class
        {
            Type typeInterface = typeof(TInterface);
            if (_objects.ContainsKey(typeInterface))
                return (TInterface)_objects[typeInterface].Value;
            else
            {
                Register<TInterface>();
                return (TInterface)_objects[typeInterface].Value;
            }
        }

        public void Register<TInterface>(TInterface objectValue)
        {
            Type typeInterface = typeof(TInterface);
            if (_objects.ContainsKey(typeInterface))
                throw new InvalidOperationException("A similar type has already been registered in the container.");
            else
            {
                _objects.Add(typeInterface, new Lazy<object>(()=> { return objectValue; }));
            }
        }
    }

    //class A
    //{
    //    public A()
    //    {
    //        Container c = new Container();
    //        c.Register<ICustomer, Customer>();
    //        c.Register<IBill, Bill>();


    //        OrderProcess o = c.CreateInstance<OrderProcess>();
    //        o.Process();
    //    }

    //    public class OP
    //    {
    //        public OP(ICustomer c, IBill b)
    //        {

    //        }
    //    }

    //}
}
