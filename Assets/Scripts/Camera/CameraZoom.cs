using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour
{
    private float _perspectiveZoomSpeed = 60f;
    public float perspectiveZoomFarthest = 80f;
    public float perspectiveZoomClosest = 40f;
    private Camera camera;
    
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update ()
    {
        KeyBoardDebug();
    }
    
    void KeyBoardDebug()
    {
        // zoom in
        if (Input.GetKey(KeyCode.Equals))
        {
            camera.fieldOfView -= _perspectiveZoomSpeed * Time.deltaTime;
            if (camera.fieldOfView < perspectiveZoomClosest)
            {
                camera.fieldOfView = perspectiveZoomClosest;
            }
        }
        // zoom out
        else if (Input.GetKey(KeyCode.Minus))
        {
            camera.fieldOfView += _perspectiveZoomSpeed * Time.deltaTime;
            if (camera.fieldOfView > perspectiveZoomFarthest)
            {
                camera.fieldOfView = perspectiveZoomFarthest;
            }
        }
    }
}
