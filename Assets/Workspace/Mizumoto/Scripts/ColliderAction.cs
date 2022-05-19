using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderAction : MonoBehaviour
{

    [SerializeField,Header("プレイヤーコントローラー")] 
    GameObject controller;
    [SerializeField, Header("プレイヤーの持っている手 true 右 : false 左")]
    bool haveHand = true;
    bool weaponHaveFlg = false; // 物をつかんでいるかどうか
    
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR

        //PC動作なら、コライダーを1m前方に
        //transform.localPosition += controller.transform.forward * 0.3f;



#else
        //VR動作なら、ボールの視覚化をオフに
        GetComponentInChildren<MeshRenderer>().enabled = false;
        
#endif
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "WeaponHave")
        {
            var script = other.gameObject.GetComponent<WeaponHaveAction>();
            if (!script)
            {
                Debug.LogWarning("武器にWeaponScriptがついていません");
                return;
            }
            script.ChangeColor(Color.white);
        }
        //if (other.gameObject.tag == "Weapon")
        //{
        //    //アイテムの親を無しにする
        //    var script = other.gameObject.GetComponent<WeaponHaveAction>();
        //    // スクリプトを持っていないなら動かさない
        //    if (!script)
        //    {
        //        Debug.LogWarning("武器にWeaponScriptがついていません");
        //        return;
        //    }
        //    //この手がアイテムから出た瞬間
        //    script.Release();
        //    //アイテムに白になる指示を出す
        //    script.ChangeColor(Color.white);
        //    //アイテムに掴んでいない指示falseを出す
        //    script.Grabed(false);
        //}
    }

    void OnTriggerStay(Collider other)
    {

        //この手がアイテムに侵入中なら
        if (other.gameObject.tag == "WeaponHave")
        {
            var script = other.gameObject.GetComponent<WeaponHaveAction>();
            if (!script)
            {
                Debug.LogWarning("武器にWeaponScriptがついていません");
                return;
            }
            //アイテムに緑になる指示を出す
            script.ChangeColor(Color.green);
            //右人差し指かスペースキー押下中なら
            if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) ||
            Input.GetKey(KeyCode.Space))
            {
                script.GetWeapon(controller.transform, haveHand);
                //アイテムに掴んでいる指示trueを出し続ける
                script.Grabed(true);
                weaponHaveFlg = true;
            }
            ////右人差し指かスペースキーが離れた瞬間
            //if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger) ||
            //Input.GetKeyUp(KeyCode.Space))
            //{
            //    //アイテムの親を無しにする
            //    script.Release();
            //    //アイテムに掴んでいない指示falseを出す
            //    script.Grabed(false);
            //}
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
    
