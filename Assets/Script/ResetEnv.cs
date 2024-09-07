using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResetEnv : MonoBehaviour
{
    private string resourcesPath = "Assets/Resources/";
    private string filePath = "Temp/Environment";
    private string extension = "prefab";

    private bool saved = false;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(this.transform.childCount);
    }

    // Update is called once per frame
    void Update()
    {
        //if (!this.saved)
        //{
        //    PrefabUtilityExtension.CreateNewPrefab(this.gameObject, $"{this.resourcesPath}{this.filePath}.{this.extension}");
        //    this.saved = true;
        //}
    }
    public static void CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);

        // �ʵ� ����
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            try
            {
                field.SetValue(copy, field.GetValue(original));
            }
            catch
            {

            }
        }

        // ������Ƽ ����
        System.Reflection.PropertyInfo[] properties = type.GetProperties();
        foreach (System.Reflection.PropertyInfo property in properties)
        {
            if (property.CanWrite && property.CanRead)
            {
                try
                {
                    property.SetValue(copy, property.GetValue(original, null), null);
                }
                catch
                {
                    // ���� �߻��� ����
                }
            }
        }
    }
    public void EnvReset()
    {
        GameObject prefab = Resources.Load<GameObject>(this.filePath);
        GameObject env = Instantiate(prefab);
        if (prefab != null)
        {
            CopyComponentsFromPrefab(env, gameObject);
        }
    }

    private void CopyComponentsFromPrefab(GameObject prefab, GameObject targetGameObject)
    {
        foreach (Component prefabComponent in prefab.GetComponents<Component>())
        {
            // ���� ������Ʈ ����
            Component existingComponent = targetGameObject.GetComponent(prefabComponent.GetType());
            if (existingComponent != null)
            {
                if (existingComponent.GetType().Name != "Transform")
                {
                    Destroy(existingComponent);
                }
                else
                { 
                }
            }

            // �� ������Ʈ ����
            CopyComponent(prefabComponent, targetGameObject);
        }

        // �ڽ� ������Ʈ�� ���ؼ��� ������ �۾� ����
        for (int i = 0; i < prefab.transform.childCount; i++)
        {
            Transform prefabChild = prefab.transform.GetChild(i);
            Transform targetChild = targetGameObject.transform.Find(prefabChild.name);

            if (targetChild != null)
            {
                CopyComponentsFromPrefab(prefabChild.gameObject, targetChild.gameObject);
            }
        }
    }
}

public static class PrefabUtilityExtension
{
    public static void CreateNewPrefab(GameObject gameObject, string path)
    {
        //PrefabUtility.SaveAsPrefabAsset(gameObject, path);
    }
}

interface EnvComponents
{
    public static EnvComponents Instance { get; set; }
}