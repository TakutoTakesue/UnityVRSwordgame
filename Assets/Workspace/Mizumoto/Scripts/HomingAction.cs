using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region �R���|�[�l���g�̃A�^�b�`
[RequireComponent(typeof(Rigidbody))]
#endregion

public class HomingAction : MonoBehaviour
{
    #region Serialize�ϐ��錾
    [SerializeField, Header("�ڕW�I�u�W�F�N�g")]
    GameObject target;
    [SerializeField, Header("�X�s�[�h")]
    float speed = 2;
    [SerializeField, Header("�z�[�~���O��؂鋗��")]
    float stopDistance = 2;
    #endregion
    #region �v���C�x�[�g�ϐ�
    Rigidbody myRb;  // ���g�̃��W�b�h�{�f�B;
    #endregion


    // Start is called before the first frame update
    private void Start()
    {
        myRb = GetComponent<Rigidbody>();   // �擾
    }


    private void FixedUpdate()
    {
        #region �z�[�~���O���邩�`�F�b�N
        if (!target) {
            return;
        }
        #endregion

        #region �z�[�~���O����
        // �܂�������΂�
        var movepow = transform.forward * speed;
        // �^�[�Q�b�g�Ɍ������Ĕ�΂�
        var difference = target.transform.position - transform.position;
        myRb.velocity = difference.normalized * speed;
        #endregion

        #region �z�[�~���O��؂鏈��
        // ������x�܂ŋ߂Â����ꍇ�z�[�~���O�v�f���L��
        if (Vector3.Distance(target.transform.position, transform.position) <= stopDistance) {
            target = null;
        }
        #endregion
    }

}
