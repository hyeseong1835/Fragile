using System.Collections;
using UnityEngine;
using WeaponSystem;

public class Skill_Shoot : Skill, IVoid
{
    [SerializeField] IVoid[] start, update, end;

    [SerializeField] IController[] selfEnter, selfStay, selfExit;
    [SerializeField] IController[] friendEnter, friendStay, friendExit;
    [SerializeField] IController[] enemyEnter, enemyStay, enemyExit;
    [SerializeField] IController[] objectEnter, objectStay, objectExit;

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
    
    public void InputVoid()
    {
        StartCoroutine(Shoot());
    }
    IEnumerator Shoot()
    {
        #region Start  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|  
        
            TriggerObject triggerObj = pool.Use().GetComponent<TriggerObject>();
            triggerObj.Init
            (
                con,
                selfEnter, selfStay, selfExit,
                friendEnter, friendStay, friendExit,
                enemyEnter, enemyStay, enemyExit,
                objectEnter, objectStay, objectExit
            );
            triggerObj.transform.position = con.transform.position;

            Vector3 startVector = con.targetDir;
            float startAngleZ = Mathf.Atan2(startVector.y, startVector.x) * Mathf.Rad2Deg - 90;//-|

        #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -| 
        IVoid.Invoke(start);

        float time = 0;
        while (time <= duration)
        {
            #region Update  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|  
                
                triggerObj.transform.position += startVector.normalized * throwSpeed * Time.deltaTime;//-|

                float rotateZ = startAngleZ - time * spinSpeed;
                triggerObj.transform.rotation = Quaternion.Euler(0, 0, rotateZ);

            #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|  
            IVoid.Invoke(update);

            time += Time.deltaTime;
            yield return null;
        }

        IVoid.Invoke(end);
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
