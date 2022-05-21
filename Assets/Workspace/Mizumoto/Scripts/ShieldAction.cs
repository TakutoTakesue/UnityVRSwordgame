using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAction : WeaponBase
{
    [SerializeField, Header("攻撃のカット率")]
    float CutRate = 75.0f;
    [SerializeField, Header("攻撃をはじく処理")]
    bool parryFlg = false;
    bool ParryFlg => parryFlg;  // ゲット変数
    // PlayerAction playeraction
    // Start is called before the first frame update
    void Start()
    {
        // カット率を1から100の範囲で設定する
        CutRate = Mathf.Clamp(CutRate, 0, 100);
    }

    /*
     *  攻撃を受けた時にダメージを受ける関数
     *  @param atk 攻撃力
     */
    public void OnDamage(int atk) {
        // 作れていないのでコメントアウトでやりたいことだけでも書いておく
        // ダメージを受けた時ダメージをカットしてプレイヤーに伝えてあげる
        //if (playeraction) { 
        // playeraction.OnDamage(1 - cutRate/100 * atk)
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
