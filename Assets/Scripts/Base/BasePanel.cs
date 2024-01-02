using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    private bool isShow = false;
    public int fadeTime = 10;
    private CanvasGroup canvas;
    private UnityAction hideCallBack;

    protected abstract void Init();

    protected virtual void Awake()
    {
        canvas = this.GetComponent<CanvasGroup>();
        if (canvas == null)
            canvas = this.gameObject.AddComponent<CanvasGroup>();
    }

    protected virtual void Start() 
    {
        Init();
    }

    public virtual void ShowMe() 
    {
        this.gameObject.SetActive(true);

        isShow = true;
        canvas.alpha = 0;
    }

    public virtual void HideMe(UnityAction action) 
    {
        isShow = false;
        canvas.alpha = 1;
        this.hideCallBack = action;

        this.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        if (isShow == false && canvas.alpha!=0)
        {
            canvas.alpha -= Time.unscaledDeltaTime * fadeTime;
            if (canvas.alpha <= 0) 
            {
                canvas.alpha = 0;
                hideCallBack?.Invoke();
            }
        }
        else if(isShow == true && canvas.alpha != 1)
        {
            canvas.alpha += Time.unscaledDeltaTime * fadeTime;
            if (canvas.alpha >= 1) 
            {
                canvas.alpha = 1;
            }
        }
    }
}
