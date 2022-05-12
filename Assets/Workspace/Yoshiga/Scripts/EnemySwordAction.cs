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

    private State myState = State.Entry;    // 敵のステータス
    [Header("敵の身長 : m")]
    [SerializeField] private float enemyYScale;
    private float entrySpeed;   // 敵が登場する時の地面から出てくる速さ
    [Header("登場時の魔法陣 : パーティクルオブジェクト")]
    [SerializeField] private GameObject entryEffect;
    [Header("プレイヤーと保つ距離 : m")]
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
            // プレイヤーの方に向く
            gameObject.transform.LookAt(player.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
