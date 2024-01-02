using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartPanel : BasePanel
{
    private Image imgTitle;

    private Button btnStart;
    private Button btnGuide;
    private Button btnBack;
    private Button btnExit;

    private Transform guide;

    protected override void Awake()
    {
        base.Awake();
        imgTitle = this.transform.Find("imgTitle").GetComponent<Image>();
        btnStart = this.transform.Find("btnStart").GetComponent<Button>();
        btnGuide = this.transform.Find("btnGuide").GetComponent<Button>();
        btnExit = this.transform.Find("btnExit").GetComponent<Button>();
        guide = this.transform.Find("Guide");

        btnBack = guide.Find("btnBack").GetComponent<Button>();
    }

    protected override void Init()
    {
        ShowMe();

        btnStart.onClick.AddListener(()=> 
        {
            HideMe(null);
            GameMgr.GetInstance().StartGame();
        });

        btnGuide.onClick.AddListener(() =>
        {
            imgTitle.gameObject.SetActive(false);
            btnStart.gameObject.SetActive(false);
            btnGuide.gameObject.SetActive(false);
            btnExit.gameObject.SetActive(false);

            guide.gameObject.SetActive(true);
        });

        btnBack.onClick.AddListener(() =>
        {
            imgTitle.gameObject.SetActive(true);
            btnStart.gameObject.SetActive(true);
            btnGuide.gameObject.SetActive(true);
            btnExit.gameObject.SetActive(true);

            guide.gameObject.SetActive(false);
        });

        btnExit.onClick.AddListener(() =>
        {
            Application.Quit(0);
        });

        guide.gameObject.SetActive(false);
    }

}
