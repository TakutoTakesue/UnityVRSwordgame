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
    [Header("�T�C�h�X�e�b�v�̃C���^�[�o�� : s")]
    [SerializeField] private float StepIntervalTime;
    private float stepInterval;

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
        stepInterval = StepIntervalTime;
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

                // �U���̃C���^�[�o�������炷
                if (attackInterval > 0)
                {
                    attackInterval -= Time.deltaTime;
                }

                // �������ꂽ��ǂ������鏈��
                if (attackRange + 0.5f < Vector3.Distance(transform.position, player.transform.position))
                {
                    myAnim.SetBool("StepFlg", false);
                    myState = State.Walk;
                    break;
                }

                // �v���C���[�Ƃ̋������߂�����Ƃ��ɉ�������
                if (attackRange > Vector3.Distance(transform.position, player.transform.position) && attackInterval > 0)
                {
                    myRB.velocity = (transform.position - player.transform.position).normalized * mySpeed / 2;
                    myAnim.SetBool("StepFlg", false);
                    myAnim.SetFloat("Speed", -mySpeed);
                }
                else if(attackRange <= Vector3.Distance(transform.position, player.transform.position))
                {
                    // �T�C�h�X�e�b�v�؂�ւ�����
                    if (stepInterval > 0)
                    {
                        stepInterval -= Time.deltaTime;
                        if (stepInterval <= 0)
                        {
                            stepInterval = StepIntervalTime;
                            if(myAnim.GetFloat("StepVector") >= 0.5f)
                            {
                                myAnim.SetFloat("StepVector", 0.0f);
                            }
                            else
                            {
                                myAnim.SetFloat("StepVector", 1.0f);
                            }
                        }
                    }

                    if (myAnim.GetFloat("StepVector") >= 0.5f)
                    {
                        transform.RotateAround(player.transform.position, Vector3.up, 20 * Time.deltaTime);
                    }
                    else
                    {
                        transform.RotateAround(player.transform.position, Vector3.up, -20 * Time.deltaTime);
                    }

                    // �U���Ɉڂ鏈��
                    if (attackInterval <= 0)
                    {
                        myAnim.SetFloat("Speed", 0);
                        myState = State.Attack;
                        myAnim.SetTrigger("Attack");  
                        myAnim.SetBool("StepFlg", false);
                        break;
                    }
                    myAnim.SetFloat("Speed", 0);
                    myAnim.SetBool("StepFlg", true);
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

    public void AttackFinish()
    {
        attackInterval = AttackIntervalTime;
        myState = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
