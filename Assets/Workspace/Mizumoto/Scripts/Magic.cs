using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region �R���|�[�l���g�̃A�^�b�`
[RequireComponent(typeof(Rigidbody))]
#endregion

// ���ۂɍU����������I�u�W�F�N�g
public class Magic : MagicBase
{
    private void OnTriggerEnter(Collider other)
    {
        // �_���[�W��^����
        // OnTriggerEnter�Ȃ̂ł�قǂ̂��Ƃ��Ȃ�������󂯂邱�Ƃ͂Ȃ��Ǝv��
        // �q�b�g�����I�u�W�F�N�g�̏����擾���ē����I�u�W�F�N�g�ɂ͓�����Ȃ��݂����Ȃ��̂�
        // ���邪�v�Z�R�X�g���オ��̂ō��͂�߂Ă��� ���������������
    }
}
