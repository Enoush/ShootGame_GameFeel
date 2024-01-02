using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndPanel : BasePanel
{
    private Button btnRestart;
    private Button btnBack;

    protected override void Awake()
    {
        base.Awake();
        btnRestart = this.transform.Find("btnRestart").GetComponent<Button>();
        btnBack = this.transform.Find("btnBack").GetComponent<Button>();
    }

    protected override void Init()
    {
        btnRestart.onClick.AddListener(()=> 
        {
            GameMgr.GetInstance().ReStartGame();
        });

        btnBack.onClick.AddListener(() =>
        {
            GameMgr.GetInstance().BackToMainMenu();
        });
    }


}
