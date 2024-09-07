using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolManagerRed : MonoBehaviour
{
    public static PoolManagerRed instance;

    public List<GameObject> prefabsT1 = new List<GameObject>();
    public List<GameObject> prefabsT2 = new List<GameObject>();
    public List<GameObject> prefabsT3 = new List<GameObject>();
    public List<GameObject> selectedPrefabsRed = new List<GameObject>();

    private string type;

    void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        if (GameManager.instance.currentSceneName == "SceneType1") type = "Type1";
        else if (GameManager.instance.currentSceneName == "SceneType2") type = "Type2";
        else if (GameManager.instance.currentSceneName == "SceneType3") type = "Type3";
        else type = ChooseType.Instance.enemyType;
    }

    public void OnButtonClick(int buttonID)
    {
        if (type == "Type1")
        {
            if (buttonID >= 0 && buttonID < prefabsT1.Count)
            {
                GameObject prefab = prefabsT1[buttonID]; // ��ư ID�� �ش��ϴ� �������� ������
                int cost = prefab.GetComponent<MobController>().cost;
                if (GameManager.instance.redResource >= cost)
                {
                    selectedPrefabsRed.Add(prefab); // �������� selectedPrefabs ����Ʈ�� �߰�
                    GameManager.instance.redResource -= cost;
                    Debug.Log("Red Button " + buttonID + " clicked.");
                }
            }

            // ��ư�� Ŭ���Ǿ��� �� ȣ��Ǵ� �Լ�
            // buttonID�� ����Ͽ� � �������� �Ҵ����� ����


            // ���⿡�� allocedPrefab ����Ʈ�� �������� �߰��ϴ� ������ �߰��ϼ���.
        }
        else if (type == "Type2")
        {
            if (buttonID >= 0 && buttonID < prefabsT2.Count)
            {
                GameObject prefab = prefabsT2[buttonID]; // ��ư ID�� �ش��ϴ� �������� ������
                int cost = prefab.GetComponent<MobController>().cost;
                if (GameManager.instance.redResource >= cost)
                {
                    selectedPrefabsRed.Add(prefab); // �������� selectedPrefabs ����Ʈ�� �߰�
                    GameManager.instance.redResource -= cost;
                    Debug.Log("Red Button " + buttonID + " clicked.");
                }
            }
            // ��ư�� Ŭ���Ǿ��� �� ȣ��Ǵ� �Լ�
            // buttonID�� ����Ͽ� � �������� �Ҵ����� ����
        }
        else
        {
            if (buttonID >= 0 && buttonID < prefabsT3.Count)
            {
                GameObject prefab = prefabsT3[buttonID]; // ��ư ID�� �ش��ϴ� �������� ������
                int cost = prefab.GetComponent<MobController>().cost;
                if (GameManager.instance.redResource >= cost)
                {
                    selectedPrefabsRed.Add(prefab); // �������� selectedPrefabs ����Ʈ�� �߰�
                    GameManager.instance.redResource -= cost;
                    Debug.Log("Red Button " + buttonID + " clicked.");
                }
            }
            // ��ư�� Ŭ���Ǿ��� �� ȣ��Ǵ� �Լ�
            // buttonID�� ����Ͽ� � �������� �Ҵ����� ����
        }
    }
}