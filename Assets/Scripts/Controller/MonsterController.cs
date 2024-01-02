using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class MonsterController : MonoBehaviour
{
    [Range(0, 1)]
    public float knockIntensity = 1f;
    public float moveSpeed;

    private int hp = 100;
    public int HP {
        get {
            return this.hp;
        }
        set {
            this.hp = value;

            if (this.hp <= 0) {
                this.hp = 0;
                this.currentState = E_MonsterState.dead;
                animator.SetTrigger("die");
            }
        }
    }
    public int attack;

    public E_MonsterState currentState;
    public E_MonsterType type;

    public MonsterMgr monsterMgr;
    public Sprite[] monsterSprites;
    //渲染实体
    private Transform spriteEntity;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Awake()
    {
        spriteEntity = this.transform.Find("SpriteEntity");
        spriteRenderer = spriteEntity.GetComponent<SpriteRenderer>();
        animator = this.GetComponent<Animator>();
    }

    void Update()
    {
        if (currentState == E_MonsterState.chase && GameMgr.GetInstance().GamePause == false)
        {
            //获取移动向量
            Vector3 chaseDir = (GameMgr.GetInstance().GetPlayerPos() - this.transform.position).normalized;

            this.transform.Translate(chaseDir * moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentState == E_MonsterState.dead) return;

        switch (LayerMask.LayerToName(collision.gameObject.layer))
        {
            case "Wall":
            break;

            case "Bullet":
                animator.SetTrigger("damage");
                break;

            case "Effect":
                animator.SetTrigger("damage");
                break;
            default:
                break;
        }

    }

    public void KnockBack(Vector3 knockDir) 
    {
        DOTween.Kill("KnockBack");
        transform.DOLocalMove(knockDir * knockIntensity + this.transform.localPosition,0.1f).SetId("KnockBack");
    }

    public void dieEvenet() 
    {
        //随机创建尸体
        System.Random random = new System.Random();
        int id = random.Next(1,monsterSprites.Length);
        GameObject corpse = ObjectPool.GetInstance().OnSpawn("Corpse");
        corpse.transform.localPosition = this.transform.localPosition;
        corpse.transform.localRotation = this.transform.localRotation;
        SpriteRenderer sr = corpse.GetComponent<SpriteRenderer>();
        sr.sprite = monsterSprites[id];

        monsterMgr.RemoveMonster(this);
        spriteRenderer.sprite = monsterSprites[0];
        this.transform.localScale = Vector3.one * 0.1f;

        ObjectPool.GetInstance().OnUnspawn(this.gameObject);
    }
}
