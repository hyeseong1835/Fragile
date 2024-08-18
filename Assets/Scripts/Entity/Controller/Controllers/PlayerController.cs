using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{
    protected void OnEnable()
    {

    }
    new protected void Update()
    {
        base.Update();
    }
    public void OnMoveInput(InputAction.CallbackContext callback)
    {
        moveInput = callback.ReadValue<Vector2>();
    }
    public void OnAttackInput(InputAction.CallbackContext callback)
    {
        if (curWeapon == null) return;

        curWeapon.attackInput = callback.ReadValueAsButton();
    }
    public void OnSpecialInput(InputAction.CallbackContext callback)
    {
        if (curWeapon == null) return;

        curWeapon.specialInput = callback.ReadValueAsButton();
    }
}