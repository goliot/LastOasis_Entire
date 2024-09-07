using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
public class Movement : MonoBehaviour
{
    public float speed = 5.0f; // �̵� �ӵ�
    public float attackRange = 2.0f; // ���� ����
    private SkeletonAnimation spineAnimation;

    void Start()
    {
        // Spine ������Ʈ�� ã���ϴ�.
        spineAnimation = GetComponent<SkeletonAnimation>();
    }

    void Update()
    {
        // "red" �±׸� ���� ������Ʈ�� ã���ϴ�.
        GameObject[] redObjects = GameObject.FindGameObjectsWithTag("RedBuilding");

        // ���� ����� "red" �±׸� ���� ������Ʈ�� ã���ϴ�.
        GameObject closestRedObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject redObject in redObjects)
        {
            
            float distance = Vector3.Distance(transform.position, redObject.transform.position);
            if (distance < closestDistance)
            {
                closestRedObject = redObject;
                closestDistance = distance;
            }
        }

        // ���� ����� "red" �±׸� ���� ������Ʈ�� ���� �̵��մϴ�.
        if (closestRedObject != null)
        {
            Vector3 direction = (closestRedObject.transform.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);
            spineAnimation.AnimationName = "Side_Walk";
        }
    }
}
