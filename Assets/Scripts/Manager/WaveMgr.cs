using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMgr : MonoBehaviour
{
    public int waveNum = 0;

    private MonsterMgr monsterMgr;

    public void Init(MonsterMgr monsterMgr) 
    {
        this.monsterMgr = monsterMgr;
    }

    public void StartNextWave() 
    {
        //根据当前波数决定当前波数生成的怪物数量
        //每波出现的怪物类型
        //50%普通怪物
        //20%敏捷怪物
        //30%坦克怪物
        E_MonsterType type = E_MonsterType.Normal;
        int count = waveNum + monsterMgr.monsterNumbase;

        for (int i = 0; i < count; i++)
        {
            UnityEngine.Random.InitState(GameMgr.GetInstance().GetRandomSeed());
            int randomNum = UnityEngine.Random.Range(0, 101);

            if (randomNum > 50 && randomNum < 80) type = E_MonsterType.Tank;
            else if (randomNum > 80) type = E_MonsterType.AGI;

            monsterMgr.SpawnSingleMonster(type);
        }

        waveNum++;
    }
}
