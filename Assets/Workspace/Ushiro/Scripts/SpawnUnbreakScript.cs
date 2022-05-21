using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnbreakScript : MonoBehaviour
{


    private static bool Loaded { get; set; }

    [SerializeField]
    GameObject[] gameManagerPrefabs = null;


    void Awake()
    {
        //���łɃ��[�h�ς݂Ȃ�A��d�ɍ쐬���Ȃ�
        if (Loaded) return;

        Loaded = true;

        //�v���n�u���C���X�^���X�����āADontDestroyOnLoad
        foreach (var prefab in gameManagerPrefabs)
        {
            GameObject GameIndex = Instantiate(prefab);
            DontDestroyOnLoad(GameIndex);
        }
    }

    void Start()
    {
    }

    void Update()
    {

    }
}
