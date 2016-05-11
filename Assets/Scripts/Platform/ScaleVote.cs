using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ScaleVote : GameContentVote {
    float currentScale;

	// Use this for initialization
	void Start () {
        currentScale = 1f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    protected override void ChangeContent(params string[] content)
    {
        if (content.Length > 0)
        {
            string scaleString = content[0];
            float scale = 1f;
            if (scaleString.Equals("L"))
            {
                scale = 2f;
            }
            else if (scaleString.Equals("M"))
            {
                scale = 1.5f;
            }
            else if (scaleString.Equals("S"))
            {
                scale = 0.5f;
            }

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float relativeScale = scale / currentScale;
            currentScale = scale;
            foreach (GameObject enemy in enemies)
            {
                enemy.transform.localScale *= relativeScale;
            }
        }
    }
}
