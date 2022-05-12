using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Math = System.Math;
public class MenuScript : MonoBehaviour
{
    [SerializeField]
    Button[] DefaultButtons;

    [Space]
    [SerializeField]
    Button[] TitleBackButtons;

    [Space]
    [SerializeField]
    Vector3[] OpenWindowSize;

    [Space]
    [SerializeField]
    Vector3[] CloseWindowSize;

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


  public  void OpenMenu(Vector3 pos)
    {
        transform.position = pos;

        ButttonRecet();
        for (int i = 0; i < DefaultButtons.Length; i++)
        {
            DefaultButtons[i].gameObject.SetActive(true);
        }
        StartCoroutine("OpeningMenu");
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
        int Count = 0;
        Vector3 ThisScale =new Vector3(0.1f,0,0);
        while (Count<OpenWindowSize.Length) {
            if (Vector3.Distance(ThisScale,OpenWindowSize[Count])>0.01f)
            {

                ThisScale.y = ThisScale.y - (ThisScale.y - OpenWindowSize[Count].y) * 0.2f;
                ThisScale.x = ThisScale.x - (ThisScale.x - OpenWindowSize[Count].x) * 0.2f;
            }
            else
            {
                ThisScale = OpenWindowSize[Count];
                Count++;
            }

            this.transform.localScale = ThisScale;
            yield return new WaitForSeconds(0.01f);
        } 
    }
}
