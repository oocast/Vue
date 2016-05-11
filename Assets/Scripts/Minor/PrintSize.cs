using UnityEngine;
using System.Collections;

public class PrintSize : MonoBehaviour {

	// Use this for initialization
	void Start () {
        RectTransform rect = GetComponent<RectTransform>();
        Debug.Log(rect.sizeDelta);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
