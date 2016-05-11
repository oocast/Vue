using UnityEngine;
using System.Collections;

public class TurretFollow : MonoBehaviour {
    //values that will be set in the Inspector
    public GameObject target;
    public float rotationSpeed;

    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void Update()
    {
        var direction = target.transform.position - transform.position;

        // Set Y the same to make the rotations turret-like:
        direction.y = transform.position.y;

        var rot = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(
                                         transform.rotation,
                                         rot,
                                         rotationSpeed * Time.deltaTime);
        Vector3 euler = transform.eulerAngles;
        euler.x = 0;
        euler.z = 0;
        transform.eulerAngles = euler;
    }
}
