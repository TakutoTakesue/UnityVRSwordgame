using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAction : MonoBehaviour
{
    #region �V���A���C�Y�ϐ�
    [SerializeField,Header("�̗͂̍ő�l")]
    protected int hpMax;
    protected int hp;   // ���݂̗̑�
    public int HP => hp;    // �Q�b�^�[

    [SerializeField, Header("�U����")]
    protected int atk;
    public int ATK => atk;  // �Q�b�^�[
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        hp = hpMax; // hp�̏�����
    }

    // �_���[�W���󂯂�֐�
    // @param atk �U����
    public void OnDamage(int atk) {
        atk = Mathf.Max(atk, 0);    // �U���͂�0�ȉ��̏ꍇ��0�ɂ���
        hp = Mathf.Max(hp - atk, 0);    // hp��0�����ɂȂ�ꍇ��0�ɂ���
    }

    // �񕜊֐�
    // @param resilience �񕜗�
    public void Recovery(int resilience) {
        resilience = Mathf.Max(resilience, 0);    // �񕜗͂�0�ȉ��̏ꍇ��0�ɂ��� �_���[�W���󂯂�\��������̂�
        hp = Mathf.Min(hp + resilience, hpMax);    // hp���ȉ����ɂȂ�ꍇ��0�ɂ���
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
