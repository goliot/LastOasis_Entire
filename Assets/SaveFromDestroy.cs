using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFromDestroy : MonoBehaviour
{
    public static SaveFromDestroy instance;
    private void Awake()
    {
        // �ν��Ͻ��� ������ ���� �ν��Ͻ��� �����ϰ� �ı����� �ʵ��� ��
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // �̹� �ν��Ͻ��� �����ϸ� �ߺ��� �ν��Ͻ��̹Ƿ� �ı�
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
