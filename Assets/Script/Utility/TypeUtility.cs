using System;
using System.Reflection;

public static class TypeUtility
{
    public static Type LoadType(string name)
    {
        Type result = Type.GetType(name);
        if (result == null)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
            foreach (var assemblyName in referencedAssemblies)
            {
                var assembly = Assembly.Load(assemblyName);
                if (assembly != null)
                {
                    result = assembly.GetType(name);
                    if (result != null) break;
                }
            }
        }
        return result;
    }

}