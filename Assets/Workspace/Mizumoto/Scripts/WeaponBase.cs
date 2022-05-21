using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    protected GameObject player;    // プレイヤーオブジェクト
    protected GameObject playercamera;    // プレイヤーのカメラ
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // 拾われたときの処理
    public void Take(GameObject player, GameObject camera)
    {
        this.player = player;
        playercamera = camera;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
