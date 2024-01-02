using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoreBase:MonoBehaviour
{
    public string bulletPrefab;

    //射击间隔时间
    public float fireInterval;
	//计时器
	protected float timer;

	//允许射击标志位
	[HideInInspector]
	public bool canFire = true;


	//每个核心具有不同的射击类型
	public virtual void Fire(Vector3 bulletPos, Vector3 bulletDir,int DeviationAngle) 
	{
		canFire = false;
		timer = 0;
	}

	protected virtual void Update() 
	{
		timer += Time.deltaTime;
		if (timer >= fireInterval) canFire = true;
	}
}
