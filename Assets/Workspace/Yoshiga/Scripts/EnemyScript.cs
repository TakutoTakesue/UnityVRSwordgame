using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("�G��HP : int")]
    [SerializeField] protected int HP;
    [Header("�G�̈ړ����x : float")]
    [SerializeField] protected float mySpeed;

    // Start is called before the first frame update
    void Start()
    {
        
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
