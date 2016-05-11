using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class LoadGameScene : MonoBehaviour {
    public Image progress;

	// Use this for initialization
	void Start () {
        // Debug.Log(Application.loadedLevelName);
        Debug.Log("Start " + Application.loadedLevelName);
	    if (progress != null)
        {
            StartCoroutine(LoadGame());
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator LoadGame()
    {
        AsyncOperation loadingOperation = Application.LoadLevelAsync(1);
        /*
        loadingOperation.allowSceneActivation = false;
        progress.DOFillAmount(1f, 2.5f);
        yield return new WaitForSeconds(2f);
        loadingOperation.allowSceneActivation = true;
        */
        
        while (!loadingOperation.isDone)
        {
            progress.fillAmount = loadingOperation.progress;
            Debug.Log(loadingOperation.progress);
            yield return null;
        }
        
    }
}
