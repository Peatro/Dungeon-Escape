using UnityEngine;

public class Skeleton : Enemy, IDamageable
{
    protected override void Init()
    {
        base.Init();
    }

    protected override void Movement()
    {
        base.Movement();
    }

    public override void Damage()
    {
        base.Damage();
        Debug.Log("Skeleton::Damage()");
    }
}
