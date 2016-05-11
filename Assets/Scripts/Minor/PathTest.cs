using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PathTest : MonoBehaviour {
    public Transform wayPointParent;
    Vector3[] positions;

	// Use this for initialization
	void Start () {
        positions = new Vector3[wayPointParent.childCount];

        for (int i = 0; i < wayPointParent.childCount; i++)
        {
            positions[i] = wayPointParent.GetChild(i).position;
        }

        transform.DOPath(positions, 5f, PathType.CatmullRom).SetDelay(3f).SetSpeedBased();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
