using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonsterMgr : MonoBehaviour
{
    public int monsterNumbase = 5;
    public int baseDistance = 10;

    private WaveMgr waveMgr;
    public List<MonsterController> monsterList = new List<MonsterController>();

    public void Init(WaveMgr waveMgr)
    {
        this.waveMgr = waveMgr;
    }

    public void SpawnSingleMonster(E_MonsterType type)
    {
        float angle;
        Vector3 spawnPos;
        Collider2D collider;
        Vector3 playerPos = GameMgr.GetInstance().GetPlayerPos();
        Vector3 distancePos = (baseDistance * Vector3.right) + playerPos;

        do
        {
            UnityEngine.Random.InitState(GameMgr.GetInstance().GetRandomSeed());
            angle = UnityEngine.Random.Range(0, 361);
            spawnPos = GameMgr.GetInstance().RotateRound(distancePos, playerPos, Vector3.forward, angle);
            collider = Physics2D.OverlapBox(spawnPos, Vector2.one * 2, 0);
        } while (collider != null);

        GameObject monster = ObjectPool.GetInstance().OnSpawn("Monster");
        monster.transform.position = spawnPos;
        MonsterController monsterC = monster.GetComponent<MonsterController>();
        monsterC.type = type;

        monsterList.Add(monsterC);
        switch (type)
        {
            case E_MonsterType.Normal:
                monsterC.HP = 100;
                monsterC.attack = 30;
                monsterC.moveSpeed = 2;
                monsterC.knockIntensity = 1.0f;
                monsterC.monsterMgr = this;

                monster.transform.DOScale(Vector3.one * 2,1f).onComplete += ()=>
                {
                    monsterC.currentState = E_MonsterState.chase;
                };

                break;

            case E_MonsterType.AGI:
                monsterC.HP = 50;
                monsterC.attack = 10;
                monsterC.moveSpeed = 3;
                monsterC.knockIntensity = 1.0f;
                monsterC.monsterMgr = this;

                monster.transform.DOScale(Vector3.one, 1f).onComplete += () =>
                {
                    monsterC.currentState = E_MonsterState.chase;
                };
                break;

            case E_MonsterType.Tank:
                monsterC.HP = 300;
                monsterC.attack = 50;
                monsterC.moveSpeed = 1;
                monsterC.knockIntensity = 0.5f;
                monsterC.monsterMgr = this;
                monster.transform.DOScale(Vector3.one * 3, 1f).onComplete += () =>
                {
                    monsterC.currentState = E_MonsterState.chase;
                };
                break;

            default:
                break;
        }
    }

    public void RemoveMonster(MonsterController monster)
    {
        monsterList.Remove(monster);
        if (monsterList.Count == 0) 
        {
            waveMgr.StartNextWave();
        }
    }

    public void ClearMonster() 
    {
        this.monsterList.Clear();
    }

    public void ResetMonsterState() 
    {
        foreach (var item in monsterList)
        {
            item.currentState = E_MonsterState.idle;
        }
    }
}

public enum E_MonsterType 
{
    Normal,
    AGI,
    Tank
}

public enum E_MonsterState 
{
    idle,
    chase,
    dead
}
