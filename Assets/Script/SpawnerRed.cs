using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerRed : MonoBehaviour
{
    public static SpawnerRed instance;

    public float minX = 25.0f; // x �ּҰ�
    public float maxX = 30.0f; // x �ִ밪
    public float minY = -5.0f; // y �ּҰ�
    public float maxY = 5.0f; // y �ִ밪

    public Transform parentObject;
    public GameObject shadowObject;

    public List<GameObject> selectedPrefabsRed = new List<GameObject>();

    public int mobCount;

    float spawnTime;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        spawnTime = GameManager.instance.spawnTime;
        // spawnTime �������� SpawnPrefab �Լ� ȣ�� ����
        InvokeRepeating("SpawnPrefab", 0f, spawnTime);
    }

    private void Update()
    {
        mobCount = parentObject.childCount;
    }

    // �ֱ������� ȣ��� �Լ�
    void SpawnPrefab()
    {
        selectedPrefabsRed = PoolManagerRed.instance.selectedPrefabsRed;
        foreach (GameObject prefab in selectedPrefabsRed)
        {
            if (prefab != null)
            {
                // ������ x, y ��ǥ ����
                float randomX = Random.Range(minX, maxX);
                float randomY = Random.Range(minY, maxY);

                // ��ȯ ����: �������� ������ ��ġ�� ��ȯ
                Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);
                GameObject instantiatedPrefab = Instantiate(prefab, spawnPosition, Quaternion.identity);
                instantiatedPrefab.GetComponent<MobController>();
                
                instantiatedPrefab.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f); //���� ũ�� ����
                instantiatedPrefab.transform.SetParent(parentObject);

                //�±� ����
                instantiatedPrefab.tag = "Red";

                //�׸��� ��ȯ
                GameObject shadow = Instantiate(shadowObject, instantiatedPrefab.transform.position, Quaternion.identity);
                shadow.transform.SetParent(instantiatedPrefab.transform);
            }
        }
    }
}