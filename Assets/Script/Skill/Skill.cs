using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected Weapon weapon;

    void Awake()
    {
        weapon = GetComponent<Weapon>();
    }
}