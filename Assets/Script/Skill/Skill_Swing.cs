using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Skill_Swing : Skill
{

    public GameObject swingObj;
    public float swingDamage;
    public float duration = 0;
    public int spread = 0;
    enum Curve
    {
        Linear, Quadratic
    }
    [SerializeField] Curve swingCurve;
    [SerializeField] UnityEvent<GameObject, Collider2D> swingHitEvent;

    public IEnumerator Swing()
    {
        //�ʱ�ȭ
        pCon.isAttack = true;
        swingObj.SetActive(true);

        //��ų
        float time = 0;
        switch(swingCurve)
        {
            case Curve.Linear:
                while (time < 1)
                {
                    swingObj.transform.rotation = Quaternion.Euler(0, 0, pCon.viewRotateZ + spread * 0.5f - time * spread);

                    time += Time.deltaTime / duration;
                    yield return null;
                }
                break;
            case Curve.Quadratic:
                while (time < 1)
                {
                    swingObj.transform.rotation = Quaternion.Euler(0, 0, pCon.viewRotateZ + spread * 0.5f - time * time * spread);

                    time += Time.deltaTime / duration;
                    yield return null;
                }
                break;
        }
        swingObj.transform.rotation = Quaternion.Euler(0, 0, pCon.viewRotateZ - spread * 0.5f);
        yield return null;
        //�ʱ�ȭ
        swingObj.SetActive(false);
        pCon.isAttack = false;
    }
    public void SwingObjectTriggerEnter(GameObject obj, Collider2D coll)
    {
        swingHitEvent.Invoke(obj, coll);
    }
}