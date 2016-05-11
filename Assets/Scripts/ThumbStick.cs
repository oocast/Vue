using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ThumbStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image backgroundImage;
    private Image thumbstickImage;
    private Vector3 inputVector;

    void Start()
    {
        backgroundImage = GetComponent<Image>();
        thumbstickImage = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag (PointerEventData pointerEventData)
    {
        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(backgroundImage.rectTransform,
                                                                    pointerEventData.position,
                                                                    pointerEventData.pressEventCamera,
                                                                    out position)) 
        {
            position.x = (position.x / backgroundImage.rectTransform.sizeDelta.x);
            position.y = (position.y / backgroundImage.rectTransform.sizeDelta.y);

            inputVector = new Vector3(position.x * 2, 0, position.y * 2);
            inputVector = (inputVector.magnitude > 1f) ? inputVector.normalized : inputVector;

            // Move Thumbstick image
            thumbstickImage.rectTransform.anchoredPosition =
                new Vector3(inputVector.x * (backgroundImage.rectTransform.sizeDelta.x / 2),
                            inputVector.z * (backgroundImage.rectTransform.sizeDelta.y / 2));
        }
    }

    public virtual void OnPointerDown (PointerEventData pointerEventData)
    {
        OnDrag(pointerEventData);
    }

    public virtual void OnPointerUp (PointerEventData pointerEventData)
    {
        inputVector = Vector3.zero;
        thumbstickImage.rectTransform.anchoredPosition = Vector3.zero;
    }

    public Vector3 GetDragVector ()
    {
        return inputVector;
    }

    void OnDisable()
    {
        OnPointerUp(null);
    }
}
