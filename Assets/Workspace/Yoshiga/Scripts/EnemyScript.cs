using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("“G‚ÌHP : int")]
    [SerializeField] protected int HP;
    [Header("“G‚ÌˆÚ“®‘¬“x : float")]
    [SerializeField] protected float mySpeed;
    [Header("UŒ‚—Í : int")]
    [SerializeField] protected int power;
    [Header("Debug—p : ƒ^[ƒQƒbƒg‚É‚³‚ê‚Ä‚¢‚é‚©")]
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
