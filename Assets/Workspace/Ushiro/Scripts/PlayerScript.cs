using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Math = System.Math;
public class PlayerScript : MonoBehaviour
{
    //�o�b��Ɨp�̕ϐ��Q
    Vector3 Rot = Vector3.zero;
    [SerializeField]
    Vector2 Bias = new Vector2(5.0f, 5.0f); //��]���x
    [SerializeField]
     float maxPitch = 60.0f; //�p����
    [SerializeField]
     float minPitch = -60.0f; //��p����
    [SerializeField]
     Camera MyCamera;

    float ThirdParsonDistance;
    const float StunDistance = 3.0f;
     float StunTime = 0.25f;
    bool StunFlg;
    float Elapced;
    Vector3 InputMove;         //�X�e�B�b�N����
    [SerializeField]
     float WalkSpeed = 4.0f; //�������x
    public float walkspeed => WalkSpeed;
    Animator Myanim;

    void Start()
    {
        Elapced = 0;
        Myanim = GetComponent<Animator>();
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

        InputMove.x = Input.GetAxis("Horizontal");
        InputMove.z = Input.GetAxis("Vertical");

    }

    //�X�}�[�g�@��̉�]�n��Unity�ł̉�]�n�ɕϊ�����֐�
    Quaternion GyroToUnity(Quaternion q)
    {
        //�E��n�ƍ���n���قȂ�̂Ń}�C�i�X����
        //����I�ȈႢ�͂��Ƃ�������ւ�邱��
        //�X�}�[�g�@��͕��u���W���AUnity�͒����u���Ő������Ȃ̂�90��]
        return new Quaternion(-q.x, -q.z, -q.y, q.w) * Quaternion.Euler(90f, 0f, 0f);
    }
    private void FixedUpdate()

    {
        if (StunFlg)
        {
            Elapced += Time.fixedDeltaTime;

            if (Elapced<0.25)
            {
                ThirdParsonDistance = StunDistance * Elapced / 0.25f;
            }
            else if (Elapced>StunTime-0.25)
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
            //�i�ޕ����v�Z
            //X-Z���ʃx�N�g�����擾
            Vector3 axisDirV = Vector3.Scale(MyCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 axisDirH = Vector3.Scale(MyCamera.transform.right, new Vector3(1, 0, 1)).normalized;
            Vector3 dir = Vector3.zero;


            Vector3 absInput = new Vector3(Math.Abs(InputMove.x), 0, Math.Abs(InputMove.z));
            Vector3 Speed = Vector3.zero;


            if (absInput.x > 0.1 || absInput.z > 0.1)
            {
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {

                    float a = (absInput.x > absInput.z) ? absInput.x : absInput.z;            //x��z�ǂ��������������f
                    Speed.x = a * a * InputMove.x / Mathf.Sqrt(InputMove.x * InputMove.x + InputMove.z * InputMove.z);
                    Speed.z = a * a * InputMove.z / Mathf.Sqrt(InputMove.x * InputMove.x + InputMove.z * InputMove.z);
                }
                else
                {
                    Speed.x = InputMove.x;
                    Speed.z = InputMove.z;

                }
                Speed = axisDirV * Speed.z + axisDirH * Speed.x;
                dir = Speed * WalkSpeed;
                transform.position += dir * Time.fixedDeltaTime;
            }



        }
        if (Application.platform == RuntimePlatform.Android ||
        Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Vector3 campos = transform.position;
            //Input�X�g���[���̒���gyro.attitude�́A���f�[�^�̂܂܂ł͍��W�n���قȂ�
            //Unity������Quaternion�ɕϊ����A���ۂɉ�]����
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
            //�o�b�ł̓}�E�X�ŃG�~�����[�g����

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
            MyCamera.transform.position = campos;

        }

        if (StunFlg)
        {

        }







    } 
}
