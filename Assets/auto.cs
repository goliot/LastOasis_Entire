using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class auto : MonoBehaviour
{
    public int a;
    private void Awake()
    {
        if (Regame.instance.resetCount != 0)
        {
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(Regame.instance.resetCount);
        //Regame.instance.ResetAll();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            GameManager.instance.uiResult.gameObject.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            GameManager.instance.uiResult.gameObject.SetActive(false);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Regame.instance.ResetAll();
        }
        a++;
        //Debug.Log(a);
    }
}
