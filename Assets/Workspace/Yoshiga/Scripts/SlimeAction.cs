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

                if(isTarget)
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
            // �v���C���[�̕��Ɍ���
            gameObject.transform.LookAt(player.transform.position);
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
