using Unity.VisualScripting;

[UnitTitle("Prevent Attack Input")]
[UnitCategory("Events/Weapon/Action")]
public class PreventAttackInput : WeaponNode
{
    protected override void Act(Flow flow)
    {
        base.Act(flow);

        weapon.con.readyPreventAttackInput = true;
    }
}
[UnitTitle("Allow Attack Input")]
[UnitCategory("Events/Weapon/Action")]
public class AllowAttackInput : WeaponNode
{
    protected override void Act(Flow flow)
    {
        base.Act(flow);

        weapon.con.readyPreventAttackInput = false;
        weapon.con.preventAttackInput = false;
    }
}
[UnitTitle("Prevent Special Input")]
[UnitCategory("Events/Weapon/Action")]
public class PreventSpecialInput : WeaponNode
{
    protected override void Act(Flow flow)
    {
        base.Act(flow);

        weapon.con.readyPreventSpecialInput = true;
    }
}
[UnitTitle("Allow Special Input")]
[UnitCategory("Events/Weapon/Action")]
public class AllowSpecialInput : WeaponNode
{
    protected override void Act(Flow flow)
    {
        base.Act(flow);

        weapon.con.readyPreventSpecialInput = false;
        weapon.con.preventSpecialInput = false;
    }
}