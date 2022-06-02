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

    private State myState = State.Entry;    // �G�̃X�e�[�^�X
    [Header("����p : (0�`180)�x")]
    [SerializeField] private float searchAngle;
    [Header("���m�͈� : m")]
    [SerializeField] private float serchRange;
    [Header("�o�ꎞ�̃G�t�F�N�g : Object")]
    [SerializeField] private GameObject entryEffect;
    [Header("�r���G�t�F�N�g : Object")]
    [SerializeField] private GameObject chargeEffect;
    [Header("�U���̃C���^�[�o�� : s")]
    [SerializeField] private float AttackIntervalTime;
    private float attackInterval;
    [Header("�r���̎��� : s")]
    [SerializeField] private float ChargeIntervalTime;
    [Header("�����@ : Object")]
    [SerializeField] private GameObject fireBall;
    [Header("���@�̑��x : m/s")]
    [SerializeField] private float fireBallSpeed;
    private GameObject fireMagic;
    private ParticleSystem fireMagicPS;
    private GameObject magicPos; // ���@�̏o��ʒu
    private float fireBallSize; // �����@�̑傫��
    private float chargeInterval;
    private bool isTarget;  // �v���C���[�������Ă��邩�̃t���O

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

                // �r��
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
                // �v���C���[�̕��Ɍ���
                gameObject.transform.LookAt(new Vector3(targetPlayer.transform.position.x,transform.position.y, targetPlayer.transform.position.z));
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
