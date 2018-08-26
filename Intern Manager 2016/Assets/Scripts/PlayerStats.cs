using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    [SerializeField]
    [Range(0.1f, 4)]
    private float insanityIncreaseRate;
    private float insanityLevel;
    public float insanityRate;
    public float largeInsanityInterval, smallInsanityInterval;
    public float idealInsanityRate;
    public float insanityMax;
    public bool isWorking;
    public bool isHelping;
    public bool isReprimanding;
    private GameObject gear, slapHand;
    private SpriteRenderer gearRndr;
    public float gearRotationSpeed;

    PlayerMovement playerMovement;

	void Start () {
        insanityRate = idealInsanityRate;
        isWorking = true;

        playerMovement = GetComponent<PlayerMovement>();
        gear = transform.Find("WorkGear").gameObject;
        slapHand = transform.Find("SlapHand").gameObject;
        gearRndr = gear.GetComponent<SpriteRenderer>();

        slapHand.SetActive(false);
        StartCoroutine(IncreaseInsanity());
	}
	
	void Update () {
        if (insanityLevel > insanityMax)
        {
            GameManager.instance.DisableInterns();
            insanityLevel = insanityMax;
            insanityRate = 0;
            isWorking = false;
            isHelping = false;
            isReprimanding = false;
            UIManager.instance.FadeOut();
            return;
        }

        

        if (insanityLevel < 0)
        {
            insanityLevel = 0;
        }

        if(isWorking)
        {
            gearRndr.enabled = true;
            gear.transform.Rotate(Vector3.forward, gearRotationSpeed);
        }
        else
        {
            gearRndr.enabled = false;
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
        yield return new WaitForSeconds( 1 / insanityIncreaseRate);

        StartCoroutine(IncreaseInsanity());
        yield return null;
    }

    public void ActivateHand()
    {
        slapHand.SetActive(true);
    }
}
