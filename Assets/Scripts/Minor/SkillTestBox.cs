using UnityEngine;
using System.Collections;

public class SkillTestBox : MonoBehaviour {
    public ShockWaveSkill shockWaveSkill;
    public BombSkill bombSkill;
    public MovementSkill movementSkill;
    public MovementSkill blinkSkill;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.K))
        {
            shockWaveSkill.ActivateSkill(gameObject);
        }
	}
}
