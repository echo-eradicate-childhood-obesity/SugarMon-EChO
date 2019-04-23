using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class NewDrag : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    

    [SerializeField] Vector2 clickPosition;
    [SerializeField] Vector2 currentPosition;

    [SerializeField] Vector2 startPoint;
    [SerializeField] Vector2 endPoint;
    [SerializeField] bool drag;
    [SerializeField] RectTransform DragImage;

    [SerializeField] Vector2 minanchor;
    [SerializeField] Vector2 maxanchor;

    [SerializeField] Vector2 v1;
    [SerializeField] Vector2 v2;

    public void OnPointerClick(PointerEventData eventData)
    {

    }

   
    public void OnDrag(PointerEventData eventData)
    {
        drag = true;
        endPoint = eventData.position;
        currentPosition = eventData.position;

        v1 = clickPosition;
        v2 = currentPosition;

        if (currentPosition.x < clickPosition.x)
        {
            v1.x = currentPosition.x;
            v2.x = clickPosition.x;
        }
        if (currentPosition.y < clickPosition.y)
        {
            v1.y = currentPosition.y;
            v2.y = clickPosition.y;
        }
        maxanchor = new Vector2(v2.x / Screen.width, v2.y / Screen.height);
        minanchor = new Vector2(v1.x / Screen.width, v1.y / Screen.height);

     

        DragImage.anchorMin = minanchor;
        DragImage.anchorMax = maxanchor;

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        drag = false;
       

        startPoint = new Vector2(0f, 0f);
        endPoint = new Vector2(0f, 0f);

        currentPosition = new Vector2(0f, 0f);

        minanchor = new Vector2(0f, 0f);
        maxanchor = new Vector2(0f, 0f);

        DragImage.anchorMin = new Vector2(0f, 0f);
        DragImage.anchorMax = new Vector2(0f, 0f);

        v1 = new Vector2(0f, 0f);
        v2 = new Vector2(0f, 0f);

        drag = false;
    }
}
