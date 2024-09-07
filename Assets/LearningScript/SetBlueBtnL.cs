using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetBlueBtnL : MonoBehaviour
{
    public Sprite[] sprites1;
    public Sprite[] sprites2;
    public Sprite[] sprites3;

    //[HideInInspector]
    //public Sprite[] sprites1_default;

    //[HideInInspector]
    //public Sprite[] sprites2_default;

    //[HideInInspector]
    //public Sprite[] sprites3_default;

    public Button[] buttons;
    public string type;

    void Start()
    {
        //sprites1_default = sprites1;
        //sprites2_default = sprites2;
        //sprites3_default = sprites3;

        type = PoolManager.instance.type;
        Debug.Log("Type = " + type);

        if (buttons != null)
        {
            if (type=="Type1")
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].GetComponent<Image>().sprite = sprites1[i];
                }
            }
            else if (type=="Type2")
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].GetComponent<Image>().sprite = sprites2[i];
                }
            }
            else if (type=="Type3")
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].GetComponent<Image>().sprite = sprites3[i];
                }
            }
        }
    }

    //public void Reset()
    //{
    //    sprites1 = this.sprites1_default;
    //    sprites2 = this.sprites2_default;
    //    sprites3 = this.sprites3_default;
    //}
}
