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
    [HideInInspector] public Camera cam;
    [HorizontalGroup("Move")] public CameraMove camMove;
    [HorizontalGroup("Move")][HideLabel] public float speed;
    public float posZ = -10;

    void Awake()
    {
        Player.camCon = this;
        cam = GetComponent<Camera>();
    }
    void Update()
    {
        switch (camMove)
        {
            case CameraMove.Stop:
                break;
            case CameraMove.Perfect:
                transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, posZ);
                break;
            case CameraMove.MoveTo:
                if ((Player.transform.position - transform.position).magnitude == 0) break;

                if (((Vector2)Player.transform.position - (Vector2)transform.position).magnitude < speed * Time.deltaTime)
                    transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, posZ);
                else transform.position += (Vector3)(
                        ((Vector2)Player.transform.position - (Vector2)transform.position).normalized * speed * Time.deltaTime);
                break;
        }
    }
    public void SelectCamMove(CameraMove mode, float _speed)
    {
        camMove = mode;
        speed = _speed;
    }
}
