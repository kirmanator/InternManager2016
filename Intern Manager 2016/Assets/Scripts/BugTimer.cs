using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BugTimer : MonoBehaviour {

    bool runDownClock;
    Image timer;
    public float progress = 1f;

	// Use this for initialization
	void Start () {
        timer = transform.Find("Timer").GetComponent<Image>();
	}

	void Update () {
        if (progress > 0)
        {
            UpdateProgress();
        }
        else
        {
            ResetBugTimer();
            runDownClock = false;
            gameObject.SetActive(false);
        }
	}

    public void ResetBugTimer()
    {
        progress = 1f;
    }

    public void RunDownClock()
    {
        runDownClock = true;
    }

    void UpdateProgress()
    {
        if(runDownClock)
        {
            timer.fillAmount = progress;
        }
    }
}
