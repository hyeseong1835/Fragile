using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{
    public override bool AttackInput => attackInput;
    public override bool SpecialInput => specialInput;

    bool attackInput = false;
    bool specialInput = false;
    
    protected void OnEnable()
    {

    }
    protected void Update()
    {
        Move();
    }
    void Move()
    {
        transform.position += (Vector3)(moveInput.normalized * Time.deltaTime);
    }
    public void OnMoveInput(InputAction.CallbackContext callback)
    {
        moveInput = callback.ReadValue<Vector2>();
    }
    public void OnAttackInput(InputAction.CallbackContext callback)
    {
        if (curWeapon == null) return;

        attackInput = callback.ReadValueAsButton();
    }
    public void OnSpecialInput(InputAction.CallbackContext callback)
    {
        if (curWeapon == null) return;

        specialInput = callback.ReadValueAsButton();
    }
}