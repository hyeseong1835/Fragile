using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;



#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WeaponSystem.Component
{
#if UNITY_EDITOR
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ComponentInfoAttribute : Attribute
    {
        public string path;
        public string description;

        public ComponentInfoAttribute(string path, string description)
        {
            this.path = path;
            this.description = description;
        }
    }
    public class WeaponComponentInfoTree
    {
        public string path;
        public string name;
        public List<WeaponComponentInfo> value = new List<WeaponComponentInfo>();
        public List<WeaponComponentInfoTree> children = new List<WeaponComponentInfoTree>();

        public WeaponComponentInfoTree(string path, string name)
        {
            this.path = path;
            this.name = name;
        }
        public bool Contain(string name)
        {
            foreach (WeaponComponentInfoTree tree in children)
            {
                if (tree.name == name) return true;
            }
            return false;
        }
        public WeaponComponentInfoTree GetChild(string name)
        {
            foreach (WeaponComponentInfoTree tree in children)
            {
                if (tree.name == name) return tree;
            }
            Debug.LogError($"Not Found: {name}");
            return null;
        }
        public WeaponComponentInfo GetValue(string name)
        {
            foreach (WeaponComponentInfo v in value)
            {
                if (v.name == name) return v;
            }
            Debug.LogError($"Not Found: {name}");
            return default;
        }
        public WeaponComponentInfoTree GetOrAdd(string path)
        {
            string name = path.Split('/').Last();
            foreach (WeaponComponentInfoTree tree in children)
            {
                if (tree.name == name) return tree;
            }
            WeaponComponentInfoTree result = new WeaponComponentInfoTree(path, name);
            children.Add(result);
            return result;
        }
    }
    public struct WeaponComponentInfo
    {
        public Type type;
        public string path;
        public string name;
        public string description;

        public WeaponComponentInfo(Type type, string path, string name, string description)
        {
            this.type = type;
            this.path = path;
            this.name = name;
            this.description = description;
        }
        public override string ToString()
        {
            return $"[{type}] \"{name}\" : ({description}) \n\"{path}\"";
        }
        public WeaponComponent CreateComponent()
        {
            WeaponComponent component = (WeaponComponent)Activator.CreateInstance(type);
            {
                component.info = this;
            }
            return component;
        }
    }
#endif
    public abstract class WeaponComponent : IDisposable
    {
        public abstract void Dispose();

#if UNITY_EDITOR
        public static FloatingAreaManager floatingManager = new FloatingAreaManager();
        protected static WeaponComponent usingFloatingManagerComponent = null;
        public static WeaponComponentInfoTree componentInfoTree;

        [InitializeOnLoadMethod]
        static void LoadComponentInfo()
        {
            componentInfoTree = new WeaponComponentInfoTree("Component", "Component");
            Type componentType = typeof(WeaponComponent);
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsClass && type.IsAbstract == false && componentType.IsAssignableFrom(type))
                {
                    IEnumerable<ComponentInfoAttribute> attributes = type.GetCustomAttributes<ComponentInfoAttribute>(true).Reverse();
                    List<string> path = new List<string>();
                    foreach (ComponentInfoAttribute attribute in attributes)
                    {
                        path.AddRange(attribute.path.Split('/'));
                        AddComponentInfo(componentInfoTree, 0);

                        void AddComponentInfo(WeaponComponentInfoTree tree, int index)
                        {
                            if (index == path.Count - 1)
                            {
                                tree.value.Add(
                                    new WeaponComponentInfo(
                                        type,
                                        string.Join("/", path),
                                        path[^1],
                                        attribute.description
                                    )
                                );
                                return;
                            }
                            WeaponComponentInfoTree child = tree.GetOrAdd(string.Join("/", path.Take(++index)));
                            AddComponentInfo(child, index);
                        }
                    }
                }
            }
        }

        [MenuItem("Tools/컴포넌트 구조 확인")]
        static void DebugComponentInfo()
        {
            Log(componentInfoTree);

            void Log(WeaponComponentInfoTree tree)
            {
                Debug.Log($"\"{tree.name}\"====================\n {tree.path}");

                foreach (WeaponComponentInfo info in tree.value)
                {
                    Debug.Log(info);
                }
                foreach (WeaponComponentInfoTree t in tree.children)
                {
                    Log(t);
                }
            }
        }
        public static WeaponComponentInfo GetComponentInfo(Type type)
        {
            IEnumerable<ComponentInfoAttribute> attributes = type.GetCustomAttributes<ComponentInfoAttribute>(true);

            List<string> path = new List<string>();
            foreach (ComponentInfoAttribute attribute in attributes)
            {
                foreach (string s in attribute.path.Split('/'))
                {
                    path.Insert(0, s);
                }
            }
            return GetComponentInfo(path);
        }
        public static WeaponComponentInfo GetComponentInfo(string path)
        {
            string[] split = path.Split('/');

            WeaponComponentInfoTree open = componentInfoTree;
            for (int i = 0; i < split.Length - 1; i++)
            {
                open = open.GetChild(split[i]);
            }
            return open.GetValue(split[^1]);
        }
        public static WeaponComponentInfo GetComponentInfo(string[] split)
        {
            WeaponComponentInfoTree open = componentInfoTree;
            for (int i = 0; i < split.Length - 1; i++)
            {
                open = open.GetChild(split[i]);
            }
            return open.GetValue(split[^1]);
        }
        public static WeaponComponentInfo GetComponentInfo(List<string> split)
        {
            WeaponComponentInfoTree open = componentInfoTree;
            for (int i = 0; i < split.Count - 1; i++)
            {
                open = open.GetChild(split[i]);
            }
            return open.GetValue(split[^1]);
        }
        public static WeaponComponentInfoTree GetComponentInfoTree(string path)
        {
            string[] split = path.Split('/');

            WeaponComponentInfoTree open = componentInfoTree;
            for (int i = 0; i < split.Length; i++)
            {
                open = open.GetChild(split[i]);
            }
            return open;
        }

        public WeaponComponentInfo info = new WeaponComponentInfo(null, "Null", "Null", "Null");
        protected string changeOrigin = "Null";

        protected abstract Rect HeaderRect { get; }

        public void CreateComponentSelectMenu(string treePath, Action<WeaponComponentInfo> selectEvent)
        {
            WeaponComponentInfoTree tree = GetComponentInfoTree(treePath);

            floatingManager.Create(
                new CategoryTextPopupFloatingArea(
                    tree.children.Select(x => x.name).ToArray(),
                    tree.value.Select(x => x.name).ToArray(),
                    (floating, i) =>
                    {
                        tree = tree.children[i];
                        treePath += $"/{tree.name}";
                        floating.category = tree.children.Select(x => x.name).ToArray();
                        floating.element = tree.value.Select(x => x.name).ToArray();
                    },
                    i =>
                    {
                        selectEvent(tree.value[i]);
                        Debug.Log(tree.value[i]);
                        return true;
                    }
                )
            );
        }
        public void WeaponComponentOnGUI<TComponent>(ref TComponent origin, string label)
            where TComponent : WeaponComponent
        {
            if (info.type == null)
            {
                info = GetComponentInfo(GetType());
            }

            floatingManager.EventListen();
            
            OnGUI(label);
            
            if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && HeaderRect.Contains(Event.current.mousePosition))
            {
                GenericMenu menu = new GenericMenu();
                SetMenu<TComponent>(menu);
                menu.ShowAsContext();
            }

            if (usingFloatingManagerComponent == this)
            {
                if (HeaderRect.width > 10) floatingManager.SetRect(HeaderRect);
            }
            
            floatingManager.Draw();

            if (changeOrigin != "Null")
            {
                Dispose();

                Debug.Log(changeOrigin);
                origin = (TComponent)GetComponentInfo(changeOrigin).CreateComponent();
            }
        }
        protected abstract void OnGUI(string label);
        protected virtual void SetMenu<TComponent>(GenericMenu menu)
            where TComponent : WeaponComponent
        {
            menu.AddItem(
                new GUIContent("변경"),
                false,
                () => CreateComponentSelectMenu(
                    typeof(TComponent).GetCustomAttributes<ComponentInfoAttribute>().First().path,
                    info => changeOrigin = info.path
                )
            );
            usingFloatingManagerComponent = this;
        }
#endif
    }
}