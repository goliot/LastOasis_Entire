using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nukeManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 1�� �ڿ� ���� ���� ������Ʈ�� �ı�
        StartCoroutine(DestroyAfterDelay(1.0f));
    }

    // �ڷ�ƾ�� ����Ͽ� ���� �� ���� ������Ʈ �ı�
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void Update()
    {
        Collider2D collider = gameObject.GetComponent<Collider2D>();
        MoveCollider(collider);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        try {
            if (collider.gameObject.CompareTag("Red") && collider.gameObject.name != "Knight(Clone)" && (collider.gameObject.layer == 7 || collider.gameObject.layer == 10))
            {
                MobController temp = collider.gameObject.GetComponent<MobController>();
                if (temp != null)
                {
                    NukePower.instance[0].blueKillCount++;
                    NukePower.instance[0].blueTotalDmg += temp.health;
                    temp.takeHit(99999);
                }
            }
            else
            {
                KnightController temp = collider.GetComponent<KnightController>();
                if (temp != null)
                {
                    temp.takeHit(500);
                }
            }
        }
        catch (MissingReferenceException)
        {
            return;
        }
    }

    private void MoveCollider(Collider2D collider)
    {
        // �̵��� ����� ���� ���� (��: x��ǥ�� 1.0f�� ����)
        float moveAmount = 0.00001f;

        // ���� ��ġ���� moveAmount��ŭ �̵�
        Vector3 newPosition = collider.transform.position;
        newPosition.x += moveAmount;

        // Collider�� ��ġ�� ������Ʈ
        collider.transform.position = newPosition;
    }
}
