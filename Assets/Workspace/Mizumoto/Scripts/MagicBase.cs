using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBase : WeaponBase
{
    #region enum�錾
    // �����̎��
    public enum MagicKinds
    {
        flame,  // ���̎���
        recovery,   // ��
        thunder // �T���_�[
    }
    #endregion
    #region �X�g���N�g�ϐ�
    // �����\����
    public struct Magic {
        MagicKinds preparationMagic;
        float interval;
    }

    MagicKinds selectionMagic;    // ���\���Ă������
    #endregion

    [SerializeField, Header("�������Ă��閂�@")]
    Magic[] magic;


}
