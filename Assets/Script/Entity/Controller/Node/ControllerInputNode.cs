using Unity.VisualScripting;

[UnitTitle("Prevent Attack Input")]
[UnitCategory("Events/Weapon/Action")]
public class PreventAttackInput : GetComponentActNode<Weapon>
{
    protected override void Act(Flow flow)
    {
        component.con.readyPreventAttackInput = true;
    }
}
[UnitTitle("Allow Attack Input")]
[UnitCategory("Events/Weapon/Action")]
public class AllowAttackInput : GetComponentActNode<Weapon>
{
    protected override void Act(Flow flow)
    {
        component.con.readyPreventAttackInput = false;
        component.con.preventAttackInput = false;
    }
}
[UnitTitle("Prevent Special Input")]
[UnitCategory("Events/Weapon/Action")]
public class PreventSpecialInput : GetComponentActNode<Weapon>
{
    protected override void Act(Flow flow)
    {
        component.con.readyPreventSpecialInput = true;
    }
}
[UnitTitle("Allow Special Input")]
[UnitCategory("Events/Weapon/Action")]
public class AllowSpecialInput : GetComponentActNode<Weapon>
{
    protected override void Act(Flow flow)
    {
        component.con.readyPreventSpecialInput = false;
        component.con.preventSpecialInput = false;
    }
}