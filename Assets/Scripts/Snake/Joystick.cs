using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

    Image backImg;
    Image stick;
    Vector3 input;

    void Start()
    {
        backImg = GetComponent<Image>();
        stick = transform.GetChild(0).GetComponent<Image>();
    }


    public void OnPointerDown(PointerEventData data)
    {
        OnDrag(data);
    }

    public void OnPointerUp(PointerEventData data)
    {
        input = Vector3.zero;
        stick.rectTransform.anchoredPosition = input;
    }

    public void OnDrag(PointerEventData data)
    {
        var rect = backImg.rectTransform;
        var camera = data.pressEventCamera;
        var dataPos = data.position;
        Vector2 pos;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, dataPos, camera, out pos))
        {
            if(RectTransformUtility.ScreenPointToLocalPointInRectangle(rect,dataPos, camera, out pos))
            {
                pos.x = pos.x / backImg.rectTransform.sizeDelta.x * 2;
                pos.y = pos.y / backImg.rectTransform.sizeDelta.y * 2;

                input = new Vector3(pos.x, pos.y, 0);
                input = (input.magnitude > 1) ? input.normalized : input;

                float x = input.x * rect.sizeDelta.x * 0.4f;
                float y = input.y * rect.sizeDelta.y * 0.4f;

                stick.rectTransform.anchoredPosition = new Vector3(x, y, 0);
            }
        }
    }

    public float Horizontal()
    {
        return input.x;
    }

    public float Vertical()
    {
        return input.y;
    }
}
