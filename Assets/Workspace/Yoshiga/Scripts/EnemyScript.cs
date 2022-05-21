using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("�G��HP : int")]
    [SerializeField] protected int HP;
    [Header("�G�̈ړ����x : float")]
    [SerializeField] protected float mySpeed;
    [Header("�U���� : int")]
    [SerializeField] protected int power;
    [Header("Debug�p : �^�[�Q�b�g�ɂ���Ă��邩")]
    [SerializeField] protected bool battleFlg;
    // Start is called before the first frame update
    void Start()
    {
        battleFlg = false;
    }

    public void OnDamage(int num)
    {
        HP -= num;
    }

    public void ChangeBattleFlg()
    {
        battleFlg = !battleFlg;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
