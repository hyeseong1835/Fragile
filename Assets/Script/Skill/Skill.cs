using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.Events;
using Unity.VisualScripting;

public static class Skill
{
    public enum Curve
    {
        Linear, Quadratic
    }
    /// <summary>
    /// 대상을 회전각만큼 회전시키는 스킬
    /// </summary>
    /// <param name="origin">시전 위치</param>
    /// <param name="triggerObj">대상</param>
    /// <param name="spread">회전각</param>
    /// <param name="duration">시전에 걸리는 시간</param>
    /// <param name="swingCurve">가속도</param>
    /// <param name="enterEvent">닿았을 때</param>
    /// <param name="stayEvent">닿고 있을 때</param>
    /// <param name="exitEvent">나갔을 때</param>
    /// <param name="endEvent">시전이 끝났을 때</param>
    /// <returns></returns>
    public static IEnumerator Swing(Controller con, TriggerObject triggerObj, float startRotateZ, float spread, float duration, Curve swingCurve,
        UnityEvent<GameObject, Collider2D> enterEvent = null,
        UnityEvent<GameObject, Collider2D> stayEvent = null,
        UnityEvent<GameObject, Collider2D> exitEvent = null,
        UnityEvent<GameObject> endEvent = null)
    {
        //초기화
        triggerObj.SetEvent(enterEvent, stayEvent, exitEvent);

        //스킬
        triggerObj.transform.position = con.transform.position + (Vector3)con.targetDir * 0.5f;

        float time = 0;
        float t = 0;
        while (t < 1)
        {
            switch (swingCurve)
            {
                case Curve.Linear:
                    t = time;
                    break;
                case Curve.Quadratic:
                    t = time * time;
                    break;
            }
            triggerObj.transform.rotation = Quaternion.Euler(0, 0, startRotateZ + spread * 0.5f - t * spread - 90);
            triggerObj.transform.position = con.transform.position + new Vector3(
                Mathf.Cos((startRotateZ + spread * 0.5f - t * spread) * Mathf.Deg2Rad),
                Mathf.Sin((startRotateZ + spread * 0.5f - t * spread) * Mathf.Deg2Rad) * 0.5f, 0).normalized * 0.5f;

            time += Time.deltaTime / duration;
            yield return null;
        }
        triggerObj.transform.rotation = Quaternion.Euler(0, 0, startRotateZ - spread * 0.5f);

        yield return null;

        triggerObj.gameObject.SetActive(false);
        if (endEvent != null) endEvent.Invoke(triggerObj.gameObject);
    }
    public static IEnumerator Throw(Controller con, TriggerObject triggerObj, 
        float spinSpeed, float throwSpeed, float duration,
        UnityEvent<GameObject, Collider2D> enterEvent = null,
        UnityEvent<GameObject, Collider2D> stayEvent = null,
        UnityEvent<GameObject, Collider2D> exitEvent = null,
        UnityEvent<GameObject> endEvent = null)
    {
        //시작
        triggerObj.gameObject.SetActive(true);
        triggerObj.SetEvent(enterEvent, stayEvent, exitEvent);
        triggerObj.transform.SetParent(null);

        Vector3 startVector = con.targetDir;
        float startAngleZ = Mathf.Atan2(startVector.y, startVector.x) * Mathf.Rad2Deg - 90;
        float time = 0;

        //실행 중
        while (time <= duration)
        {
            triggerObj.transform.position += startVector.normalized * throwSpeed;
            triggerObj.transform.rotation = Quaternion.Euler(0, 0, startAngleZ - time * spinSpeed);
            time += Time.deltaTime;
            yield return null;
        }

        //종료
        if (endEvent != null) endEvent.Invoke(triggerObj.gameObject);
    }
}
