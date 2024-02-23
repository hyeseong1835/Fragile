using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Skill_Spin : Skill
{
    public GameObject spinObj;
    public float spinDuration;
    public float throwSpeed;
    public float spinSpeed;
    public float spinDamage;
    [SerializeField] UnityEvent<GameObject, Collider2D> spinHitEvent;

    public IEnumerator Spin()
    {
        if (spinObj.activeInHierarchy == true) yield break;
        weapon.isUsing = false;
        spinObj.gameObject.SetActive(true);
        transform.SetParent(null);
        float startAngleZ = Mathf.Atan2(player.cam.ScreenToWorldPoint(pCon.mousePos).y - player.transform.position.y,
            player.cam.ScreenToWorldPoint(pCon.mousePos).x - player.transform.position.x) * Mathf.Rad2Deg - 90;
        Vector3 startVector = player.cam.ScreenToWorldPoint(pCon.mousePos) + new Vector3(0, 0, 10) - player.transform.position;
        float time = 0;
        while (time <= spinDuration)
        {
            spinObj.transform.position += startVector.normalized * throwSpeed;
            spinObj.transform.rotation = Quaternion.Euler(0, 0, startAngleZ - time * spinSpeed);
            time += Time.deltaTime;
            yield return null;
        }
        Destroy(spinObj);
        weapon.DestroyWeapon();
    }
    public void spawnedObjectTriggerEnter(GameObject obj, Collider2D coll)
    {
        if (coll.gameObject.layer == 20)
        {
            Debug.Log(coll.gameObject.name + " (Enemey)");
            weapon.AddDurability(-1);
            coll.GetComponent<Stat>().OnDamage(spinDamage * weapon.damage);
        }
        else if (coll.gameObject.layer == 21)
        {
            Debug.Log(coll.gameObject.name + " (Obstacle)");
            weapon.AddDurability(-1);
            coll.GetComponent<Stat>().OnDamage(spinDamage * weapon.damage);

        }
        else if (coll.gameObject.layer == 22)
        {
            Debug.Log(coll.gameObject.name + " (UnDestroy)");
        }
    }
}
