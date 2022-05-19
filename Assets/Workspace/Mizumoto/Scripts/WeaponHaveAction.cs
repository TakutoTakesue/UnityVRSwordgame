using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHaveAction : MonoBehaviour
{
    #region インスペクターで変えれる変数宣言
    [SerializeField, Header("プレイヤーが武器を持った時の補正位置")]
    Vector3 correctionPosition;
    [SerializeField, Header("プレイヤーが武器を持った時の補正角度")]
    Vector3 correctionRotation;
    [SerializeField, Header("逆手もちするために必要な角度")]
    float reverseMoveAngle = 120;
    [SerializeField, Header("逆手もちが出きる武器かどうか")]
    bool reverseMove = false;
    #endregion

    #region プライベート変数宣言
    Renderer myRenderer; //自身のレンダラー
    Rigidbody myRigidbody; //自身の物理挙動
    GameObject Weapon;  // 自身が持っている武器
    bool handRight = true;  // 右手に持っているかどうか 
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent != null)
        {
            Weapon = transform.parent.gameObject;
            if (Weapon == null)
            {
                Debug.LogWarning("親の武器がありません");
                Destroy(gameObject);
                return;
            }
        }
        else {
            Debug.LogWarning("親がありません");
            Destroy(gameObject);
            return;
        }
        //自身のレンダラーを取得
        myRenderer = GetComponentInChildren<MeshRenderer>();
        //自身の物理挙動を取得
        myRigidbody = Weapon.GetComponent<Rigidbody>();
        ChangeColor(Color.white); //白に変色
        
    }

    //色変更処理
    public void ChangeColor(Color ColorName)
    {
        myRenderer.material.color = ColorName;
    }
    /*
     *  手が掴む処理
     * @param Mode（true : 掴む/false : 放す）
     */
    public void Grabed(bool Mode )
    {
        myRigidbody.isKinematic = Mode; //物理挙動の可否
        myRigidbody.useGravity = !Mode; //重力利用の可否
        if (Mode)
        {
            //Weapon.transform.rotation = transform.rotation;
            ChangeColor(Color.red); //赤に変色
        }
    }

    /* 
     *  ウェポン入手する
     *  @param transform コントローラーtransform
     *  @param handR (true : 右手/false : 左手)
     */
    public void GetWeapon(Transform transform, bool handR) {
        var a = transform.forward; // コントローラーの向いている方向
        var b = Weapon.transform.forward; // 持ちてのベクトル
        var signedAngle = Vector3.SignedAngle(a, b, Vector3.up);    // 二つのベクトルのなす角
        Vector3 rotation = handR ?  Vector3.zero : new Vector3(0, 180, 0);   // 右手で持っているなら何もしない左手なら180度回転
        Vector3 updatePosition = handR ? correctionPosition : -correctionPosition;
        Weapon.transform.parent = transform;
        // 逆手もちかどうかの処理
        // 角度がreverseMoveAngle以上の時は逆手もち
        if (Mathf.Abs(signedAngle) <= reverseMoveAngle || !reverseMove)
        {
            // 自身の手の角度とオブジェクトの向いているベクトルが90度以内なら普通にもつ
            Weapon.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + correctionRotation + rotation);
        }
        else
        {
            // 逆手もちなので回転させる
            rotation.x += 180.0f;
            // 逆手もち
            Weapon.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + correctionRotation + rotation);
        }
        
        Weapon.transform.position = transform.position + updatePosition;
    }

    // 武器を離す
    public void Release() {
        Weapon.transform.parent = null;
    }

    private void Update()
    {
       
    }

}
