using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyScript : MonoBehaviour
{
    protected List<GameObject> player = new List<GameObject>();
    protected GameObject nearPlayer; // 一番近いプレイヤー
    protected GameObject targetPlayer; // 現在標的にしているプレイヤーを格納するための変数
    [Header("敵のHP : int")]
    [SerializeField] protected int HP;
    [Header("敵の移動速度 : float")]
    [SerializeField] protected float mySpeed;
    [Header("攻撃力 : int")]
    [SerializeField] protected int power;
    [Header("Debug用 : ターゲットにされているか")]
    [SerializeField] protected bool battleFlg;
    // Start is called before the first frame update
    protected void Start()
    {
        // プレイヤーを全員Listに追加
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var i in players)
        {
            player.Add(i);
        }
        battleFlg = false;
    }

    // 誰が一番近いか判定するための処理
    protected void FindTarget()
    {
        float distance = 0.0f;
        float saveDistace = 100.0f;
        foreach (var i in player)
        {
            distance = Vector3.Distance(this.gameObject.transform.position, i.transform.position);
            if (saveDistace > distance)
            {
                saveDistace = distance;
                nearPlayer = i;
            }
        }
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
