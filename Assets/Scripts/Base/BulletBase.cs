using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
	//子弹速度
	public float bulletSpeed = 10;
	//子弹威力
	public int damagePower = 50;
	//命中后产生爆炸的概率
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
			//播放特效
			GameObject eff = ObjectPool.GetInstance().OnSpawn("HitEff");
            eff.transform.position = this.transform.position;
            eff.transform.rotation = this.transform.rotation;

            GameMgr.GetInstance().UnspawnGameObjectDelay(eff,0.25f);
		}
		else if (collision.gameObject.layer == LayerMask.NameToLayer("Monster")) 
		{
			MonsterController monsterc = collision.GetComponent<MonsterController>();
			ObjectPool.GetInstance().OnUnspawn(this.gameObject);

			//播放命中特效
			//有概率产生爆炸
			Random.InitState(GameMgr.GetInstance().GetRandomSeed());
			int rateInt = Random.Range(0, 101);
			GameObject eff = ObjectPool.GetInstance().OnSpawn(rateInt <= explodePossibility ? "ExplodeEff" : "HitEff");
			eff.transform.position = this.transform.position;
			eff.transform.rotation = this.transform.rotation;

			GameMgr.GetInstance().UnspawnGameObjectDelay(eff, 0.25f);
			//怪物受击
			monsterc.KnockBack(this.transform.right);
			//怪物收到伤害
			monsterc.HP -= damagePower;
		}
	}
}