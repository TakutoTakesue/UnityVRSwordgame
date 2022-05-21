using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBase : WeaponBase
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
    public struct Magic {
        MagicKinds preparationMagic;
        float interval;
    }

    MagicKinds selectionMagic;    // 今構えている呪文
    #endregion

    [SerializeField, Header("装備している魔法")]
    Magic[] magic;


}
