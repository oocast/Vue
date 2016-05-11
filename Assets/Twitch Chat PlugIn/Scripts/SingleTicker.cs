using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SingleTicker : MonoBehaviour
{

    /// <summary>
    /// Call upon the creation of ticker object
    /// </summary>
    /// <param name="lifetime">
    /// Time in seconds before the ticker destroyed
    /// </param>
    public void RegisterDestroy (float lifetime)
    {
        Destroy(gameObject, lifetime);
    }

    /// <summary>
    /// Adjust the text box width based on font size and string length
    /// </summary>
    public void AdjustWidth()
    {
        int fontSize = GetComponent<Text>().fontSize;
        float charWidth = 0.75f * fontSize;
        Vector2 sizeDelta = GetComponent<RectTransform>().sizeDelta;
        sizeDelta.x = charWidth * GetComponent<Text>().text.Length;
        GetComponent<RectTransform>().sizeDelta = sizeDelta;
    }
}
