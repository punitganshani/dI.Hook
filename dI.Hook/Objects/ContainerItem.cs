using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dIHook.Objects
{
    public sealed class ContainerItem
    {
        public Type InterfaceType { get; private set; }

        public Guid Key { get; private set; }

        public Lazy<Object> Object { get; private set; }

        public ContainerItem(Type interfaceType, Type objectType, params object[] parameters)
        {
            if (interfaceType == null)
                throw new ArgumentNullException("interfaceType");

            InterfaceType = interfaceType;
            Object = new Lazy<object>(() => { return Activator.CreateInstance(objectType, parameters); });
            Key = Guid.NewGuid();
        }

        public ContainerItem(Type interfaceType, Func<Object> func)
        {
            if (interfaceType == null)
                throw new ArgumentNullException("interfaceType");

            if (func == null)
                throw new ArgumentNullException("func");

            InterfaceType = interfaceType;
            Object = new Lazy<object>(func);
            Key = Guid.NewGuid();
        }

        public ContainerItem(Type interfaceType, Lazy<Object> lazyObject)
        {
            if (interfaceType == null)
                throw new ArgumentNullException("interfaceType");

            if (lazyObject == null)
                throw new ArgumentNullException("lazyObject");

            InterfaceType = interfaceType;
            Object = lazyObject;
            Key = Guid.NewGuid();
        }

        public ContainerItem(Guid key, Type interfaceType, Func<Object> func)
        {
            if (key == Guid.Empty)
                throw new ArgumentOutOfRangeException("key");

            if (interfaceType == null)
                throw new ArgumentNullException("interfaceType");

            if (func == null)
                throw new ArgumentNullException("func");

            InterfaceType = interfaceType;
            Object = new Lazy<object>(func);
            Key = key;
        }

        public ContainerItem(Guid guid, Type typeInterface, Lazy<object> lazy)
        {
            if (guid == Guid.Empty)
                throw new ArgumentOutOfRangeException("guid");

            if (typeInterface == null)
                throw new ArgumentNullException("typeInterface");

            if (lazy == null)
                throw new ArgumentNullException("lazy");

            this.Key = guid;
            this.InterfaceType = typeInterface;
            this.Object = lazy;
        }

        public ContainerItem(Guid guid, Type interfaceType, Type objectType, params object[] parameters)
        {
            if (guid == Guid.Empty)
                throw new ArgumentOutOfRangeException("guid");

            if (interfaceType == null)
                throw new ArgumentNullException("interfaceType");

            this.Key = guid;
            this.InterfaceType = interfaceType;
            this.Object = new Lazy<object>(() => { return Activator.CreateInstance(objectType, parameters); });

        }

    }
}
