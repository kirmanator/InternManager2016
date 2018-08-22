using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Image insanityProgressImg;
    public Image clockProgressImg;
    public static UIManager instance;

	void Start () {
		if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
	}

	void Update () {
		
	}

    public void UpdateInsanityProgress(float progress)
    {
        insanityProgressImg.fillAmount = progress;
    }

    public void UpdateClockProgress(float progress)
    {
        clockProgressImg.fillAmount = progress;
    }


}
