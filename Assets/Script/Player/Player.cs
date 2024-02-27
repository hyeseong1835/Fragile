using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(WeaponController))]
[RequireComponent(typeof(PlayerGrafic))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public static class Player
{
    public static PlayerController pCon;
    public static WeaponController wCon;
    public static PlayerGrafic grafic;

    public static UI_Inventory inventoryUI;
    public static Camera cam;
}
