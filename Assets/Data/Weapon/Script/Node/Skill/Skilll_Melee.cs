using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[UnitTitle("Melee")]
[UnitCategory("Weapon/Skill")]
public class Skill_Melee : CoroutineSkill
{
    public enum SwingCurve { Linear, Quadratic }

    [DoNotSerialize] public ValueInput Iv_triggerObj;

    [DoNotSerialize] public ValueInput Iv_duration;
    [DoNotSerialize] public ValueInput Iv_spread;
    [DoNotSerialize] public ValueInput Iv_startSpear;
    [DoNotSerialize] public ValueInput Iv_spear;
    [DoNotSerialize] public ValueInput Iv_curve;

    protected override void Definition()
    {
        base.Definition();

        Iv_triggerObj = ValueInput<TriggerObject>("TriggerObj");
        Iv_duration = ValueInput<float>("Duration", 1);
        Iv_spread = ValueInput<float>("Spread", 0);
        Iv_startSpear = ValueInput<float>("Start Spear", 1);
        Iv_spear = ValueInput<float>("Spear", 0);
        Iv_curve = ValueInput<SwingCurve>("Curve", SwingCurve.Linear);
    }
    protected override IEnumerator Use(Flow flow, GameObject gameObject, Weapon weapon)
    {
        TriggerObject triggerObj = flow.GetValue<TriggerObject>(Iv_triggerObj);
        float duration = flow.GetValue<float>(Iv_duration);
        float spread = flow.GetValue<float>(Iv_spread);
        float startSpear = flow.GetValue<float>(Iv_startSpear);
        float spear = flow.GetValue<float>(Iv_spear);
        SwingCurve curve = flow.GetValue<SwingCurve>(Iv_curve);

        #region Start  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        if (weapon.hand_obj != null) weapon.hand_obj.gameObject.SetActive(false);

        weapon.con.hand.HandLink(triggerObj.transform, HandMode.ToTarget);

        triggerObj.transform.position = weapon.con.transform.position + (Vector3)weapon.con.ControllerData.center;//-|

        float startRotateZ = Math.Vector2ToDegree(weapon.con.targetDir);

        #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        float time = 0;
        float t = 0;
        while (t < 1)
        {
            switch (curve)
            {
                case SwingCurve.Linear:
                    t = time;
                    break;
                case SwingCurve.Quadratic:
                    t = time * time;
                    break;
            }

            #region Update  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

            float rotateZ = startRotateZ + spread * 0.5f - t * spread - 90;
            triggerObj.transform.rotation = Quaternion.Euler(0, 0, rotateZ);
            triggerObj.transform.position = (Vector2)weapon.con.transform.position + weapon.con.ControllerData.center//-|
              + Math.Vector2TransformToEllipse(
                  Math.RadianToVector2((rotateZ + 90) * Mathf.Deg2Rad),
                    0.75f, 0.5f
                ) * (startSpear + spear * t);

            #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -| 

            time += Time.deltaTime / duration;
            yield return null;
        }

        #region End  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

        UseEnd();

        if (weapon.hand_obj != null) weapon.hand_obj.gameObject.SetActive(false);//-|

        triggerObj.gameObject.SetActive(false);

        if (weapon.hand_obj != null)
        {
            weapon.hand_obj.gameObject.SetActive(true);
            weapon.con.hand.HandLink(weapon.hand_obj, HandMode.ToHand);
        }
        else
        {
            weapon.con.hand.HandLink(null);
        }

        #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|
    }
}