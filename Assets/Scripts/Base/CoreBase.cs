using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoreBase:MonoBehaviour
{
    public string bulletPrefab;

    //������ʱ��
    public float fireInterval;
	//��ʱ��
	protected float timer;

	//���������־λ
	[HideInInspector]
	public bool canFire = true;


	//ÿ�����ľ��в�ͬ���������
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
