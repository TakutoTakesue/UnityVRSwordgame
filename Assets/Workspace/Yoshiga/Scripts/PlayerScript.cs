using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float moveChangeInterval;
    private float WalkSpeed;
    public float walkspeed => WalkSpeed;
    [Header("左右移動切り替えまでの秒数 : s")]
    [SerializeField]
    private float MoveChangeInterval;
    [Header("プレイヤーのスピード")]
    [SerializeField]
    private float Speed;
    private Rigidbody myRB;

    // Start is called before the first frame update
    void Start()
    {
        moveChangeInterval = MoveChangeInterval;
        myRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(moveChangeInterval > 0)
        {
            moveChangeInterval -= Time.deltaTime;
            if(moveChangeInterval <= 0)
            {
                moveChangeInterval = MoveChangeInterval;
                Speed = -Speed;
                
            }
        }
        myRB.velocity = new Vector3(Speed, 0, 0);
    }
}
