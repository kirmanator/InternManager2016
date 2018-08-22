using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioClip projectFinishedClip;
    public AudioClip slapClip;
    public AudioClip BugClip;
    public AudioClip doneWithDayClip;

    public static AudioManager instance;

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
}
