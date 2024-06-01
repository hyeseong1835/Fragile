using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerControllerData : ControllerData<PlayerController, PlayerControllerData>
{
    [Required][PropertyOrder(0)]
    [LabelText("Cam Controller")][LabelWidth(Editor.propertyLabelWidth)]
    public CameraController camCon;
}
