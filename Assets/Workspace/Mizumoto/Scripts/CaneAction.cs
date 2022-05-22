using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaneAction : WeaponBase
{
    #region シリアライズ変数
    [SerializeField, Header("ロックオンのピン")]
    GameObject LockOnCursor;
    #endregion
    #region プライベート変数
    Transform yeyPos = null; // 目の位置
    IEnumerator weaponPick;    // コール―チン変数
    Magic magic;
    #endregion



    // 取得されているときだけ回るコール―チン
    IEnumerator WeaponPick() {
        // 1 目線からRayを飛ばす
        RaycastHit hit;
        // 目線からRayを飛ばす
        if (Physics.Raycast(yeyPos.position, yeyPos.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, yeyPos.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Hit");
        }
        else
        {
            Debug.DrawRay(yeyPos.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
        // 2 もしRayにオブジェクト当たっていたらそれがターゲットがとれるオブジェクトか判別

        // 3 もしターゲットが取れるオブジェクトの場合はターゲットUIを表示する


        yield return new WaitForSeconds(0.1f);

    }

    // 杖を取得する処理
    public override void Take(GameObject player, GameObject camera) 
    {
        // 変数に代入
        this.player = player;
        playercamera = camera;
        yeyPos = camera.transform;
        weaponPick = WeaponPick();
        StartCoroutine(weaponPick);
    }

    // 杖を手放す処理
    public override void release()
    {
        this.player = null;
        playercamera = null;
        yeyPos = null;
        StopCoroutine(weaponPick);
    }

}
