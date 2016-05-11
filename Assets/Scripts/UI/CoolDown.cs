using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoolDown : MonoBehaviour {
   // public float countDownTime;
   // float counter;
    Image CoolDownImage;
    private bool coolingDown;
    private float waitTime;
    
        // Use this for initialization
    void Start () {
        CoolDownImage = GetComponent<Image>();
        coolingDown = false;
       // countDownTime = -1f;
        //    counter = 0f;
    }
    /*
        // Update is called once per frame
        void Update () {
            if (countDownTime > 0f)
            {
                counter += Time.deltaTime;

            }
        }

        public void SetTimer(float cooldownTime)
        {
            countDownTime = cooldownTime;
            CoolDownImage = GetComponent<Image>();
        }

        */

    // Update is called once per frame
    void Update()
    {
        if (coolingDown == true)
        {
            //Reduce fill amount over 30 seconds
            CoolDownImage.fillAmount -= Time.deltaTime / waitTime;
        }
    }

    public void SetTimer(float cooldownTime)
    {
        waitTime = cooldownTime;
        CoolDownImage.fillAmount = 1f;
        coolingDown = true;
    }

    public void ClearTimer()
    {
        CoolDownImage.fillAmount = 0f;
        coolingDown = false;
    }
}
