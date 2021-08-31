using UnityEngine;

public class MossGiant : Enemy, IDamageable
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
        Debug.Log("MossGiant::Damage()");
    }
}

