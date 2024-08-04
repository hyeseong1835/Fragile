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

        [DoNotSerialize] public ValueInputPort<MeleeData> dataPort;
        [DoNotSerialize] public ValueInputPort<GameObject> objPort;

        [DoNotSerialize] public ValueOutputPort<float> tPort;

        protected override void Definition()
        {
            base.Definition();

            dataPort = ValueInputPort <MeleeData>.Define(this, "Data");
            objPort = ValueInputPort<GameObject>.Define(this, "Object");

            tPort = ValueOutputPort<float>.Define(this, "T");
        }
        protected override IEnumerator UseCoroutine(Flow flow)
        {
            MeleeData data = dataPort.GetValue(flow);
            GameObject obj = objPort.GetValue(flow);

            float startRotateZ = Math.Vector2ToDegree(component.con.targetDir);

            float time = 0;
            float t = 0;
            
            while (true)
            {
                switch (data.curve)
                {
                    case SwingCurve.Linear:
                        t = time;
                        break;
                    case SwingCurve.Quadratic:
                        t = time * time;
                        break;
                }

                float rotateZ = startRotateZ + data.spread * 0.5f - t * data.spread - 90;
                obj.transform.rotation = Quaternion.Euler(0, 0, rotateZ);
                obj.transform.position = (Vector2)component.con.transform.position + component.con.Center//-|
                  + Math.Vector2TransformToEllipse(
                      Math.RadianToVector2((rotateZ + 90) * Mathf.Deg2Rad), 0.75f, 0.5f
                      ) * (data.startSpear + data.spear * t);
                tPort.SetValue(flow, t);
                Update();

                yield return null;
                if (time * data.duration + Time.deltaTime >= data.duration)
                {
                    End();
                    yield break;
                }
                else time += Time.deltaTime / data.duration;
            }
        }
    }
}