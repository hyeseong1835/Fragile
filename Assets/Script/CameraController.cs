using Sirenix.OdinInspector;
using UnityEngine;

public enum CameraMove
{
    Stop, Perfect, MoveTo
}
public class CameraController : MonoBehaviour
{
    public static CameraController instance { get; private set; }

    [LabelWidth(Editor.labelWidth)]
    public Transform target;
    
    [Required]
    [LabelWidth(Editor.labelWidth)]
    public Camera cam;

    [HorizontalGroup("Move")]
    #region Move  - - - - - - - - - - - - - - - - - - -| 

        [LabelWidth(Editor.labelWidth)]
        public CameraMove camMove = CameraMove.Stop;//-|
                                                        [HorizontalGroup("Move", width: Editor.shortNoLabelPropertyWidth)]
        [HideLabel]
        public float speed;

    #endregion  - - - - - - - - - - - - - - - - - - - -|

    [LabelWidth(Editor.labelWidth)]
    public float posZ = -10;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }
    void Update()
    {
        target = PlayerController.instance.transform;

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
