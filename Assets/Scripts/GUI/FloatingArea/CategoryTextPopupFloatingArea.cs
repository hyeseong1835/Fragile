using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CategoryTextPopupFloatingArea : FloatingArea
{
    public string[] category;
    public string[] element;

    Action<CategoryTextPopupFloatingArea, int> selectCategoryEvent;
    Func<int, bool> selectElementEvent;
    
    int mouseOnIndex = -1;
    float height;
    float topSpace, bottomSpace, rightSpace, leftSpace;

    public static GUIStyle categoryStyle = new GUIStyle(EditorStyles.miniButtonMid);
    public static GUIStyle elementStyle = new GUIStyle(EditorStyles.boldLabel);

    public CategoryTextPopupFloatingArea(
        string[] category,
        string[] element,
        Action<CategoryTextPopupFloatingArea, int> selectCategoryEvent,
        Func<int, bool> selectElementEvent,
        float height = 20,
        float topSpace = 5,
        float bottomSpace = 10,
        float rightSpace = 1,
        float leftSpace = 1
    )
    {
        this.category = category;
        this.element = element;

        this.selectCategoryEvent = selectCategoryEvent;
        this.selectElementEvent = selectElementEvent;

        this.height = height;

        this.topSpace = topSpace;
        this.bottomSpace = bottomSpace;
        this.rightSpace = rightSpace;
        this.leftSpace = leftSpace;
    }
    public override void OnCreated()
    {

    }
    public override void EventListen()
    {
        mouseOnIndex = Mathf.FloorToInt((Event.current.mousePosition.y - (manager.rect.y + topSpace)) / height);
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Event.current.Use();
            if (0 <= mouseOnIndex && mouseOnIndex < category.Length)
            {
                selectCategoryEvent.Invoke(this, mouseOnIndex);
                return;
            }
            if (category.Length <= mouseOnIndex && mouseOnIndex < element.Length)
            {
                if(selectElementEvent.Invoke(mouseOnIndex))
                {
                    manager.Destroy();
                }
                return;
            }
        }
    }
    public override void Draw()
    {
        for (int i = 0; i < category.Length; i++)
        {
            GUI.Label(
                new Rect(
                    manager.rect.x + leftSpace,
                    manager.rect.y + height * i + topSpace,
                    manager.rect.width - (rightSpace + leftSpace),
                    height
                ),
                category[i],
                categoryStyle
            );
        }
        for (int i = 0; i < element.Length; i++)
        {
            GUI.Label(
                new Rect(
                    manager.rect.x + leftSpace,
                    manager.rect.y + height * (i + category.Length) + topSpace,
                    manager.rect.width - (rightSpace + leftSpace),
                    height
                ),
                element[i],
                elementStyle
            );
        }
        //GUI.Label(new Rect(Event.current.mousePosition, new Vector2(50, 20)), $"{mouseOnIndex}");
    }

    public override float GetHeight()
    {
        return height * (category.Length + element.Length) + topSpace + bottomSpace;
    }
    public override void CreateField()
    {
        base.CreateField();
    }
}
