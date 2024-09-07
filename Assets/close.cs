using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class close : MonoBehaviour
{
    public GameObject inventory;
    public Button[] buttons;

    public void closeInventory()
    {
        inventory.SetActive(false);
        foreach(Button button in buttons)
        {
            button.gameObject.SetActive(true);
        }
    }
}
