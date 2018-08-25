using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Activity { Bug, InNout, LaCroix, LinkedIn, Overwatch, Twitter, Work, Youtube }

public class InternBehaviour : MonoBehaviour {

    GameObject activityBox;
    SpriteRenderer activitySpriteRenderer;
    GameObject bugTimerDisplay;
    AudioSource audio;

    [SerializeField]
    private float workTimeDurationMin, workTimeDurationMax;
    private float workTimeDuration;
    private float workTimer;

    [SerializeField]
    private float bugFixTimeDurationMin, bugFixTimeDurationMax;
    private float bugFixTimeDuration;
    private float goofOffTimer;

    private bool isWorking;
    private bool isGoofingOff;
    private bool isSlapped;

    //public Sprite activityBoxSprite;
    private Sprite activitySprite;

    public AudioClip helpSound1, helpSound2;

    private Activity currentActivity;
    public Intern internName;

    [SerializeField]
    private float activityRotationSpeed;
    private float activitySpriteRotation;

    public Activity CurrentActivity { get { return currentActivity; } }
	
	void Start () {
        Initialize();
	}

	void Update () {
        if (isWorking)
        {
            UpdateWorkTimer();
        }
        else if(isGoofingOff)
        {
            if(GameManager.instance.PMovement.IsHelpingIntern(this))
            {
                if(currentActivity == Activity.Bug)
                {
                    UpdateGoofOffTimer();
                    if(!bugTimerDisplay.activeSelf)
                    {
                        bugTimerDisplay.SetActive(true);
                    }
                    bugTimerDisplay.GetComponent<BugTimer>().RunDownClock();
                    bugTimerDisplay.GetComponent<BugTimer>().progress = goofOffTimer / bugFixTimeDuration;

                }
            }
            if (GameManager.instance.PMovement.IsRepremandingIntern(this))
            {
                Debug.Log("I am reprimanding an intern");
                if(currentActivity != Activity.Work && currentActivity != Activity.Bug)
                {
                    Debug.Log("The intern is goofing off");
                    UpdateGoofOffTimer();
                }
            }
        }
        if(activitySpriteRenderer)
        {
            activitySpriteRenderer.gameObject.transform.localRotation = Quaternion.Euler(0, 0, activitySpriteRotation);
        }
    }

    void Initialize()
    {
        //activityBox = transform.Find("ActivityBox").gameObject;
        activitySpriteRenderer = transform.Find("ActivitySprite").GetComponent<SpriteRenderer>();
        activitySpriteRotation = activitySpriteRenderer.gameObject.transform.localEulerAngles.z;
        Debug.Log("sprite rotation: " + activitySpriteRotation);
        activitySpriteRenderer.gameObject.transform.localRotation = Quaternion.Euler(0, 0, activitySpriteRotation);
        StartCoroutine(RotateActivitySprite());

        bugTimerDisplay = transform.Find("BugTimerDisplay").gameObject;
        audio = GetComponent<AudioSource>();

        bugTimerDisplay.SetActive(false);

        //activityBox.GetComponent<SpriteRenderer>().sprite = activityBoxSprite;

        currentActivity = Activity.Work;
        isWorking = true;

        bugFixTimeDuration = UnityEngine.Random.Range(bugFixTimeDurationMin, bugFixTimeDurationMax);
        workTimeDuration = UnityEngine.Random.Range(workTimeDurationMin, workTimeDurationMax);

        ResetWorkTimer();
        ResetGoofOffTimer();

        UpdateActivity(currentActivity);
    }

    void UpdateActivity(Activity activity)
    {
        if(activity == Activity.Bug)
        {
            GameManager.instance.AddLargeIntervalToPStats();
        }
        else
        {
            if(activity != Activity.Work)
            {
                GameManager.instance.AddSmallIntervalToPStats();
            }
        }
        activitySprite = GameManager.instance.Activities[(int)activity];
        currentActivity = activity;
        activitySpriteRenderer.sprite = activitySprite;
        Debug.Log("Updated " + gameObject.name + "'s activity to " + currentActivity.ToString());
    }

    Activity ChooseRandomActivity()
    {
        int randomActivity = (int)Mathf.Round(UnityEngine.Random.Range(0, Enum.GetNames(typeof(Activity)).Length));
        if((Activity)randomActivity == Activity.Bug)
        {
            int choice = (int)Mathf.Round(UnityEngine.Random.value);
            audio.PlayOneShot(AudioManager.instance.BugClip);
            audio.PlayOneShot(choice == 0 ? helpSound1 : helpSound2);
        }
        
        return (Activity)randomActivity;
    }

    IEnumerator RotateActivitySprite()
    {
        yield return new WaitForSeconds(activityRotationSpeed);
        activitySpriteRotation *= -1;
        StartCoroutine(RotateActivitySprite());
    }

    #region Timer BS
    void ResetWorkTimer()
    {
        // Reset time duration after min and max values have been altered
        workTimeDuration = UnityEngine.Random.Range(workTimeDurationMin, workTimeDurationMax);
        workTimer = workTimeDuration;
        isWorking = true;
    }

    void ResetGoofOffTimer()
    {
        if (currentActivity == Activity.Bug)
        {
            // Reset time duration after min and max values have been altered
            bugFixTimeDuration = UnityEngine.Random.Range(bugFixTimeDurationMin, bugFixTimeDurationMax);
            goofOffTimer = bugFixTimeDuration;
        }
        else
        {
            goofOffTimer = 0;
        }
    }

    public void UpdateGoofOffTimer()
    {
        if(goofOffTimer <= 0)
        {
            if(currentActivity == Activity.Bug)
            {
                GameManager.instance.SubtractLargeIntervalFromPStats();
            }
            else
            {
                GameManager.instance.SubtractSmallIntervalFromPStats();
            }
            UpdateActivity(Activity.Work);
            ResetWorkTimer();
            isGoofingOff = false;
            GameManager.instance.PStats.isReprimanding = false;
            return;
        }
        goofOffTimer -= Time.deltaTime;
    }

    void UpdateWorkTimer()
    {
        if(workTimer <= 0)
        {
            if(currentActivity != Activity.Work)
            {
                return;
            }
            Activity newActivity;
            do
            {
                newActivity = ChooseRandomActivity();
            } while (newActivity == Activity.Work);

            UpdateActivity(newActivity);
            isWorking = false;
            isGoofingOff = true;
            ResetGoofOffTimer();
            return;
        }
        workTimer -= Time.deltaTime;
    }
    #endregion
}
