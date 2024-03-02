using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(WeaponController))]
[RequireComponent(typeof(PlayerGrafic))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Player: MonoBehaviour
{
    public static new GameObject gameObject;
    public static new Transform transform;
    public static PlayerController pCon;
    public static WeaponController wCon;
    public static PlayerGrafic grafic;

    public static Camera cam;


    void Awake()
    {
        transform = GetComponent<Transform>();
        pCon = GetComponent<PlayerController>();
        wCon = GetComponent<WeaponController>();
        grafic = GetComponent<PlayerGrafic>();

        cam = Camera.main;
    }
}
