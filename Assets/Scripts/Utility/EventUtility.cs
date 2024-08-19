using UnityEngine;

public static class EventUtility
{
    static Event e => Event.current;
    public static bool MouseDown(int mouse) => e.button == mouse && e.type == EventType.MouseDown;
    public static bool MouseDrag(int mouse) => e.button == mouse && e.type == EventType.MouseDrag;
    public static bool MouseUp(int mouse) => e.button == mouse && e.type == EventType.MouseUp;
    public static bool KeyDown(KeyCode keyCode) => e.keyCode == keyCode && e.type == EventType.KeyDown;
}
