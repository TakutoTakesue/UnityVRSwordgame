using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordAction : MonoBehaviour
{
    private GameObject player;
    private PlayerScript playerScript;
    private Rigidbody myRB;
    private CapsuleCollider myCollision;
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
    [Header("�v���C���[�ƕۂ��� : m")]
    [SerializeField] private float AttackRange;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
        Debug.Log(playerScript.walkspeed);
        myRB = GetComponent<Rigidbody>();
        myCollision = GetComponent<CapsuleCollider>();
        Instantiate(entryEffect,new Vector3(transform.position.x,
                                            transform.position.y + enemyYScale + 0.1f,
                                            transform.position.z),
                                Quaternion.Euler(90,0,0));
        entrySpeed = enemyYScale * (Time.deltaTime / 3);
    }

    private void FixedUpdate()
    {
        switch (myState)
        {
            case State.Entry:
                if(transform.position.y < 0)
                {
                    transform.position += new Vector3(0, entrySpeed, 0);
                    if(transform.position.y > 0)
                    {
                        myState = State.Idle;
                        myRB.useGravity = true;
                        myCollision.enabled = true;
                    }
                }
                break;
            case State.Idle:
                
                if(AttackRange > Vector3.Distance(transform.position,player.transform.position))
                {
                    myRB.velocity = new Vector3(transform.position.x - player.transform.position.x,
                                                transform.position.y - player.transform.position.y,
                                                transform.position.z - player.transform.position.z).normalized * playerScript.walkspeed;
                }
                break;
        }

        if(myState == State.Idle || myState == State.Walk)
        {
            // �v���C���[�̕��Ɍ���
            gameObject.transform.LookAt(player.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
