using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region コンポーネントのアタッチ
[RequireComponent(typeof(Rigidbody))]
#endregion

public class WeaponBase : MonoBehaviour
{
    #region protected変数宣言
    protected GameObject player;    // プレイヤーオブジェクト
    protected GameObject playercamera;    // プレイヤーのカメラ
    #endregion
   

    // 拾われたときの処理
    public virtual void Take(GameObject player, GameObject camera) 
    {
        this.player = player;
        playercamera = camera;
    }

    // 武器を手放すときの処理
    public virtual void release()
    {
        this.player = null;
        playercamera = null;
    }

}
