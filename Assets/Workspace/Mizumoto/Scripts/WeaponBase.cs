using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    protected GameObject player;    // �v���C���[�I�u�W�F�N�g
    protected GameObject playercamera;    // �v���C���[�̃J����
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // �E��ꂽ�Ƃ��̏���
    public void Take(GameObject player, GameObject camera)
    {
        this.player = player;
        playercamera = camera;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
