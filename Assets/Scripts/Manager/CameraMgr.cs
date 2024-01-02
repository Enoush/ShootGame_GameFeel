using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class CameraMgr : SingletonMono<CameraMgr>
{
    public float smoothInputSpeed = .2f;
    public Vector3 bornPos;
    public Vector3 offsetVector;
    private Vector3 smoothInputVelocity;
    private Vector3 targetPos;

    public Transform followTarget;
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = this.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (followTarget == null) return;

        targetPos = followTarget.position + offsetVector;
        this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPos, ref smoothInputVelocity, smoothInputSpeed);
    }

    public void DoCameraShake(Vector3 shakePos, float duration = 0.2f,int vibrato = 10,UnityAction finishAction = null)
    {
        this.transform.DOShakePosition(duration, shakePos, vibrato).onComplete += () =>
        {
            finishAction?.Invoke();
        };
    }

    public void DoPlayerHurtEff()
    {
        animator.SetTrigger("Hurt");
    }

    public void ResetCameraPos() 
    {
        this.transform.position = bornPos;
    }
}
