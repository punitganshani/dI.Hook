dI.Hook - Light-weight IoC and DI engine using AOP
=======

dI.Hook (pronounced as, du' hʊk) is a **light-weight IoC and DI engine** to define hooks via code or configuration, call them dynamically or conditionally and dispose them when not required.  However, **this is not yet another dependency injection framework**.  It does a lot more than just DI -

* dI.Hook creates a **controlled** repository of hooks instead of searching & instantiating a hook at a time when required 
* dI.Hook allows **lazy-loading** the hook objects at run-time, thereby optimizing the memory allocation and performance 
* dI.Hook allows addition of hooks by both **hook-type** and **live hook instance** thereby 
  ** allowing sharing of hook instances between multiple repositories and/or 
  ** adding multiple hooks of same type with different attributes in a single repository

* dI.Hook allows creating a hierarchy of hooks 
* dI.Hook promotes both repository based hooks and method based hooks -   ** Hooks can now be stored in a repository or 
   ** The repository can be designed light-weight and methods could be decorated by Hook Attributes 

* Hooks can be **invoked conditionally** using C# Funcs, delegates and Actions 
* Hooks can be invoked **sequentially** (as blocking calls) or **parallel** (in separate threads) 

 

It is called dI.Hook as it uses dependency injection to allow you to create run-time dependencies or hooks.

Simple to learn, well documented, and extremely light-weight.

## Using in your application

You can reference dI.Hook using Nuget
[Download instructions from Nuget.org](https://nuget.org/packages/dI.Hook)
