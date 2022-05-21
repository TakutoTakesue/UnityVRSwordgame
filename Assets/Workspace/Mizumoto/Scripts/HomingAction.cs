using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region コンポーネントのアタッチ
[RequireComponent(typeof(Rigidbody))]
#endregion

public class HomingAction : MonoBehaviour
{
    #region Serialize変数宣言
    [SerializeField, Header("目標オブジェクト")]
    GameObject target;
    [SerializeField, Header("スピード")]
    float speed = 2;
    [SerializeField, Header("ホーミングを切る距離")]
    float stopDistance = 2;
    #endregion
    #region プライベート変数
    Rigidbody myRb;  // 自身のリジッドボディ;
    #endregion


    // Start is called before the first frame update
    private void Start()
    {
        myRb = GetComponent<Rigidbody>();   // 取得
    }


    private void FixedUpdate()
    {
        #region ホーミングするかチェック
        if (!target) {
            return;
        }
        #endregion

        #region ホーミング処理
        // まっすぐ飛ばす
        var movepow = transform.forward * speed;
        // ターゲットに向かって飛ばす
        var difference = target.transform.position - transform.position;
        myRb.velocity = difference.normalized * speed;
        #endregion

        #region ホーミングを切る処理
        // ある程度まで近づいた場合ホーミング要素をキル
        if (Vector3.Distance(target.transform.position, transform.position) <= stopDistance) {
            target = null;
        }
        #endregion
    }

}
