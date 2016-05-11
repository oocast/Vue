using UnityEngine;
using System.Collections;

public class CameraMeasurement : MonoBehaviour
{
    public Transform[] edgeMarkers = new Transform[4];
    private float _speed = 10f;
    Ray[] rays = new Ray[4];
    Camera camera;
    ThumbStick thumbstick;

    // Use this for initialization
    void Start () {
        camera = GetComponent<Camera>();
        thumbstick = GameObject.Find("Thumbstick Background").GetComponent<ThumbStick>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * _speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * _speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.forward * _speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.back * _speed * Time.deltaTime;
        }
        //if (GameObject.Find("Thumbstick Background").GetComponent<ThumbStick>())
        transform.position += thumbstick.GetDragVector() * _speed * Time.deltaTime;

        rays[0] = camera.ViewportPointToRay(new Vector3(0, 0, camera.nearClipPlane));
        rays[1] = camera.ViewportPointToRay(new Vector3(0, 1, camera.nearClipPlane));
        rays[2] = camera.ViewportPointToRay(new Vector3(1, 1, camera.nearClipPlane));
        rays[3] = camera.ViewportPointToRay(new Vector3(1, 0, camera.nearClipPlane));
        for (int i = 0; i < 4; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(rays[i], out hit))
            {
                edgeMarkers[i].position = hit.point;
            }
        }

        
        //Debug.Log(camera.WorldToViewportPoint(new Vector3(0,0,0)));
    }

    Vector3 CancelHeight (Vector3 input)
    {
        input.y = 0;
        return input;
    }
}
