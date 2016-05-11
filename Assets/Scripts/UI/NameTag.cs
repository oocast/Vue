using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NameTag : MonoBehaviour {
    public CanvasScaler canvasScaler;
    public Transform referenceTransform;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (referenceTransform != null && canvasScaler != null)
        {
            
            Vector2 screenPos = Camera.main.WorldToScreenPoint(referenceTransform.position);
            screenPos.x /= Camera.main.pixelWidth;
            screenPos.y /= Camera.main.pixelHeight;
            screenPos.Scale(canvasScaler.referenceResolution);
            screenPos.y += 20f;
            GetComponent<RectTransform>().anchoredPosition = screenPos;
        }
        else if (referenceTransform == null)
        {
            Destroy(gameObject);
        }
	}

    public void Initialize(string shownText, Transform referenceTransform)
    {
        GetComponent<Text>().text = shownText;
        this.referenceTransform = referenceTransform;
        canvasScaler = GameObject.Find("Canvas").GetComponentInParent<CanvasScaler>();
    }
}
