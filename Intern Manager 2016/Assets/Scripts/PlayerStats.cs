using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    private float insanityLevel;
    public float insanityRate;
    public float largeInsanityInterval, smallInsanityInterval;
    public float idealInsanityRate;
    public float insanityMax;
    public bool isWorking;
    public bool isHelping;
    public bool isReprimanding;

    PlayerMovement playerMovement;

	void Start () {
        insanityRate = idealInsanityRate;
        playerMovement = GetComponent<PlayerMovement>();
        StartCoroutine(IncreaseInsanity());
        isWorking = true;
	}
	
	void Update () {
        if(insanityLevel < 0)
        {
            insanityLevel = 0;
        }
        if(insanityRate < idealInsanityRate)
        {
            insanityRate = idealInsanityRate;
        }
        UIManager.instance.UpdateInsanityProgress(insanityLevel / insanityMax);
	}

    public void AddToInsanityRate(float value)
    {
        insanityRate += value;
    }

    public void AddToInsanityLevel(float value)
    {
        insanityLevel += value;
    }

    IEnumerator IncreaseInsanity()
    {
        insanityLevel += insanityRate;
        yield return new WaitForSeconds(1);

        StartCoroutine(IncreaseInsanity());
        yield return null;
    }
}
