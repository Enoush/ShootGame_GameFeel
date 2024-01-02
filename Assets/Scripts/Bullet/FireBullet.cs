using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : BulletBase
{
    public override void Init()
    {

    }

    public override void Update()
    {
        base.Update();
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
