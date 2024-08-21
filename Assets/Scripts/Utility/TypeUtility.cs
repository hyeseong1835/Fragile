using System;
using System.Linq;
using System.Reflection;

public static class TypeUtility
{
    public static Type[] LoadChildType<T>() => LoadChildType(typeof(T));

    public static Type[] LoadChildType(Type type)
    {
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => 
                t.IsClass 
                && t.IsAbstract == false
                && type.IsAssignableFrom(t)
        ).ToArray();
    }
    public static ClassT[] CreateChildInstance<ClassT>() where ClassT : class
    {
        Type[] types = LoadChildType(typeof(ClassT));
        ClassT[] result = new ClassT[types.Length];

        for (int i = 0; i < types.Length; i++)
        {
            Type type = types[i];
            ClassT? instance = (ClassT?)Activator.CreateInstance(type);

            if (instance == null)
            {
                Console.WriteLine("Failed to create instance of " + type.Name);
                continue;
            }
            result[i] = instance;
        }

        return result;
    }
}