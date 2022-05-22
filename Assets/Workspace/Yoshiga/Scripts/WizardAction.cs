using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAction : EnemyScript
{
    private GameObject player;
    private Rigidbody myRB;
    private Animator myAnim;
    public enum State
    {
        Entry,
        Idle,
        Discover,
        Attack,
        Wince,
        Hit,
        Death,
    }

    private State myState = State.Entry;    // 敵のステータス
    [Header("感知範囲 : m")]
    [SerializeField] private float range;
    [Header("登場時のエフェクト : Object")]
    [SerializeField] private GameObject entryEffect;
    [Header("詠唱エフェクト : Object")]
    [SerializeField] private GameObject chargeEffect;
    [Header("攻撃のインターバル : s")]
    [SerializeField] private float AttackIntervalTime;
    private float attackInterval;
    [Header("詠唱の時間 : s")]
    [SerializeField] private float ChargeIntervalTime;
    [Header("炎魔法 : Object")]
    [SerializeField] private GameObject fireBall;
    [Header("魔法の速度 : m/s")]
    [SerializeField] private float fireBallSpeed;
    private GameObject fireMagic;
    private ParticleSystem fireMagicPS;
    private GameObject magicPos; // 魔法の出る位置
    private float fireBallSize; // 炎魔法の大きさ
    private float chargeInterval;
    private bool isTarget;  // プレイヤーを見つけているかのフラグ
    private GameObject targetPlayer; // 狙っている敵(ターゲット)

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myRB = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
        Instantiate(entryEffect, transform.position + new Vector3(0, 0, 0), transform.rotation);
        isTarget = false;
        attackInterval = AttackIntervalTime;
        chargeInterval = ChargeIntervalTime;
        battleFlg = true;
        fireBallSize = 0.1f;
        magicPos = transform.Find("MagicPos").gameObject;
    }

    private void FixedUpdate()
    {
        switch (myState)
        {
            case State.Idle:
                if(isTarget)
                {
                    if(attackInterval > 0 && battleFlg)
                    {
                        attackInterval -= Time.deltaTime;
                        if(attackInterval <= 0)
                        {
                            attackInterval = AttackIntervalTime;
                            chargeEffect.SetActive(true);
                            myState = State.Attack;
                            myAnim.SetBool("Charge", true);
                            fireMagic = Instantiate(fireBall, magicPos.transform.position, chargeEffect.transform.rotation);
                            fireMagicPS = fireMagic.GetComponentInChildren<ParticleSystem>();
                        }
                    }
                }
                break;
            case State.Attack:

                // 詠唱
                if(chargeInterval > 0)
                {
                    chargeInterval -= Time.deltaTime;
                    fireBallSize += Time.deltaTime / (ChargeIntervalTime * 2);
                    fireMagicPS.startSize = fireBallSize * 1.5f;
                    fireMagic.transform.position = magicPos.transform.position;

                    if (chargeInterval <= 0)
                    {
                        myState = State.Idle;
                        chargeEffect.SetActive(false);
                        myAnim.SetBool("Charge", false);
                        chargeInterval = ChargeIntervalTime;
                        fireMagic.GetComponent<Rigidbody>().velocity = (player.transform.position - fireMagic.transform.position).normalized * fireBallSpeed;
                        fireBallSize = 0.1f;
                    }
                }
                break;
        }

        // プレイヤーを発見
        if (!isTarget && range >= Vector3.Distance(transform.position, player.transform.position))
        {
            isTarget = true;
            myAnim.SetBool("Discover", true);
            myState = State.Idle;
        }

        if (isTarget)
        {
            if (myState == State.Idle || myState == State.Attack)
            {
                // プレイヤーの方に向く
                gameObject.transform.LookAt(new Vector3(player.transform.position.x,transform.position.y,player.transform.position.z));
            }
        }
    }

    private void EntryFinish()
    {
        myAnim.SetBool("Entry", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
