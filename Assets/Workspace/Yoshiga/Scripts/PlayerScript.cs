using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float moveChangeInterval;
    private float WalkSpeed;
    public float walkspeed => WalkSpeed;
    [Header("���E�ړ��؂�ւ��܂ł̕b�� : s")]
    [SerializeField]
    private float MoveChangeInterval;
    [Header("�v���C���[�̃X�s�[�h")]
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
