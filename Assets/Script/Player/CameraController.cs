using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public enum CameraMove
{
    Stop, Perfect, MoveTo
}
public class CameraController : MonoBehaviour
{
    [SerializeField] 
        Transform target;
    
    [Required] 
        public Camera cam;

    [HorizontalGroup("Move")]
    #region Move
    
            public CameraMove camMove = CameraMove.Stop;

        [HorizontalGroup("Move", width:30)]
            [HideLabel]
            public float speed;

    #endregion
    public float posZ = -10;

    void Update()
    {
        if (target == null) return;

        switch (camMove)
        {
            case CameraMove.Stop:
                break;
            case CameraMove.Perfect:
                transform.position = new Vector3(target.position.x, target.position.y, posZ);
                break;
            case CameraMove.MoveTo:
                if ((target.position - transform.position).magnitude == 0) break;

                if (((Vector2)target.position - (Vector2)transform.position).magnitude < speed * Time.deltaTime)
                    transform.position = new Vector3(target.position.x, target.position.y, posZ);
                else transform.position += (Vector3)(
                        ((Vector2)target.position - (Vector2)transform.position).normalized * speed * Time.deltaTime);
                break;
        }
    }
    public void SelectCamMove(CameraMove mode, float _speed)
    {
        camMove = mode;
        speed = _speed;
    }
}
