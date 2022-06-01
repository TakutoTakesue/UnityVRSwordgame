using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyScript : MonoBehaviour
{
    protected List<GameObject> player = new List<GameObject>();
    protected GameObject nearPlayer; // ��ԋ߂��v���C���[
    protected GameObject targetPlayer; // ���ݕW�I�ɂ��Ă���v���C���[���i�[���邽�߂̕ϐ�
    [Header("�G��HP : int")]
    [SerializeField] protected int HP;
    [Header("�G�̈ړ����x : float")]
    [SerializeField] protected float mySpeed;
    [Header("�U���� : int")]
    [SerializeField] protected int power;
    [Header("Debug�p : �^�[�Q�b�g�ɂ���Ă��邩")]
    [SerializeField] protected bool battleFlg;
    // Start is called before the first frame update
    protected void Start()
    {
        // �v���C���[��S��List�ɒǉ�
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var i in players)
        {
            player.Add(i);
        }
        battleFlg = false;
    }

    // �N����ԋ߂������肷�邽�߂̏���
    protected void FindTarget()
    {
        float distance = 0.0f;
        float saveDistace = 100.0f;
        foreach (var i in player)
        {
            distance = Vector3.Distance(this.gameObject.transform.position, i.transform.position);
            if (saveDistace > distance)
            {
                saveDistace = distance;
                nearPlayer = i;
            }
        }
    }

    public void OnDamage(int num)
    {
        HP -= num;
    }

    public void ChangeBattleFlg()
    {
        battleFlg = !battleFlg;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
