using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFromDestroy : MonoBehaviour
{
    public static SaveFromDestroy instance;
    private void Awake()
    {
        // 인스턴스가 없으면 현재 인스턴스로 설정하고 파괴되지 않도록 함
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // 이미 인스턴스가 존재하면 중복된 인스턴스이므로 파괴
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
