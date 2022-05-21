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

    private State myState = State.Entry;    // �G�̃X�e�[�^�X
    [Header("���m�͈� : m")]
    [SerializeField] private float range;
    [Header("�o�ꎞ�̃G�t�F�N�g : Object")]
    [SerializeField] private GameObject entryEffect;
    [Header("�U���̃C���^�[�o�� : s")]
    [SerializeField] private float AttackIntervalTime;
    private float attackInterval;
    [Header("�r���̎��� : s")]
    [SerializeField] private float ChargeIntervalTime;
    private float chargeInterval;
    private bool isTarget;  // �v���C���[�������Ă��邩�̃t���O

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
                break;
            case State.Attack:

                // �r��
                if(chargeInterval > 0)
                {
                    chargeInterval -= Time.deltaTime;
                }
                break;
        }

        // �v���C���[�𔭌�
        if (!isTarget && range >= Vector3.Distance(transform.position, player.transform.position))
        {
            isTarget = true;
            myAnim.SetBool("Discover", true);
            myState = State.Idle;
        }

        if (isTarget)
        {
            if (myState == State.Idle && myState == State.Attack)
            {
                // �v���C���[�̕��Ɍ���
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
