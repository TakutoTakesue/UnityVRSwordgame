using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WizardAction : EnemyScript
{
    private SphereCollider myRange;
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
    [Header("視野角 : (0～180)度")]
    [SerializeField] private float searchAngle;
    [Header("感知範囲 : m")]
    [SerializeField] private float serchRange;
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

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        myRB = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
        Instantiate(entryEffect, transform.position + new Vector3(0, 0, 0), transform.rotation);
        isTarget = false;
        attackInterval = AttackIntervalTime;
        chargeInterval = ChargeIntervalTime;
        battleFlg = true;
        fireBallSize = 0.1f;
        magicPos = transform.Find("MagicPos").gameObject;
        myRange = GetComponent<SphereCollider>();
        myRange.radius = serchRange;
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
                        fireMagic.GetComponent<Rigidbody>().velocity = (targetPlayer.transform.position - fireMagic.transform.position).normalized * fireBallSpeed;
                        fireBallSize = 0.1f;
                    }
                }
                break;
        }

        if (isTarget)
        {
            if (myState == State.Idle || myState == State.Attack)
            {
                // プレイヤーの方に向く
                gameObject.transform.LookAt(new Vector3(targetPlayer.transform.position.x,transform.position.y, targetPlayer.transform.position.z));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && targetPlayer == null)
        {
            //　プレイヤーの方向
            Vector3 playerDirection = other.transform.position - transform.position;
            //　敵の前方からのプレイヤーの方向
            float angle = Vector3.Angle(transform.forward, playerDirection);
            //　サーチする角度内だったらプレイヤー発見
            if (angle <= searchAngle)
            {
                targetPlayer = other.gameObject;
                isTarget = true;
                myAnim.SetBool("Discover", true);
                myState = State.Idle;
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

#if UNITY_EDITOR
    //　サーチする角度表示
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        if (myRange == null)
        {
            Handles.DrawSolidArc(new Vector3(transform.position.x, 0, transform.position.z), 
                                     Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, 
                                     searchAngle * 2f, 0);
        }
        else
        {
            Handles.DrawSolidArc(new Vector3(transform.position.x, 0, transform.position.z), 
                                     Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, 
                                     searchAngle * 2f, myRange.radius);
        }
        
    }
#endif
}
