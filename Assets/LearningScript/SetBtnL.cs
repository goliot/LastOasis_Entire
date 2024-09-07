using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetBtnL : MonoBehaviour
{
    public Sprite[] sprites;
    public Button[] buttons;

    void Start()
    {
        if (buttons != null)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].GetComponent<Image>().sprite = sprites[i];
            }
        }
    }
}
