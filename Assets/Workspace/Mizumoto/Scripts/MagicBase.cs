using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 魔法のベース
public class MagicBase : MonoBehaviour
{
    #region enum宣言
    // 呪文の種類
    public enum MagicKinds
    {
        flame,  // 炎の呪文
        recovery,   // 回復
        thunder // サンダー
    }
    #endregion
    #region ストラクト変数
    // 呪文構造体
    public struct MagicInformation
    {
        MagicKinds preparationMagic;
        float interval;
    }

    MagicKinds selectionMagic;    // 今構えている呪文
    #endregion
    #region シリアライズ変数
    [SerializeField, Header("装備している呪文")]
    MagicInformation[] magic;
    [SerializeField, Header("範囲攻撃かどうか")]
    bool rangeAttack = false;
    [SerializeField, Header("魔法を飛ばすスピード")]
    float speed = 1.0f;
    [SerializeField, Header("呪文の基")]
    GameObject magicObj;
    #endregion
    #region protected変数
    protected int maxMagic = 0;   // 今装備している呪文の最大数
    protected int nowMagic = 0;   // 構えている呪文
    protected int somebody = 0;   // 撃ったのは誰なのか
    #endregion

    /// <summary>
    /// 魔法を直線的に放つ
    /// <paramref name="target"/> 呪文をどのオブジェクトに飛ばすか
    /// </summary>
    void MagicShot(GameObject target) {
       var magicobj = Instantiate(magicObj, transform.position, Quaternion.identity);
        // 呪文をホーミングさせる
        magicobj.AddComponent<HomingAction>();  
    }

    // 直接相手の位置に生みだす
    void DirectlyMagic(GameObject target) {
        // 呪文を直接ターゲットに出す
        var magicobj = Instantiate(magicObj, target.transform.position, Quaternion.identity);
       
    }
}
