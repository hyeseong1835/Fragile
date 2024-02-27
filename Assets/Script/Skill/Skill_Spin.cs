using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        Skill.weapon.isUsing = false;
        spinObj.gameObject.SetActive(true);
        transform.SetParent(null);
        float startAngleZ = Mathf.Atan2(Player.cam.ScreenToWorldPoint(Player.pCon.mousePos).y - pTransform.position.y,
            Player.cam.ScreenToWorldPoint(Player.pCon.mousePos).x - pTransform.position.x) * Mathf.Rad2Deg - 90;
        Vector3 startVector = Player.cam.ScreenToWorldPoint(Player.pCon.mousePos) + new Vector3(0, 0, 10) - pTransform.position;
        float time = 0;
        while (time <= spinDuration)
        {
            spinObj.transform.position += startVector.normalized * throwSpeed;
            spinObj.transform.rotation = Quaternion.Euler(0, 0, startAngleZ - time * spinSpeed);
            time += Time.deltaTime;
            yield return null;
        }
        Destroy(spinObj);
        Skill.weapon.DestroyWeapon();
    }
    public void SpinObjectTriggerEnter(GameObject obj, Collider2D coll)
    {
        spinHitEvent.Invoke(obj, coll);
    }
}
