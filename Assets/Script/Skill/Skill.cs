using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public static class Skill
{
    public enum Curve
    {
        Linear, Quadratic
    }
    /// <summary>
    /// ����� ȸ������ŭ ȸ����Ű�� ��ų
    /// </summary>
    /// <param name="origin">���� ��ġ</param>
    /// <param name="triggerObj">���</param>
    /// <param name="spread">ȸ����</param>
    /// <param name="duration">������ �ɸ��� �ð�</param>
    /// <param name="swingCurve">���ӵ�</param>
    /// <param name="enterEvent">����� ��</param>
    /// <param name="stayEvent">��� ���� ��</param>
    /// <param name="exitEvent">������ ��</param>
    /// <param name="endEvent">������ ������ ��</param>
    /// <returns></returns>
    public static IEnumerator Swing(Controller con, TriggerObject triggerObj, float spread, float duration, Curve swingCurve,
        UnityEvent<GameObject, Collider2D> enterEvent = null,
        UnityEvent<GameObject, Collider2D> stayEvent = null,
        UnityEvent<GameObject, Collider2D> exitEvent = null,
        UnityEvent<GameObject> endEvent = null)
    {
        //�ʱ�ȭ
        triggerObj.SetEvent(enterEvent, stayEvent, exitEvent);

        //��ų
        triggerObj.transform.position = con.transform.position + (Vector3)con.center;

        float startRotateZ = Utility.Vector2ToDegree(con.targetDir);

        float time = 0;
        float t = 0;
        float rotateZ = startRotateZ;
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
            rotateZ = startRotateZ + spread * 0.5f - t * spread - 90;
            triggerObj.transform.rotation = Quaternion.Euler(0, 0, rotateZ);
            triggerObj.transform.position = (Vector2)con.transform.position + con.center
                + Utility.Vector2TransformToEllipse(
                    Utility.RadianToVector2((rotateZ + 90) * Mathf.Deg2Rad)
                    , 0.75f, 0.5f);

            time += Time.deltaTime / duration;
            yield return null;
        }
        triggerObj.transform.rotation = Quaternion.Euler(0, 0, startRotateZ - spread * 0.5f - 90);
        triggerObj.transform.position = (Vector2)con.transform.position + con.center
                + Utility.Vector2TransformToEllipse(
                    Utility.RadianToVector2((rotateZ + 90) * Mathf.Deg2Rad)
                    , 0.75f, 0.5f);

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
        //����
        triggerObj.gameObject.SetActive(true);
        triggerObj.SetEvent(enterEvent, stayEvent, exitEvent);
        triggerObj.transform.SetParent(null);

        Vector3 startVector = con.targetDir;
        float startAngleZ = Mathf.Atan2(startVector.y, startVector.x) * Mathf.Rad2Deg - 90;
        float time = 0;

        //���� ��
        while (time <= duration)
        {
            triggerObj.transform.position += startVector.normalized * throwSpeed;
            triggerObj.transform.rotation = Quaternion.Euler(0, 0, startAngleZ - time * spinSpeed);
            time += Time.deltaTime;
            yield return null;
        }

        //����
        if (endEvent != null) endEvent.Invoke(triggerObj.gameObject);
    }
}
