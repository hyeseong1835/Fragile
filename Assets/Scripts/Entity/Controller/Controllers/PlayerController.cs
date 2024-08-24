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

        //curWeapon.data.rule.attackInvoker.input = callback.ReadValueAsButton();
    }
    public void OnSpecialInput(InputAction.CallbackContext callback)
    {
        if (curWeapon == null) return;

        //curWeapon.data.rule.specialInvoker.input = callback.ReadValueAsButton();
    }
}