using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    CharacterMovement characterMovement;
    Transform playerAvatorTransform;
    Vector3 playerToCamera;
    //ThumbStick thumbstick;

    void Awake()
    {
        playerAvatorTransform = GameObject.Find("Character").transform;
        characterMovement = playerAvatorTransform.GetComponent<CharacterMovement>();
        characterMovement.onCharacterMove += () =>
        {
            transform.position = playerToCamera + playerAvatorTransform.position;
        };
    }


    // Use this for initialization
    void Start ()
    {
        playerToCamera = transform.position - playerAvatorTransform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {

    }
}
