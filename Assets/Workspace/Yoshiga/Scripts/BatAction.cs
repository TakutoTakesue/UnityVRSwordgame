using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BatAction : EnemyScript
{
    private SphereCollider myRange;
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
    [Header("視野角 : (0～180)度")]
    [SerializeField] private float searchAngle;
    [Header("感知範囲 : m")]
    [SerializeField] private float serchRange;
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
    private Vector3 attackStartPos;  // 攻撃を仕掛ける時にいた位置
    private Vector3 attackFinishPos;　// 攻撃を仕掛ける位置
    private float attackDistance;
    private bool attackDirFlg = false;
    private float attackElapsed;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        myRB = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
        isTarget = false;
        idleInterval = idleIntervalTime;
        attackInterval = AttackIntervalTime;
        myRange = GetComponent<SphereCollider>();
        myRange.radius = serchRange;
    }

    private void FixedUpdate()
    {
        switch (myState)
        {
            case State.Idle:

                if (isTarget)
                {
                    if (attackInterval > 0 && battleFlg)
                    {
                        attackInterval -= Time.deltaTime;
                    }

                    // 少し離れたら追いかける処理
                    if (attackRange + 2.0f < Vector3.Distance(transform.position, targetPlayer.transform.position))
                    {
                        myState = State.Walk;
                        myAnim.SetBool("Move", true);
                        break;
                    }

                    if (attackInterval <= 0)
                    {
                        myState = State.Attack;
                        myRB.useGravity = false;
                        attackStartPos = transform.position;
                        attackFinishPos = targetPlayer.transform.position + new Vector3(0, 1, 0);
                        attackDistance = Vector3.Distance(attackFinishPos, transform.position);
                    }
                }
                else
                {
                    if (idleInterval > 0 && !isTarget)
                    {
                        idleInterval -= Time.deltaTime;
                        if(idleInterval <= 0)
                        {
                            myState = State.Walk;
                            myAnim.SetBool("Move", true);
                            destination = SetDestination();
                            // 目的地を向く
                            gameObject.transform.LookAt(new Vector3(destination.x, transform.position.y, destination.z));
                            myRB.velocity = (destination - transform.position).normalized * mySpeed;
                            idleInterval = idleIntervalTime;
                        }                       
                    }
                }
                break;
            case State.Walk:

                if (isTarget)
                {
                    myRB.velocity = (new Vector3(targetPlayer.transform.position.x,
                                                 transform.position.y,
                                                 targetPlayer.transform.position.z) - transform.position).normalized * mySpeed;

                    if (attackRange >= Vector3.Distance(transform.position, targetPlayer.transform.position))
                    {
                        myAnim.SetBool("Move", false);
                        myRB.velocity = Vector3.zero;
                        myState = State.Idle;
                    }
                }
                else
                {
                    // 目的地に到着
                    if (Vector3.Distance(transform.position, destination) <= 0.1f)
                    {                       
                        myAnim.SetBool("Move", false);
                        myRB.velocity = Vector3.zero;
                        myState = State.Idle;
                    }
                }
                break;
            case State.Attack:

                attackElapsed += Time.deltaTime;

                if (!attackDirFlg)
                {
                    transform.position = Vector3.Lerp(attackStartPos, attackFinishPos, attackElapsed);
                    if (transform.position == attackFinishPos)
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

        if (isTarget)
        {
            if (myState == State.Idle || myState == State.Walk)
            {
                // プレイヤーの方を向く
                gameObject.transform.LookAt(targetPlayer.transform.position);
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
                myState = State.Idle;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (myState == State.Attack && collision.gameObject.tag == "Player")
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
        return new Vector3(setPos.x, transform.position.y, setPos.z);
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
