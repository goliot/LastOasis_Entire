using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour
{
    private Vector2 inputStartPos;
    private Vector2 inputEndPos;
    public float moveSpeed = 1f;
    public Transform mapBounds; // Map�� ���

    private bool isMoving = false;
    private bool uiTouched = false;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (!IsPointerOverUIObject(touch.position))
                {
                    inputStartPos = touch.position;
                    isMoving = true;
                }
                else
                {
                    uiTouched = true;
                }
            }
            else if (touch.phase == TouchPhase.Moved && isMoving)
            {
                inputEndPos = touch.position;
                Vector2 deltaPos = (inputEndPos - inputStartPos) * moveSpeed;
                Vector3 newPosition = transform.position - new Vector3(deltaPos.x, deltaPos.y, 0) * Time.deltaTime;

                // ���� ��� �������� ī�޶� ��ġ ����
                if (mapBounds)
                {
                    Bounds bounds = mapBounds.GetComponent<Collider2D>().bounds;
                    float halfHeight = Camera.main.orthographicSize;
                    float halfWidth = halfHeight * Camera.main.aspect;

                    newPosition.x = Mathf.Clamp(newPosition.x, bounds.min.x + halfWidth, bounds.max.x - halfWidth);
                    newPosition.y = Mathf.Clamp(newPosition.y, bounds.min.y + halfHeight, bounds.max.y - halfHeight);
                }

                transform.position = newPosition;
                inputStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (!uiTouched)
                {
                    isMoving = false;
                }
                else
                {
                    uiTouched = false;
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIObject(Input.mousePosition))
            {
                inputStartPos = Input.mousePosition;
                isMoving = true;
            }
            else
            {
                uiTouched = true;
            }
        }
        else if (Input.GetMouseButton(0) && isMoving)
        {
            inputEndPos = Input.mousePosition;
            Vector2 deltaPos = (inputEndPos - inputStartPos) * moveSpeed;
            Vector3 newPosition = transform.position - new Vector3(deltaPos.x, deltaPos.y, 0) * Time.deltaTime;

            // ���� ��� �������� ī�޶� ��ġ ����
            if (mapBounds)
            {
                Bounds bounds = mapBounds.GetComponent<Collider2D>().bounds;
                float halfHeight = Camera.main.orthographicSize;
                float halfWidth = halfHeight * Camera.main.aspect;

                newPosition.x = Mathf.Clamp(newPosition.x, bounds.min.x + halfWidth, bounds.max.x - halfWidth);
                newPosition.y = Mathf.Clamp(newPosition.y, bounds.min.y + halfHeight, bounds.max.y - halfHeight);
            }

            transform.position = newPosition;
            inputStartPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (!uiTouched)
            {
                isMoving = false;
            }
            else
            {
                uiTouched = false;
            }
        }
    }

    // UI ��� ���� ��ġ �����Ͱ� �ִ��� Ȯ���ϴ� �Լ�
    private bool IsPointerOverUIObject(Vector2 touchPosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = touchPosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}
