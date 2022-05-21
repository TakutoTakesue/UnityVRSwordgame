using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    GameObject CameraPos;

    [SerializeField]
    GameObject LookPos;     //�L�����̒����_
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

    [SerializeField]
    GameObject MenuUI;

    [SerializeField]
    GameObject MenuPos;     //���j���[���J���Ƃ��̏ꏊ
    [SerializeField]
    GameObject RightHand;     //�E��
    [SerializeField]
    GameObject LeftHand;     //����



    [SerializeField]
    int StartLife;               //���g��HP�̏����l
    [SerializeField]
    Image LifeGageImage;               //���g��HP(�Q�[�W)
    [SerializeField]
    Image KasouLifeGageImage;              //���g�̉��zHP(�Q�[�W)
    int Life;               //���g��HP
    float KasouLife;               //���g�̉��zHP
    float KeepDamage;             //�Ō�Ɏ󂯂��_���[�W(�A���̏ꍇ���Z)
    float LifeDelay;               //HP�̕ϓ�
    float HPlessDelay;              //HP�Q�[�W������n�߂�܂ł̎��ԊǗ�
    bool deadflg;               //����ł��邩�ǂ���
    public Text LifeTex;
    void Start()
    {

        //�������

        RightHand.transform.position = new Vector3(0.3700722f, -0.5688783f, -0.1231707f);
        RightHand.transform.rotation = new Quaternion(107.09f, 573.226f, 510.86f, 0);
        LeftHand.transform.position = new Vector3(-0.3612857f, -0.5318148f, -0.1369106f);
        LeftHand.transform.rotation = new Quaternion(-255.33f, -375.268f, -375.268f, 0);

        //����������܂�




        Elapced = 0;
        Myanim = GetComponent<Animator>();
        MenuUI.SetActive(false);


        KasouLife = StartLife;
        Life = StartLife;
        LifeDelay = 0;
        deadflg = false;
    }

    void Update()
    {
        //�������




        if (Input.GetKey(KeyCode.G))
        {
            OnDamage(10);

        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            OnDamage(-1000);

        }



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
        //����������܂�
        InputMove.x = Input.GetAxis("Horizontal");
        InputMove.z = Input.GetAxis("Vertical");

    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (!StunFlg)
        {
            //����J�����̌����ƘA��������
            Myanim.SetLookAtWeight(1.0f, 0.0f, 1.0f, 0.0f, 0.5f);
            Myanim.SetLookAtPosition(LookPos.transform.position);



            //�@�E���IK�̃E�G�C�g�ݒ�
            Myanim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            Myanim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            //�@�����IK�̃E�G�C�g�ݒ�
            Myanim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            Myanim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            //�@�E��̈ʒu�ݒ�
            Myanim.SetIKPosition(AvatarIKGoal.RightHand, RightHand.transform.position);
            Myanim.SetIKRotation(AvatarIKGoal.RightHand, RightHand.transform.rotation);
            //�@����̈ʒu�ݒ�
            Myanim.SetIKPosition(AvatarIKGoal.LeftHand, LeftHand.transform.position);
            Myanim.SetIKRotation(AvatarIKGoal.LeftHand, LeftHand.transform.rotation);






        }

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
       // UnityEngine.Debug.Log(LifeDelay + "+" + KasouLife + "+" + KeepDamage);
        if (Life != KasouLife)
        {
            LifeDelay -= Time.fixedDeltaTime;
            if (LifeDelay < 0)
            {
                KasouLife -= KeepDamage * Time.fixedDeltaTime * 3;
                if (KasouLife < Life)
                {

                    KasouLife = Life;
                }
            }
        }

        LifeGageImage.fillAmount = (float)Life / StartLife;
     //   UnityEngine.Debug.Log(Life / StartLife);
        KasouLifeGageImage.fillAmount = (float)KasouLife / StartLife;
        LifeTex.text = "LIFE�F" + Life.ToString("d3");


        if (StunFlg)
        {

            //�X�^�����̃J�����̈ړ�
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
                    //�L�[�}�E�ł̑���(�f�o�b�O�p)

                    float a = (absInput.x > absInput.z) ? absInput.x : absInput.z;            //x��z�ǂ��������������f
                    Speed.x = a * a * InputMove.x / Mathf.Sqrt(InputMove.x * InputMove.x + InputMove.z * InputMove.z);
                    Speed.z = a * a * InputMove.z / Mathf.Sqrt(InputMove.x * InputMove.x + InputMove.z * InputMove.z);
                }
                else
                {
                    Speed.x = InputMove.x;
                    Speed.z = InputMove.z;

                }
                //�ړ��������Z�o
                Speed = axisDirV * Speed.z + axisDirH * Speed.x;
                dir = Speed * WalkSpeed;
                transform.position += dir * Time.fixedDeltaTime;
            }



        }
        if (Application.platform == RuntimePlatform.Android ||
        Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Vector3 campos =CameraPos.transform.position;
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
            Vector3 campos = CameraPos.transform.position;
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
            campos -= axisDirV;
            MyCamera.transform.position = campos;

        }

        if (StunFlg)
        {

        }


    }
    public void OnDamage(int damage)
    {

            if (LifeDelay < 0)
            {
                KasouLife = Life;
                KeepDamage = damage;
            }
            else
            {
                KeepDamage += damage;
            }
            LifeDelay = 0.5f;
        UnityEngine.Debug.Log(LifeDelay);
        Life -= damage;

            if (Life < 0)
            {
                Life = 0;
                deadflg = true;
            }
        
    }
}
