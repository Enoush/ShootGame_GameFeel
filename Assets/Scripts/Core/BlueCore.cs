using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCore : CoreBase
{
    public override void Fire(Vector3 bulletPos,Vector3 bulletDir, int DeviationAngle)
    {
        //精度调整
        UnityEngine.Random.InitState(GameMgr.GetInstance().GetRandomSeed());
        float angle = UnityEngine.Random.Range(-DeviationAngle, DeviationAngle + 1);

        GameObject bullet = ObjectPool.GetInstance().OnSpawn(bulletPrefab);
		bullet.transform.localPosition = bulletPos;
		bullet.transform.right = bulletDir;
        bullet.transform.RotateAround(bulletPos, Vector3.forward, angle);

        base.Fire(bulletPos, bulletDir, DeviationAngle);
	}
}
