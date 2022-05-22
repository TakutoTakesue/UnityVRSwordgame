using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���@�̃x�[�X
public class MagicBase : MonoBehaviour
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
    public struct MagicInformation
    {
        MagicKinds preparationMagic;
        float interval;
    }

    MagicKinds selectionMagic;    // ���\���Ă������
    #endregion
    #region �V���A���C�Y�ϐ�
    [SerializeField, Header("�������Ă������")]
    MagicInformation[] magic;
    [SerializeField, Header("�͈͍U�����ǂ���")]
    bool rangeAttack = false;
    [SerializeField, Header("���@���΂��X�s�[�h")]
    float speed = 1.0f;
    [SerializeField, Header("�����̊�")]
    GameObject magicObj;
    #endregion
    #region protected�ϐ�
    protected int maxMagic = 0;   // ���������Ă�������̍ő吔
    protected int nowMagic = 0;   // �\���Ă������
    protected int somebody = 0;   // �������̂͒N�Ȃ̂�
    #endregion

    /// <summary>
    /// ���@�𒼐��I�ɕ���
    /// <paramref name="target"/> �������ǂ̃I�u�W�F�N�g�ɔ�΂���
    /// </summary>
    void MagicShot(GameObject target) {
       var magicobj = Instantiate(magicObj, transform.position, Quaternion.identity);
        // �������z�[�~���O������
        magicobj.AddComponent<HomingAction>();  
    }

    // ���ڑ���̈ʒu�ɐ��݂���
    void DirectlyMagic(GameObject target) {
        // �����𒼐ڃ^�[�Q�b�g�ɏo��
        var magicobj = Instantiate(magicObj, target.transform.position, Quaternion.identity);
       
    }
}
