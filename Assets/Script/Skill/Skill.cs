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
    /// ����� ȸ������ŭ ȸ����Ű�� ��ų
    /// </summary>
    /// <param name="origin">���� ��ġ</param>
    /// <param name="trigger">���</param>
    /// <param name="spread">ȸ����</param>
    /// <param name="duration">������ �ɸ��� �ð�</param>
    /// <param name="swingCurve">���ӵ�</param>
    /// <param name="enterEvent">����� ��</param>
    /// <param name="stayEvent">��� ���� ��</param>
    /// <param name="exitEvent">������ ��</param>
    /// <param name="endEvent">������ ������ ��</param>
    /// <returns></returns>
    public static IEnumerator Swing(UnityEngine.Transform origin, TriggerObject trigger,float startRotateZ, float spread, float duration, Curve swingCurve,
        UnityEvent<GameObject, Collider2D> enterEvent = null,
        UnityEvent<GameObject, Collider2D> stayEvent = null,
        UnityEvent<GameObject, Collider2D> exitEvent = null,
        UnityEvent<GameObject> endEvent = null)
    {
        //�ʱ�ȭ
        trigger.SetEvent(enterEvent, stayEvent, exitEvent);

        //��ų
        Player.grafic.stateHandAnimation = false;
        trigger.transform.position = origin.position + (Vector3)Player.pCon.playerToMouse.normalized * 0.5f;

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
            trigger.transform.rotation = Quaternion.Euler(0, 0, startRotateZ + spread * 0.5f - t * spread - 90);
            trigger.transform.position = origin.position + new Vector3(
                Mathf.Cos((startRotateZ + spread * 0.5f - t * spread) * Mathf.Deg2Rad),
                Mathf.Sin((startRotateZ + spread * 0.5f - t * spread) * Mathf.Deg2Rad) * 0.5f, 0).normalized * 0.5f;

            time += Time.deltaTime / duration;
            yield return null;
        }
        trigger.transform.rotation = Quaternion.Euler(0, 0, startRotateZ - spread * 0.5f);

        yield return null;

        trigger.gameObject.SetActive(false);
        Player.grafic.stateHandAnimation = true;
        if (endEvent != null) endEvent.Invoke(trigger.gameObject);
    }
    public static IEnumerator Throw(Weapon weapon, TriggerObject trigger, float spinSpeed, float throwSpeed, float duration,
    UnityEvent<GameObject, Collider2D> enterEvent = null,
    UnityEvent<GameObject, Collider2D> stayEvent = null,
    UnityEvent<GameObject, Collider2D> exitEvent = null,
    UnityEvent<GameObject> endEvent = null)
    {
        //����
        weapon.Remove();
        trigger.gameObject.SetActive(true);
        trigger.SetEvent(enterEvent, stayEvent, exitEvent);
        weapon.transform.SetParent(null);
        float startAngleZ = Mathf.Atan2(Player.camCon.cam.ScreenToWorldPoint(Player.pCon.mousePos).y - Player.transform.position.y,
            Player.camCon.cam.ScreenToWorldPoint(Player.pCon.mousePos).x - Player.transform.position.x) * Mathf.Rad2Deg - 90;
        Vector3 startVector = Player.camCon.cam.ScreenToWorldPoint(Player.pCon.mousePos) + new Vector3(0, 0, 10) - Player.transform.position;
        float time = 0;

        //���� ��
        while (time <= duration)
        {
            trigger.transform.position += startVector.normalized * throwSpeed;
            trigger.transform.rotation = Quaternion.Euler(0, 0, startAngleZ - time * spinSpeed);
            time += Time.deltaTime;
            yield return null;
        }

        //����

        if (endEvent != null) endEvent.Invoke(trigger.gameObject);
    }
}
