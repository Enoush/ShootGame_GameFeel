using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RedCore : CoreBase
{
    public int baseAngleOffset = 10;

    public override void Fire(Vector3 bulletPos,Vector3 bulletDir,int DeviationAngle)
    {
        UnityEngine.Random.InitState(GameMgr.GetInstance().GetRandomSeed());
        float angle = UnityEngine.Random.Range(-DeviationAngle, DeviationAngle + 1);

        GameObject bullet1 = ObjectPool.GetInstance().OnSpawn(bulletPrefab);
        bullet1.transform.localPosition = bulletPos;
        bullet1.transform.right = bulletDir;
        bullet1.transform.RotateAround(bulletPos, Vector3.forward, angle + baseAngleOffset);

        GameObject bullet2 = ObjectPool.GetInstance().OnSpawn(bulletPrefab);
        bullet2.transform.localPosition = bulletPos;
        bullet2.transform.right = bulletDir;
        bullet2.transform.RotateAround(bulletPos, Vector3.forward, angle - baseAngleOffset);

        GameObject bullet3 = ObjectPool.GetInstance().OnSpawn(bulletPrefab);
        bullet3.transform.localPosition = bulletPos;
        bullet3.transform.right = bulletDir;
        bullet3.transform.RotateAround(bulletPos, Vector3.forward, angle);

        base.Fire(bulletPos, bulletDir, DeviationAngle);
    }
}
