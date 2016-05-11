using UnityEngine;
using System.Collections;

public class ShowAttackRange : MonoBehaviour
{
    LineRenderer lineRenderer;
    Transform characterTransform;
    float _lineHight = 0.1f;

	// Use this for initialization
	void Start ()
    {
        
        characterTransform = transform.parent;
        lineRenderer = GetComponent<LineRenderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void DrawAttackRange(float halfBrandishAngle, float brandishDistanceMax, float brandishDistanceMin = 0)
    {
        Vector3 forward = transform.forward;
        Vector3 rightBound = Quaternion.AngleAxis(halfBrandishAngle, Vector3.up) * forward;
        rightBound.Normalize();
        lineRenderer.SetPosition(0, transform.position + rightBound * brandishDistanceMin);
        lineRenderer.SetPosition(1, transform.position + rightBound * brandishDistanceMax);
        lineRenderer.SetPosition(2, transform.position + forward * brandishDistanceMax);

        Vector3 leftBound = Quaternion.AngleAxis(-halfBrandishAngle, Vector3.up) * forward;
        leftBound.Normalize();
        lineRenderer.SetPosition(3, transform.position + leftBound * brandishDistanceMax);
        lineRenderer.SetPosition(4, transform.position + leftBound * brandishDistanceMin);
        lineRenderer.SetPosition(5, transform.position + rightBound * brandishDistanceMin);
    }

    public void FlushAttackRange()
    {
        for (int i = 0; i < 6; i++)
        {
            lineRenderer.SetPosition(i, Vector3.zero);
        }
    }
}
