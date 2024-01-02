using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : SingletonMono<GameMgr>
{
    private bool gamePause;
    public bool GamePause 
    {
        get { return gamePause; }
        set {
            gamePause = value;
            Time.timeScale = gamePause ? 0 : 1;
        }
    }

    public Vector3 playerBornPos;
    public Vector3 redCorePos;
    public Vector3 YellowCorePos;

    private PlayerController player;
    private WaveMgr waveMgr;
    private MonsterMgr monsterMgr;
    private ObjectPool objectPool;
    private Collider2D safeArea;

    public GameStartPanel gameStartPanel;
    public GamePausePanel gamePausePanel;
    public GameEndPanel gameEndPanel;

    protected override void Awake()
    {
        base.Awake();
        monsterMgr = this.GetComponent<MonsterMgr>();
        waveMgr = this.GetComponent<WaveMgr>();
        safeArea = GameObject.FindGameObjectWithTag("SafeArea").GetComponent<Collider2D>();

        objectPool = this.GetComponent<ObjectPool>();
        objectPool.RegisterNewPool("Monster");
        objectPool.RegisterNewPool("FireBullet");
        objectPool.RegisterNewPool("HitEff");
        objectPool.RegisterNewPool("ExplodeEff");
        objectPool.RegisterNewPool("Corpse");
    }
    private void Start()
    {
        gameStartPanel.ShowMe();
        gamePausePanel.HideMe(null);
        gameEndPanel.HideMe(null);
    }
    private void Init()
    {
        safeArea.isTrigger = true;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        GameObject yellowCore = GameObject.FindGameObjectWithTag("YellowCore");
        GameObject redCore = GameObject.FindGameObjectWithTag("RedCore");

        if (playerObj == null) playerObj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Player"));
        if (yellowCore == null) yellowCore = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/YellowCore"));
        if (redCore == null) redCore = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/RedCore"));

        yellowCore.transform.position = YellowCorePos;
        redCore.transform.position = redCorePos;

        playerObj.transform.localPosition = playerBornPos;
        playerObj.transform.localEulerAngles = Vector3.zero;

        this.player = playerObj.GetComponent<PlayerController>();
        CameraMgr.GetInstance().followTarget = playerObj.transform;
        monsterMgr.Init(waveMgr);
        waveMgr.Init(monsterMgr);
    }
    public void StartGame() 
    {
        ResumeGame();
        Init();
    }
    public void StartWave()
    {
        waveMgr.StartNextWave();
    }
    public void PauseGame() 
    {
        gamePausePanel.ShowMe();
        this.GamePause = true;
    }
    public void ResumeGame() 
    {
        gamePausePanel.HideMe(null);
        this.GamePause = false;
    }
    public void EndGame() 
    {
        monsterMgr.ResetMonsterState();
        gameEndPanel.ShowMe();
    }
    public void ReStartGame() 
    {
        ResumeGame();
        gameEndPanel.HideMe(null);
        objectPool.OnUnspawnAll();
        monsterMgr.ClearMonster();
        waveMgr.waveNum = 0;

        Init();
    }
    public void BackToMainMenu() 
    {
        PauseGame();
        gamePausePanel.HideMe(null);
        gameEndPanel.HideMe(null);
        gameStartPanel.ShowMe();

        objectPool.OnUnspawnAll();
        monsterMgr.ClearMonster();
        waveMgr.waveNum = 0;
        CameraMgr.GetInstance().ResetCameraPos();
    }

    #region 公共方法
    public Vector3 GetPlayerPos()
    {
        return this.player.transform.position;
    }
    public Vector3 RotateRound(Vector3 position, Vector3 center, Vector3 axis, float angle)
    {
        return Quaternion.AngleAxis(angle, axis) * (position - center) + center;
    }
    public int GetRandomSeed()
    {
        byte[] bytes = new byte[4];
        System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        rng.GetBytes(bytes);
        return BitConverter.ToInt32(bytes, 0);
    }
    public void UnspawnGameObjectDelay(GameObject go, float time)
    {
        IEnumerator coroutine = UnspawnGameObjectCoroutine(go, time);
        StartCoroutine(coroutine);
    }
    IEnumerator UnspawnGameObjectCoroutine(GameObject go,float time) 
    {
        yield return new WaitForSeconds(time);
        ObjectPool.GetInstance().OnUnspawn(go);
    }
    #endregion

}
