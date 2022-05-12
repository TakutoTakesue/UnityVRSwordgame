using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    Button[] DefaultButtons;

    [Space]
    [SerializeField]
    Button[] TitleBackButtons;

    [Space]
    [SerializeField]
    Vector2 MaxWindowSize;

    enum MENU
    {
        DEFAULT,
        TITLE,
        CLOSE
    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OpenMenu(Vector3 pos)
    {
        transform.position = pos;

        ButttonRecet();
        for (int i = 0; i < DefaultButtons.Length; i++)
        {
            DefaultButtons[i].gameObject.SetActive(true);
        }
    }
    void TitleBack()
    {

    }

    void ButttonRecet()
    {
        for (int i = 0; i < TitleBackButtons.Length; i++)
        {
            TitleBackButtons[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < DefaultButtons.Length; i++)
        {
            DefaultButtons[i].gameObject.SetActive(false);
        }
    }

    void ChangeMenu(MENU m)
    {
        ButttonRecet();
        switch (m)
        {
            case MENU.DEFAULT:
                for (int i = 0; i < DefaultButtons.Length; i++)
                {
                    DefaultButtons[i].gameObject.SetActive(true);
                }
                break;
            case MENU.TITLE:
                for (int i = 0; i < TitleBackButtons.Length; i++)
                {
                    TitleBackButtons[i].gameObject.SetActive(true);
                }
                break;
            case MENU.CLOSE:
                break;
        }
    }
    IEnumerator OpeningMenu()
    {
        float WindowX = 0.1f;
        float WindowY = 0.1f;


        float XSpeed = 0.01f;
        float YSpeed = 0.01f;

        while (true) {
            if (WindowY<1)
            {
                YSpeed += 0.01f;
                WindowX += XSpeed;
                WindowY += YSpeed;
            }
            else
            {
            
            }


            yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));
        } 
    }
}
