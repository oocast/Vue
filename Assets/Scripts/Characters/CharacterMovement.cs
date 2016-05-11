using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{
    /// <summary>
    /// Event when the position of character updates
    /// </summary>
    public event System.Action onCharacterMove;
    public event System.Action<float> onCharacterMoveSpeed;

    public bool isGrounded;

    public float speed = 8f;
    ThumbStick thumbstick;
    CharacterAttack attack;
    CharacterController controller;
    PlayerAnimation playerAnimation;

    [HideInInspector]
    public Vector3 characterMoveVector;

    CountLock moveLock;

    void Awake()
    {
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
    }

    // Use this for initialization
    void Start () {
        thumbstick = GameObject.Find("Thumbstick Background").GetComponent<ThumbStick>();
        attack = GetComponent<CharacterAttack>();
        controller = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time > attack.staggerBeforeThisTime)
        {
            Vector3 dragVector = thumbstick.GetDragVector();
            characterMoveVector = dragVector * speed;
            //transform.position += characterMoveVector;
            controller.SimpleMove(characterMoveVector);
            playerAnimation.UpdateRunningSpeed(dragVector.magnitude);
            isGrounded = controller.isGrounded;
            if (dragVector.magnitude > 0.4f)
            {
                transform.rotation = Quaternion.LookRotation(thumbstick.GetDragVector());
                
                //Handheld.Vibrate();
                //Debug.Log(thumbstick.GetDragVector());
            }

            if (onCharacterMove != null)
            {
                onCharacterMove();
            }
            if (onCharacterMoveSpeed != null)
            {
                onCharacterMoveSpeed(dragVector.magnitude);
            }
        }
    }

    public void SimpleMove(Vector3 velocity)
    {
        controller.SimpleMove(velocity);
    }
}
