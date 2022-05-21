using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAction : MonoBehaviour
{
    #region シリアライズ変数
    [SerializeField,Header("体力の最大値")]
    protected int hpMax;
    protected int hp;   // 現在の体力
    public int HP => hp;    // ゲッター

    [SerializeField, Header("攻撃力")]
    protected int atk;
    public int ATK => atk;  // ゲッター
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        hp = hpMax; // hpの初期化
    }

    // ダメージを受ける関数
    // @param atk 攻撃力
    public void OnDamage(int atk) {
        atk = Mathf.Max(atk, 0);    // 攻撃力が0以下の場合は0にする
        hp = Mathf.Max(hp - atk, 0);    // hpが0いかになる場合は0にする
    }

    // 回復関数
    // @param resilience 回復力
    public void Recovery(int resilience) {
        resilience = Mathf.Max(resilience, 0);    // 回復力が0以下の場合は0にする ダメージを受ける可能性があるので
        hp = Mathf.Min(hp + resilience, hpMax);    // hpが以下かになる場合は0にする
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
