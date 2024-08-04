using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Skill
{
    [UnitTitle("Throw")]
    [UnitCategory("Weapon/Skill")]
    public class Throw : CoroutineSkillNode
    {
        [DoNotSerialize] public ValueInputPort<GameObject> objectPortIn;
        [DoNotSerialize] public ValueInputPort<float> spinSpeedIn;
        [DoNotSerialize] public ValueInputPort<float> throwSpeedIn;
        [DoNotSerialize] public ValueInputPort<float> durationIn;

        [DoNotSerialize] public ValueInputPort<Pool> poolIn;

        protected override void Definition()
        {
            base.Definition();

            objectPortIn = ValueInputPort<GameObject>.Define(this, "Object");
            spinSpeedIn = ValueInputPort<float>.Define(this, "Spin Speed");
            throwSpeedIn = ValueInputPort<float>.Define(this, "Throw Speed");
            durationIn = ValueInputPort<float>.Define(this, "Duration");
            poolIn = ValueInputPort<Pool>.Define(this, "Pool");
        }
        protected override IEnumerator UseCoroutine(Flow flow)
        {
            #region Start  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|  

            GameObject triggerObj = objectPortIn.GetValue(flow);
            float spinSpeed = spinSpeedIn.GetValue(flow);
            float throwSpeed = throwSpeedIn.GetValue(flow);
            float duration = durationIn.GetValue(flow);
            Pool pool = poolIn.GetValue(flow);

            Vector3 startVector = component.con.targetDir;
            float startAngleZ = Mathf.Atan2(startVector.y, startVector.x) * Mathf.Rad2Deg - 90;//-|

            #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -| 

            float time = 0;
            while (time <= duration)
            {
                #region Update  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|  

                triggerObj.transform.position += startVector.normalized * throwSpeed * Time.deltaTime;//-|

                float rotateZ = startAngleZ - time * spinSpeed;
                triggerObj.transform.rotation = Quaternion.Euler(0, 0, rotateZ);

                #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|  
                Update();

                time += Time.deltaTime;
                yield return null;
            }
            End();
        }
    }
}
