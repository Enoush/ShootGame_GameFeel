
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.Experimental.Rendering.Universal;
using System;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 10f;  //最大速度
    public float smoothInputSpeed = .2f;

    private int hp = 100;
    public int HP
    {
        get
        {
            return this.hp;
        }
        set
        {
            this.hp = value;

            if (this.hp <= 0)
            {
                this.hp = 0;
                Die();
            }
        }
    }

    //开火后坐力
    public float fireRecoil = 0.1f;
    //开火精准度偏移值
    public int fireDeviationAngle = 5;

    private E_PlayerState currentState;

    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;

    private Animator animator;

    //渲染实体
    private Transform spriteEntity;
    //当前的核心
    private CoreBase core;

    private Transform coreTrans;
    //武器位置
    private Transform weaponTrans;
    //发射子弹位置
    private Transform fireTrans;
    //开火特效
    private Light2D fireEff;

    private PlayerInput input;
    private InputAction moveAction;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        input = this.GetComponent<PlayerInput>();
        spriteEntity = this.transform.Find("SpriteEntity");
        core = spriteEntity.Find("Core").GetChild(0).GetComponent<CoreBase>();
        fireEff = spriteEntity.Find("FireEff").GetComponent<Light2D>();
        coreTrans = spriteEntity.Find("Core");
        weaponTrans = spriteEntity.Find("Weapon");
        fireTrans = weaponTrans.Find("FirePos");
    }

    private void Start()
    {
        input.onActionTriggered += OnActionTrigger;
        moveAction = input.actions["Move"];

        fireEff.intensity = 0;
    }

    private void Update()
    {
        if (currentState == E_PlayerState.dead || currentState == E_PlayerState.stiff || GameMgr.GetInstance().GamePause) return;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 v = (worldMousePos - spriteEntity.position).normalized;
        spriteEntity.right = v;
    }
    private void FixedUpdate()
    {
        if (currentState == E_PlayerState.dead || currentState == E_PlayerState.stiff) return;

        Vector2 input = moveAction.ReadValue<Vector2>();
        currentInputVector = Vector2.SmoothDamp(currentInputVector, input, ref smoothInputVelocity, smoothInputSpeed);
        this.transform.Translate(Time.fixedDeltaTime * currentInputVector * maxSpeed,Space.World);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Item")
        {
            CoreBase changeCore = collision.GetComponent<CoreBase>();
            if (core != null) ChangeCore(changeCore);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "SafeArea")
        {
            collision.isTrigger = false;
            GameMgr.GetInstance().StartWave();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Monster")
        {
            if (currentState != E_PlayerState.invincible || currentState != E_PlayerState.dead)
            {
                Hurt(collision);
            }
        }
    }
    private void OnActionTrigger(InputAction.CallbackContext context)
    {
        if (GameMgr.GetInstance().GamePause) return;

        switch (context.action.name)
        {
            case "Fire":

                if (context.phase == InputActionPhase.Performed)
                {
                    if (core.canFire == false || core == null) return;

                    Vector3 firPos = this.fireTrans.position;
                    Vector3 firDir = spriteEntity.right;

                    //后座力
                    spriteEntity.DOPunchPosition(-spriteEntity.right * fireRecoil, 0.1f, 2, 0.5f);
                    //相机震动
                    CameraMgr.GetInstance().DoCameraShake(-spriteEntity.right * 0.1f, 0.05f);
                    //枪口后坐力
                    weaponTrans.DOPunchPosition(-transform.right * fireRecoil, 0.1f, 2, 0.5f);

                    //开火特效
                    fireEff.intensity = 1;
                    fireEff.pointLightInnerRadius = 0f;
                    fireEff.pointLightOuterRadius = 0.2f;
                    StopAllCoroutines();
                    StartCoroutine("reduceLightEff");

                    core.Fire(firPos, firDir, fireDeviationAngle);
                }
                break;
            case "Exit":
                GameMgr.GetInstance().PauseGame();
                break;
            case "Move":
                if (context.phase == InputActionPhase.Performed)
                {
                    currentState = E_PlayerState.move;
                }
                else if (context.phase == InputActionPhase.Canceled)
                {
                    currentState = E_PlayerState.idle;
                }
                break;

            default:
                break;
        }
    }
    private void Hurt(Collision2D collision)
    {
        animator.SetTrigger("Hurt");
        StartCoroutine("StiffCoroutine");

        Vector3 endPos = this.transform.position - (collision.transform.position - this.transform.position) * 3;
        Collider2D collider = Physics2D.OverlapBox(endPos, Vector2.one * 2, 0);
        if (collider == null) this.transform.DOMove(endPos, 1.0f);
        this.HP -= collision.gameObject.GetComponent<MonsterController>().attack;


        CameraMgr.GetInstance().DoPlayerHurtEff();
        CameraMgr.GetInstance().DoCameraShake(new Vector3(1, 1, 0), 0.5f, 10);
    }
    public void Restore()
    {
        this.currentState = E_PlayerState.idle;
    }
    private void Die()
    {
        this.currentState = E_PlayerState.dead;
        GameObject go = ObjectPool.GetInstance().OnSpawn("ExplodeEff");
        CameraMgr.GetInstance().DoCameraShake(new Vector3(1, 1, 0), 0.5f, 10);
        go.transform.position = this.transform.position;
        go.transform.rotation = this.transform.rotation;

        Destroy(this.gameObject);
        GameMgr.GetInstance().EndGame();
        return;
    }
    private void ChangeCore(CoreBase changeCore)
    {
        Transform trans = coreTrans.GetChild(0);

        changeCore.transform.position = trans.position;
        changeCore.transform.rotation = trans.rotation;
        changeCore.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        coreTrans.DetachChildren();

        Destroy(trans.gameObject);
        this.core = changeCore;
        changeCore.tag = "Player";
        changeCore.transform.SetParent(coreTrans);
        changeCore.transform.localScale = Vector3.one;
    }
    IEnumerator StiffCoroutine() 
    {
        this.currentState = E_PlayerState.stiff;
        yield return new WaitForSeconds(0.25f);
        this.currentState = E_PlayerState.invincible;
    }
    IEnumerator reduceLightEff()
    {
        while (fireEff.intensity > 0)
        {
            fireEff.intensity -= 0.1f;
            fireEff.pointLightInnerRadius += 0.1f;
            fireEff.pointLightOuterRadius += 0.1f;
            yield return new WaitForEndOfFrame();
        }
    }
}
public enum E_PlayerState
{
    idle,
    move,
    stiff,
    invincible,
    dead
}
