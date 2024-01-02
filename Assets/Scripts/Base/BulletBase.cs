using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
	//�ӵ��ٶ�
	public float bulletSpeed = 10;
	//�ӵ�����
	public int damagePower = 50;
	//���к������ը�ĸ���
	[Range(0,100)]
	public int explodePossibility = 10;

	public abstract void Init();

	public virtual void Update()
	{
		this.transform.Translate(Vector3.right * Time.deltaTime * bulletSpeed);
	}

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
		{
			ObjectPool.GetInstance().OnUnspawn(this.gameObject);
			//������Ч
			GameObject eff = ObjectPool.GetInstance().OnSpawn("HitEff");
            eff.transform.position = this.transform.position;
            eff.transform.rotation = this.transform.rotation;

            GameMgr.GetInstance().UnspawnGameObjectDelay(eff,0.25f);
		}
		else if (collision.gameObject.layer == LayerMask.NameToLayer("Monster")) 
		{
			MonsterController monsterc = collision.GetComponent<MonsterController>();
			ObjectPool.GetInstance().OnUnspawn(this.gameObject);

			//����������Ч
			//�и��ʲ�����ը
			Random.InitState(GameMgr.GetInstance().GetRandomSeed());
			int rateInt = Random.Range(0, 101);
			GameObject eff = ObjectPool.GetInstance().OnSpawn(rateInt <= explodePossibility ? "ExplodeEff" : "HitEff");
			eff.transform.position = this.transform.position;
			eff.transform.rotation = this.transform.rotation;

			GameMgr.GetInstance().UnspawnGameObjectDelay(eff, 0.25f);
			//�����ܻ�
			monsterc.KnockBack(this.transform.right);
			//�����յ��˺�
			monsterc.HP -= damagePower;
		}
	}
}