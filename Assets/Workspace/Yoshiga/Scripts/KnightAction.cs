using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAction : EnemyScript
{
    private GameObject player;
    private Rigidbody myRB;
    private CapsuleCollider myCollision;
    private Animator myAnim;
    public enum State
    {
        Entry,
        Idle,
        Walk,
        Attack,
        Defense,
        Hit,
        Wince,
        Death,
    }

    private State myState = State.Entry;    // �G�̃X�e�[�^�X
    [Header("�G�̐g�� : m")]
    [SerializeField] private float enemyYScale;
    private float entrySpeed;   // �G���o�ꂷ�鎞�̒n�ʂ���o�Ă��鑬��
    [Header("�o�ꎞ�̖��@�w : �p�[�e�B�N���I�u�W�F�N�g")]
    [SerializeField] private GameObject entryEffect;
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
        myCollision = GetComponent<CapsuleCollider>();
        myAnim = GetComponent<Animator>();
        // �o�ꎞ�̖��@�w�̐���
        Instantiate(entryEffect, new Vector3(transform.position.x,
                                            transform.position.y + enemyYScale + 0.1f,
                                            transform.position.z), Quaternion.Euler(90, 0, 0));
        entrySpeed = enemyYScale * (Time.deltaTime / 3);
        attackInterval = AttackIntervalTime;
    }

    private void FixedUpdate()
    {
        switch (myState)
        {
            case State.Entry:

                // �o�ꎞ�ɒn�ʂ���o�Ă��鏈��
                if (transform.position.y < 0)
                {
                    transform.position += new Vector3(0, entrySpeed, 0);
                    if (transform.position.y > 0)
                    {
                        myState = State.Walk;
                        myRB.useGravity = true;
                        myCollision.enabled = true;
                    }
                }
                break;
            case State.Idle:

                // �v���C���[�Ƃ̋������߂�����Ƃ��ɉ�������
                if (attackRange > Vector3.Distance(transform.position, player.transform.position) && attackInterval > 0)
                {
                    myRB.velocity = (transform.position - player.transform.position).normalized * mySpeed / 2;
                    myAnim.SetFloat("Speed", -mySpeed);
                }
                else
                {
                    if (attackInterval <= 0)
                    {
                        myState = State.Attack;
                        myAnim.SetTrigger("Attack");
                    }
                    myAnim.SetFloat("Speed", 0);
                }
                break;
            case State.Walk:

                // �v���C���[�Ƃ̋���������Ă���ƃv���C���[�Ɍ������ĕ�������
                if(attackRange <= Vector3.Distance(transform.position, player.transform.position))
                {
                    myRB.velocity = (player.transform.position - transform.position).normalized * mySpeed;
                    myAnim.SetFloat("Speed", mySpeed);
                }
                else
                {
                    myRB.velocity = Vector3.zero;
                    myState = State.Idle;
                }
                break;
        }

        if (myState == State.Idle || myState == State.Walk)
        {
            // �v���C���[�̕��Ɍ���
            gameObject.transform.LookAt(player.transform.position);
        }
    }

    void AttackFinish()
    {
        attackInterval = AttackIntervalTime;
        myState = State.Idle;
        Debug.Log("aaaaa");
    }

    // Update is called once per frame
    void Update()
    {
        // �U���̃C���^�[�o�������炷
        if(attackInterval > 0)
        {
            attackInterval -= Time.deltaTime;
        }
    }
}
