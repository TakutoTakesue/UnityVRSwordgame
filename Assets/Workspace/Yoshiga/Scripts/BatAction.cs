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

    private State myState = State.Idle;    // �G�̃X�e�[�^�X
    [Header("����p : (0�`180)�x")]
    [SerializeField] private float searchAngle;
    [Header("���m�͈� : m")]
    [SerializeField] private float serchRange;
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
    private Vector3 attackStartPos;  // �U�����d�|���鎞�ɂ����ʒu
    private Vector3 attackFinishPos;�@// �U�����d�|����ʒu
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

                    // �������ꂽ��ǂ������鏈��
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
                            // �ړI�n������
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
                    // �ړI�n�ɓ���
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
                // �v���C���[�̕�������
                gameObject.transform.LookAt(targetPlayer.transform.position);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && targetPlayer == null)
        {
            //�@�v���C���[�̕���
            Vector3 playerDirection = other.transform.position - transform.position;
            //�@�G�̑O������̃v���C���[�̕���
            float angle = Vector3.Angle(transform.forward, playerDirection);
            //�@�T�[�`����p�x����������v���C���[����
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

    // �ړI�n�������_���ŕԂ�����
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
    //�@�T�[�`����p�x�\��
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
