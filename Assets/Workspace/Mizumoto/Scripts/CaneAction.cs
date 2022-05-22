using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaneAction : WeaponBase
{
    #region �V���A���C�Y�ϐ�
    [SerializeField, Header("���b�N�I���̃s��")]
    GameObject LockOnCursor;
    #endregion
    #region �v���C�x�[�g�ϐ�
    Transform yeyPos = null; // �ڂ̈ʒu
    IEnumerator weaponPick;    // �R�[���\�`���ϐ�
    Magic magic;
    #endregion



    // �擾����Ă���Ƃ��������R�[���\�`��
    IEnumerator WeaponPick() {
        // 1 �ڐ�����Ray���΂�
        RaycastHit hit;
        // �ڐ�����Ray���΂�
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
        // 2 ����Ray�ɃI�u�W�F�N�g�������Ă����炻�ꂪ�^�[�Q�b�g���Ƃ��I�u�W�F�N�g������

        // 3 �����^�[�Q�b�g������I�u�W�F�N�g�̏ꍇ�̓^�[�Q�b�gUI��\������


        yield return new WaitForSeconds(0.1f);

    }

    // ����擾���鏈��
    public override void Take(GameObject player, GameObject camera) 
    {
        // �ϐ��ɑ��
        this.player = player;
        playercamera = camera;
        yeyPos = camera.transform;
        weaponPick = WeaponPick();
        StartCoroutine(weaponPick);
    }

    // ������������
    public override void release()
    {
        this.player = null;
        playercamera = null;
        yeyPos = null;
        StopCoroutine(weaponPick);
    }

}
