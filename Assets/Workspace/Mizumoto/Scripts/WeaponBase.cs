using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region �R���|�[�l���g�̃A�^�b�`
[RequireComponent(typeof(Rigidbody))]
#endregion

public class WeaponBase : MonoBehaviour
{
    #region protected�ϐ��錾
    protected GameObject player;    // �v���C���[�I�u�W�F�N�g
    protected GameObject playercamera;    // �v���C���[�̃J����
    #endregion
   

    // �E��ꂽ�Ƃ��̏���
    public virtual void Take(GameObject player, GameObject camera) 
    {
        this.player = player;
        playercamera = camera;
    }

    // �����������Ƃ��̏���
    public virtual void release()
    {
        this.player = null;
        playercamera = null;
    }

}
