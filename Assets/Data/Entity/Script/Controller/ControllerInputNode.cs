using Unity.VisualScripting;

[UnitTitle("Prevent Attack Input")]
[UnitCategory("Events/Weapon/Action")]
public class PreventAttackInput : ActNode
{
    Weapon weapon;

    protected override void Act(Flow flow)
    {
        if (weapon == null) weapon = flow.stack.gameObject.GetComponent<Weapon>();

        weapon.con.readyPreventAttackInput = true;
    }
}
[UnitTitle("Allow Attack Input")]
[UnitCategory("Events/Weapon/Action")]
public class AllowAttackInput : ActNode
{
    Weapon weapon;

    protected override void Act(Flow flow)
    {
        if (weapon == null) weapon = flow.stack.gameObject.GetComponent<Weapon>();

        weapon.con.readyPreventAttackInput = false;
        weapon.con.preventAttackInput = false;
    }
}
[UnitTitle("Prevent Special Input")]
[UnitCategory("Events/Weapon/Action")]
public class PreventSpecialInput : ActNode
{
    Weapon weapon;

    protected override void Act(Flow flow)
    {
        if (weapon == null) weapon = flow.stack.gameObject.GetComponent<Weapon>();

        weapon.con.readyPreventSpecialInput = true;
    }
}
[UnitTitle("Allow Special Input")]
[UnitCategory("Events/Weapon/Action")]
public class AllowSpecialInput : ActNode
{
    Weapon weapon;
    protected override void Act(Flow flow)
    {
        if (weapon == null) weapon = flow.stack.gameObject.GetComponent<Weapon>();

        weapon.con.readyPreventSpecialInput = false;
        weapon.con.preventSpecialInput = false;
    }
}