using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace dIHook.Utilities
{
    public static class DirectoryExtensions
    {
        public static  List<Assembly> GetAssemblies(string path, string filter)
        {
            List<Assembly> assemblies = new List<Assembly>();
            string[] files = Directory.GetFiles(Environment.CurrentDirectory, filter);

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    assemblies.Add(Assembly.LoadFile(files[i]));
                }
                catch (BadImageFormatException) // not a valid assembly
                { }
                catch (Exception)
                { }
            }

            return assemblies;
        }
    }
}
