using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform background;
    public RectTransform handle;

    // Joystick'in anlýk yön verisini tutar (-1 ile 1 arasýnda)
    [HideInInspector] public Vector2 inputVector;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        // Dokunulan yeri arka planýn koordinatlarýna çevir
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out position))
        {
            position.x = (position.x / background.sizeDelta.x) * 2 - 1;
            position.y = (position.y / background.sizeDelta.y) * 2 - 1;

            inputVector = new Vector2(position.x, position.y);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // Ýç yuvarlaðý hareket ettir
            handle.anchoredPosition = new Vector2(inputVector.x * (background.sizeDelta.x / 2), inputVector.y * (background.sizeDelta.y / 2));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData); // Ekrana dokunulduðu an çalýþmaya baþla
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Parmak çekildiðinde joystick'i merkeze sýfýrla
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}