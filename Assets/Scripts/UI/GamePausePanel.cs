using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePausePanel : BasePanel
{
    private Button btnContinue;
    private Button btnRestart;
    private Button btnBack;

    protected override void Awake()
    {
        base.Awake();

        btnContinue = this.transform.Find("btnContinue").GetComponent<Button>();
        btnRestart = this.transform.Find("btnRestart").GetComponent<Button>();
        btnBack = this.transform.Find("btnBack").GetComponent<Button>();
    }

    protected override void Init()
    {
        btnRestart.onClick.AddListener(()=> 
        {
            GameMgr.GetInstance().ResumeGame();
            GameMgr.GetInstance().ReStartGame();
        });

        btnBack.onClick.AddListener(() =>
        {
            GameMgr.GetInstance().ResumeGame();
            GameMgr.GetInstance().BackToMainMenu();
        });

        btnContinue.onClick.AddListener(() =>
        {
            GameMgr.GetInstance().ResumeGame();
        });
    }

}
