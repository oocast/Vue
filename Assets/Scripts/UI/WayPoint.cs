using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WayPoint : MonoBehaviour
{
    public GameObject target;
    public Camera referenceCamera;
    Vector2 dir;

    void Awake()
    {

    }

    void Start()
    {
        //camera = GetComponent<Camera>();
    }
    void Update()
    {
        if (GetComponent<Image>().enabled)
        {
            Vector3 screenPos = referenceCamera.WorldToScreenPoint(target.transform.position);
            dir = screenPos - transform.position;
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, -angle);
            //transform.Rotate(0, 0, angle);
        }
    }

    void AssignNewTarget(GameObject target)
    {
        this.target = target;
    }
}
 


