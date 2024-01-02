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
        //���ݵ�ǰ����������ǰ�������ɵĹ�������
        //ÿ�����ֵĹ�������
        //50%��ͨ����
        //20%���ݹ���
        //30%̹�˹���
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
