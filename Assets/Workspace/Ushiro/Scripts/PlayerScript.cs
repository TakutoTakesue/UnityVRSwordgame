using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Math = System.Math;


public class PlayerScript : MonoBehaviour
{
    //ＰＣ作業用の変数群
    Vector3 Rot = Vector3.zero;
    [SerializeField]
    Vector2 Bias = new Vector2(5.0f, 5.0f); //回転速度
    [SerializeField]
    float maxPitch = 60.0f; //仰角制限
    [SerializeField]
    float minPitch = -60.0f; //俯角制限
    [SerializeField]
    Camera MyCamera;

    [SerializeField]
    GameObject LookPos;     //キャラの注視点
    float ThirdParsonDistance;
    const float StunDistance = 3.0f;
    float StunTime = 0.25f;
    bool StunFlg;
    float Elapced;
    Vector3 InputMove;         //スティック入力
    [SerializeField]
    float WalkSpeed = 4.0f; //歩く速度
    public float walkspeed => WalkSpeed;
    Animator Myanim;

    [SerializeField]
    GameObject MenuUI;

    [SerializeField]
    GameObject MenuPos;     //メニューを開くときの場所

    void Start()
    {
        Elapced = 0;
        Myanim = GetComponent<Animator>();
        MenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!StunFlg)
            {
                StunFlg = true;
                Myanim.SetTrigger("Stun");
                Myanim.Update(0f);
                StunTime = Myanim.GetCurrentAnimatorStateInfo(0).length;
            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            MenuUI.SetActive(true);
            MenuScript menu = MenuUI.GetComponent<MenuScript>();
            if (menu != null)
            {
                menu.OpenMenu(MenuPos.transform.position);
            }

        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            MenuScript menu = MenuUI.GetComponent<MenuScript>();
            if (menu != null)
            {
                menu.CloseMenu();
            }

        }

        InputMove.x = Input.GetAxis("Horizontal");
        InputMove.z = Input.GetAxis("Vertical");

    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (!StunFlg)
        {
            //顔をカメラの向きと連動させる
            Myanim.SetLookAtWeight(1.0f, 0.0f, 1.0f, 0.0f, 0.5f);
            Myanim.SetLookAtPosition(LookPos.transform.position);
        }

    }
    //スマート機器の回転系をUnityでの回転系に変換する関数
    Quaternion GyroToUnity(Quaternion q)
    {
        //右手系と左手系が異なるのでマイナス符号
        //決定的な違いはｙとｚが入れ替わること
        //スマート機器は平置き標準、Unityは直立置きで水平線なので90回転
        return new Quaternion(-q.x, -q.z, -q.y, q.w) * Quaternion.Euler(90f, 0f, 0f);
    }
    private void FixedUpdate()

    {
        if (StunFlg)
        {

            //スタン中のカメラの移動
            Elapced += Time.fixedDeltaTime;

            if (Elapced < 0.25)
            {
                ThirdParsonDistance = StunDistance * Elapced / 0.25f;
            }
            else if (Elapced > StunTime - 0.25)
            {
                ThirdParsonDistance = StunDistance * (StunTime - Elapced) / 0.25f;
            }
            else
            {
                ThirdParsonDistance = StunDistance;
            }
            if (Elapced > StunTime)
            {
                StunFlg = false;
                Elapced = 0;
                {
                    transform.localEulerAngles = new Vector3(0, MyCamera.transform.localEulerAngles.y + transform.localEulerAngles.y, 0);
                    MyCamera.transform.localEulerAngles = new Vector3(MyCamera.transform.localEulerAngles.x, 0, 0);
                }
            }
        }
        else
        {
            //進む方向計算
            //X-Z平面ベクトルを取得
            Vector3 axisDirV = Vector3.Scale(MyCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 axisDirH = Vector3.Scale(MyCamera.transform.right, new Vector3(1, 0, 1)).normalized;
            Vector3 dir = Vector3.zero;


            Vector3 absInput = new Vector3(Math.Abs(InputMove.x), 0, Math.Abs(InputMove.z));
            Vector3 Speed = Vector3.zero;


            if (absInput.x > 0.1 || absInput.z > 0.1)
            {
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    //キーマウでの操作(デバッグ用)

                    float a = (absInput.x > absInput.z) ? absInput.x : absInput.z;            //xとzどっちが長いか判断
                    Speed.x = a * a * InputMove.x / Mathf.Sqrt(InputMove.x * InputMove.x + InputMove.z * InputMove.z);
                    Speed.z = a * a * InputMove.z / Mathf.Sqrt(InputMove.x * InputMove.x + InputMove.z * InputMove.z);
                }
                else
                {
                    Speed.x = InputMove.x;
                    Speed.z = InputMove.z;

                }
                //移動方向を算出
                Speed = axisDirV * Speed.z + axisDirH * Speed.x;
                dir = Speed * WalkSpeed;
                transform.position += dir * Time.fixedDeltaTime;
            }



        }
        if (Application.platform == RuntimePlatform.Android ||
        Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Vector3 campos = transform.position;
            //Inputストリームの中のgyro.attitudeは、生データのままでは座標系が異なる
            //Unity向けのQuaternionに変換し、実際に回転する
            Rot = GyroToUnity(Input.gyro.attitude).eulerAngles;

            MyCamera.transform.localEulerAngles = new Vector3(-Rot.y, 0, 0);
            transform.localEulerAngles = new Vector3(0, Rot.x, 0);

            Vector3 axisDirV = Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized;
            axisDirV = axisDirV * ThirdParsonDistance;
            campos -= axisDirV;
            MyCamera.transform.position = campos;

        }
        else
        {
            Vector3 campos = transform.position;
            //ＰＣではマウスでエミュレートする

            if (StunFlg)
            {
                Rot.x = MyCamera.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * Bias.x;
                Rot.y += Input.GetAxis("Mouse Y") * Bias.y;
                Rot.y = Mathf.Clamp(Rot.y, minPitch, maxPitch);
                MyCamera.transform.localEulerAngles = new Vector3(-Rot.y, Rot.x, 0);
            }
            else
            {
                
                Rot.x = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * Bias.x;
                Rot.y += Input.GetAxis("Mouse Y") * Bias.y;
                Rot.y = Mathf.Clamp(Rot.y, minPitch, maxPitch);
                MyCamera.transform.localEulerAngles = new Vector3(-Rot.y, 0, 0);
                transform.localEulerAngles = new Vector3(0, Rot.x, 0);
            }
            Vector3 axisDirV = Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized;
            axisDirV = axisDirV * ((ThirdParsonDistance > StunDistance) ? StunDistance : ThirdParsonDistance);
            axisDirV.y = -1;
            campos -= axisDirV;
            campos.y += 0.4f;
            MyCamera.transform.position = campos;

        }

        if (StunFlg)
        {

        }


    }
    public void OnDamage(int damage)
    {

    }
}
