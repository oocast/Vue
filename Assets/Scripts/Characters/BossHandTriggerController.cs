using UnityEngine;
using System.Collections;

public class BossHandTriggerController : MonoBehaviour {

    public GameObject leftHandObject;
    public GameObject rightHandObject;
    public GameObject armObject;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Enable/Disable the triggers on boss hands according to mask
    /// </summary>
    /// <param name="mask">bit 0: left hand, bit 1: right hand</param>
    public void ToggleHandTrigger(int mask)
    {
        if (leftHandObject != null && rightHandObject != null)
        {
            if ((mask & 1) > 0)
            {
                leftHandObject.SetActive(true);
            }
            else
            {
                leftHandObject.SetActive(false);
            }

            if ((mask & 2) > 0)
            {
                rightHandObject.SetActive(true);
            }
            else
            {
                rightHandObject.SetActive(false);
            }

            if ((mask & 4) > 0)
            {
                armObject.SetActive(true);
            }
            else
            {
                armObject.SetActive(false);
            }
        }
    }
}
