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

    private State myState = State.Idle;    // �G�̃X�e�[�^�X
    [Header("���m�͈� : m")]
    [SerializeField] private float range;
    [Header("�ړ��͈͂̔��a : m")]
    [SerializeField] private float moveRadius;
    [Header("Idle��Ԃł��̏�ɂƂǂ܂鎞�� : s")]
    [SerializeField] private float idleIntervalTime;
    private float idleInterval;
    private Vector3 destination; // �ړI�n
    private bool isTarget;  // �v���C���[�������Ă��邩�̃t���O
    [Header("�U���͈� : m")]
    [SerializeField] private float attackRange;
    [Header("�U���̃C���^�[�o�� : s")]
    [SerializeField] private float AttackIntervalTime;
    private float attackInterval;
    private Vector3 attackStartPos;  // �U�����n�߂��ꏊ
    private Vector3 attackFinishPos; // �U�����鎞�Ɍ������ꏊ
    private float attackDistance; // �U���������̓G�ƃv���C���[�̋���
    private bool attackDirFlg = false;
    private float attackElapsed; // �U���̌o�ߎ���

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

                    // �v���C���[�Ƃ̋���������Ă���ƃv���C���[�Ɍ������Ă�������
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
                #region �ړ�
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
                        // �ړI�n�ɓ���
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

        // �v���C���[�𔭌�
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
                // �v���C���[�̕��Ɍ���
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
            // �ړI�n������
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

    // �ړI�n�������_���ŕԂ�����
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
