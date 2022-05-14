using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Math = System.Math;

public enum MENU
{
    DEFAULT,
    TITLE,
    CLOSE
}
public class MenuScript : MonoBehaviour
{
    [SerializeField]
    Button[] DefaultButtons;        //���j���[���J�������̉�ʂ̃{�^��

    [Space]
    [SerializeField]
    Button[] TitleBackButtons;        //�^�C�g���ɖ߂�����������̊m�F��ʂ̃{�^��

    [Space]
    [SerializeField]
    Vector3[] OpeningWindowSize;        //���j���[���J�������̃E�B���h�E�̃A�j���[�V����

    [SerializeField]
    float OpeningWindowSpeed;        //���j���[���J�������̃E�B���h�E�̃A�j���[�V�����̃X�s�[�h

    [Space]
    [SerializeField]
    Vector3[] ClosingWindowSize;        //���j���[��������̃E�B���h�E�̃A�j���[�V����

    [SerializeField]
    float ClosingWindowSpeed;        //���j���[��������̃E�B���h�E�̃A�j���[�V�����̃X�s�[�h
    bool WindowScaleflg;               //���E�B���h�E���A�j���[�V�����̓r�����ǂ���



    void Start()
    {
        WindowScaleflg = false;
    }


    //���j���[���J��
    public void OpenMenu(Vector3 pos)
    {
        if (!WindowScaleflg)
        {
            transform.position = pos;

            ButttonRecet();
            for (int i = 0; i < DefaultButtons.Length; i++)
            {
                DefaultButtons[i].gameObject.SetActive(true);
            }
            StartCoroutine("OpeningMenu");
        }
    }

    //���j���[�����
    public void CloseMenu()
    {

        if (!WindowScaleflg)
        {
            ButttonRecet();
            StartCoroutine("ClosingMenu");
        }
    }
    void TitleBack()
    {

    }
    //���j���[�̃{�^����S����A�N�e�B�u������
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
    //���j���[�̐؂�ւ�
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

    //���j���[���J���A�j���[�V����
    IEnumerator OpeningMenu()
    {
        WindowScaleflg = true;
        int count = 1;
        transform.localScale = OpeningWindowSize[0];
        while (count < OpeningWindowSize.Length)
        {
            Vector3 thisScale = transform.localScale;

            if (Vector3.Distance(thisScale, OpeningWindowSize[count]) > 0.1)
            {
                thisScale.x = thisScale.x + (OpeningWindowSize[count].x - thisScale.x) * OpeningWindowSpeed;
                thisScale.y = thisScale.y + (OpeningWindowSize[count].y - thisScale.y) * OpeningWindowSpeed;
                UnityEngine.Debug.Log(Vector3.Distance(thisScale, OpeningWindowSize[count]));
            }
            else
            {
                UnityEngine.Debug.Log("Next");
                thisScale = OpeningWindowSize[count];
                count++;
            }
            this.transform.localScale = thisScale;

            yield return new WaitForSeconds(0.01f);
        }
        WindowScaleflg = false;
    }



    //���j���[�����A�j���[�V����
    IEnumerator ClosingMenu()
    {
        WindowScaleflg = true;
        int count = 1;
        transform.localScale = ClosingWindowSize[0];
        while (count < ClosingWindowSize.Length)
        {
            Vector3 thisScale = transform.localScale;

            if (Vector3.Distance(thisScale, ClosingWindowSize[count]) > 0.01)
            {
                thisScale.x = thisScale.x + (ClosingWindowSize[count].x - thisScale.x) * ClosingWindowSpeed;
                thisScale.y = thisScale.y + (ClosingWindowSize[count].y - thisScale.y) * ClosingWindowSpeed;
                UnityEngine.Debug.Log(Vector3.Distance(thisScale, ClosingWindowSize[count]));
            }
            else
            {
                UnityEngine.Debug.Log("Next");
                thisScale = ClosingWindowSize[count];
                count++;
            }
            this.transform.localScale = thisScale;

            yield return new WaitForSeconds(0.01f);
        }
        ButttonRecet();
        WindowScaleflg = false;
        this.gameObject.SetActive(false);
    }
}
