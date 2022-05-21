using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("敵のHP : int")]
    [SerializeField] protected int HP;
    [Header("敵の移動速度 : float")]
    [SerializeField] protected float mySpeed;
    [Header("敵の攻撃力 : int")]
    [SerializeField] protected int power;
    [Header("ターゲットにされているか")]
    [SerializeField] protected bool battleFlg;
    // Start is called before the first frame update
    void Start()
    {
        battleFlg = false;
    }

    protected void OnDamage(int num)
    {
        HP -= num;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
