using Sirenix.OdinInspector;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Skill_Shoot : Skill, Input_Empty
{
    [SerializeField] Input_TriggerHit enter, stay, exit;
    [SerializeField] Input_TriggerEvent start, update, end;

    [SerializeField]
    float spinSpeed;
    [SerializeField]
    float throwSpeed;
    [SerializeField]
    float duration;

    [SerializeField]
    Pool pool;

    protected override void InitSkill()
    {
        if (Editor.GetType(Editor.StateType.IsPlay))
        {
            pool.Init();
        }
    }
    public void Empty()
    {
        StartCoroutine(Shoot());
    }
    IEnumerator Shoot()
    {
        #region Start  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|  
        
            TriggerObject triggerObj = pool.Use().GetComponent<TriggerObject>();

            triggerObj.SetEvent
            (
                enter.TriggerHit,
                stay.TriggerHit,
                exit.TriggerHit
            );
            triggerObj.gameObject.SetActive(true);
            triggerObj.transform.position = con.transform.position;

            Vector3 startVector = con.targetDir;
            float startAngleZ = Mathf.Atan2(startVector.y, startVector.x) * Mathf.Rad2Deg - 90;//-|

        #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -| 
        start.TriggerEvent(triggerObj);

        float time = 0;
        while (time <= duration)
        {
            #region Update  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|  
                
                triggerObj.transform.position += startVector.normalized * throwSpeed * Time.deltaTime;//-|

                float rotateZ = startAngleZ - time * spinSpeed;
                triggerObj.transform.rotation = Quaternion.Euler(0, 0, rotateZ);

            #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|  
            update.TriggerEvent(triggerObj);

            time += Time.deltaTime;
            yield return null;
        }

        end.TriggerEvent(triggerObj);
    }
    
    public override void Break()
    {
        weapon.WeaponDestroy();
    }

    public override void Removed()
    {
        //Ç® Á¦°Å
    }

    public override void Destroyed()
    {

    }
}
