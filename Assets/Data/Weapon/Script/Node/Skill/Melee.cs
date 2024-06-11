using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Skill
{
    [UnitTitle("Melee")]
    [UnitCategory("Weapon/Skill")]
    public class Melee : CoroutineSkillNode
    {
        public enum SwingCurve { Linear, Quadratic }

        [DoNotSerialize] public ValueInputPort<GameObject> objectPort;
        [DoNotSerialize] public ValueInputPort<float> durationPort;
        [DoNotSerialize] public ValueInputPort<float> spreadPort;
        [DoNotSerialize] public ValueInputPort<float> startSpearPort;
        [DoNotSerialize] public ValueInputPort<float> spearPort;
        [DoNotSerialize] public ValueInputPort<SwingCurve> curvePort;

        [DoNotSerialize] public ValueOutputPort<float> tPort;

        protected override void Definition()
        {
            base.Definition();

            objectPort = ValueInputPort<GameObject>.Define(this, "Object");
            durationPort = ValueInputPort<float>.Define(this, "Duration");
            spreadPort = ValueInputPort<float>.Define(this, "Spread");
            startSpearPort = ValueInputPort<float>.Define(this, "Start Spear");
            spearPort = ValueInputPort<float>.Define(this, "Spear");
            curvePort = ValueInputPort<SwingCurve>.Define(this, "Curve");

            tPort = ValueOutputPort<float>.Define(this, "T");
        }
        protected override IEnumerator UseCoroutine(Flow flow)
        {
            GameObject obj = objectPort.GetValue(flow);
            float duration = durationPort.GetValue(flow);
            float spread = spreadPort.GetValue(flow);
            float startSpear = startSpearPort.GetValue(flow);
            float spear = spearPort.GetValue(flow);
            SwingCurve curve = curvePort.GetValue(flow);

            float startRotateZ = Math.Vector2ToDegree(weapon.con.targetDir);

            float time = 0;
            float t = 0;
            
            while (true)
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

                float rotateZ = startRotateZ + spread * 0.5f - t * spread - 90;
                obj.transform.rotation = Quaternion.Euler(0, 0, rotateZ);
                obj.transform.position = (Vector2)weapon.con.transform.position + weapon.con.ControllerData.center//-|
                  + Math.Vector2TransformToEllipse(
                      Math.RadianToVector2((rotateZ + 90) * Mathf.Deg2Rad), 0.75f, 0.5f
                      ) * (startSpear + spear * t);
                tPort.SetValue(flow, t);
                Update();

                yield return null;
                if (time * duration + Time.deltaTime >= duration)
                {
                    End();
                    yield break;
                }
                else time += Time.deltaTime / duration;
            }
        }
    }
}