using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dIHook.Objects
{
    public interface IContainer
    {
        System.Collections.ObjectModel.ReadOnlyCollection<dIHook.Objects.ContainerItem> Collection { get; }
        System.Collections.ObjectModel.ReadOnlyCollection<Guid> Keys { get; }
        void Register<TClass>() where TClass : class;
        void Register<TClass>(Guid key) where TClass : class;
        void Register<TInterface, TImplementation>() where TImplementation : class, TInterface;
        void Register<TInterface, TImplementation>(Guid guid) where TImplementation : class, TInterface;
        void Register<TInterface>(Guid key, TInterface objectValue);
        void Register<TInterface>(TInterface objectValue);
        TInterface Resolve<TInterface>() where TInterface : class;
        TInterface Resolve<TInterface>(Guid key) where TInterface : class;
        System.Collections.ObjectModel.ReadOnlyCollection<Type> Types { get; }
    }
}
