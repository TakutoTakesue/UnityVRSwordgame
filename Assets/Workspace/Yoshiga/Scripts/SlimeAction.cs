using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAction : EnemyScript
{
    private GameObject player;
    private Rigidbody myRB;
    private Animator myAnim;
    public enum State
    {
        Idle,
        Walk,
        Attack,
        Wince,
        Hit,
        Death,
    }

    private State myState = State.Idle;    // 敵のステータス
    [Header("感知範囲 : m")]
    [SerializeField] private float range;
    [Header("移動範囲の半径 : m")]
    [SerializeField] private float moveRadius;
    [Header("Idle状態でその場にとどまる時間 : s")]
    [SerializeField] private float idleIntervalTime;
    private float idleInterval;
    private Vector3 destination; // 目的地
    private bool isTarget;  // プレイヤーを見つけているかのフラグ
    [Header("攻撃範囲 : m")]
    [SerializeField] private float attackRange;
    [Header("攻撃のインターバル : s")]
    [SerializeField] private float AttackIntervalTime;
    private float attackInterval;
    private Vector3 attackStartPos;  // 攻撃を始めた場所
    private Vector3 attackFinishPos; // 攻撃する時に向かう場所
    private float attackDistance; // 攻撃した時の敵とプレイヤーの距離
    private bool attackDirFlg = false;
    private float attackElapsed; // 攻撃の経過時間

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myRB = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
        isTarget = false;
        idleInterval = idleIntervalTime;
        attackInterval = AttackIntervalTime;
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
                    }

                    // プレイヤーとの距離が離れているとプレイヤーに向かっていく処理
                    if (attackRange + 2.0f <= Vector3.Distance(transform.position, player.transform.position))
                    {
                        myState = State.Walk;
                        myAnim.SetBool("Move", true);
                    }

                    if(attackInterval <= 0)
                    {
                        myState = State.Attack;
                        myRB.useGravity = false;
                        attackStartPos = transform.position;
                        attackFinishPos = player.transform.position + new Vector3(0, 1, 0);
                        attackDistance = Vector3.Distance(attackFinishPos, transform.position);
                    }

                }
                else
                {
                    if (idleInterval > 0 && !isTarget)
                    {
                        idleInterval -= Time.deltaTime;
                        if (idleInterval <= 0)
                        {
                            myAnim.SetTrigger("Jump");
                        }
                    }
                }               
                break;
            case State.Walk:
                #region 移動
                if (isTarget)
                {
                    if(attackRange >= Vector3.Distance(transform.position, player.transform.position))
                    {
                        myAnim.SetBool("Move", false);
                        myState = State.Idle;
                    }
                }
                else
                {
                    if (Vector3.Distance(transform.position, destination) <= 0.1f)
                    {
                        // 目的地に到着
                        myAnim.SetBool("Move", false);
                        myState = State.Idle;
                    }
                }
                #endregion
                break;
            case State.Attack:

                attackElapsed += Time.deltaTime;
                
                if (!attackDirFlg)
                {               
                    transform.position = Vector3.Lerp(attackStartPos, attackFinishPos, attackElapsed);
                    if(transform.position == attackFinishPos)
                    {
                        attackDirFlg = true;
                        attackElapsed = 0.0f;
                    }
                }
                else
                {
                    transform.position = Vector3.Lerp(attackFinishPos, attackStartPos, attackElapsed);
                    if (transform.position == attackStartPos)
                    {
                        attackDirFlg = false;
                        myState = State.Idle;
                        myRB.useGravity = true;
                        attackElapsed = 0.0f;
                        attackInterval = AttackIntervalTime;
                    }
                }
                break;
        }

        // プレイヤーを発見
        if(!isTarget && range >= Vector3.Distance(transform.position, player.transform.position))
        {
            isTarget = true;
            myState = State.Idle;
            myAnim.SetTrigger("Jump");
        }

        if (isTarget)
        {
            if(myState == State.Idle || myState == State.Walk)
            {
                // プレイヤーの方に向く
                gameObject.transform.LookAt(player.transform.position);
            }         
        }
    }

    public void JumpFinish()
    {
        myState = State.Walk;
        myAnim.SetBool("Move", true);   
        if(!isTarget)
        {
            destination = SetDestination();
            // 目的地を向く
            gameObject.transform.LookAt(destination);
            idleInterval = idleIntervalTime;
        }    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(myState == State.Attack && collision.gameObject.tag == "Player")
        {
            attackDirFlg = true;
            attackElapsed = 0.0f;
            attackFinishPos = transform.position;
        }
    }

    // 目的地をランダムで返す処理
    private Vector3 SetDestination()
    {
        Vector3 setPos = moveRadius * Random.insideUnitSphere + transform.position;
        return new Vector3(setPos.x,0,setPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
