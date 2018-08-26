using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*Game Flow:
 * - Intern starts with Work Activity
 * 
 * - Each Intern has internal timer which randomly changes their current activity once the timer runs out
 * - If the new activity is goofing off (anything but bug or work), player must walk over to their position and simply reprimand them (slap them)
 * - If the new activity is a bug, player must spend a certain amount of time helping to fix it
 * - Once either processes has ended, interns go back to work
 * 
 * - Player's Insanity level increases while interns are goofing off
 * - Player must return back to their computer to decrease Insanity level
 * 
 * - Duration of interns' work timers begin to decrease after each bug
 */
public class GameManager : MonoBehaviour {

    public static GameManager instance;
    RaycastHit hit;

    System.DateTime dateTime;
    string displayTime;
    int startTime, endTime;

    private float currentTime;

    [SerializeField]
    private int totalTime;

    private float timeInterval;

    [SerializeField]
    private int timeMultiplier = 1;

    public Dictionary<Intern, Transform> InternPositions;
    [Header("Sprites")]
    public Sprite bugSprite, inNOutSprite, laCroixSprite, linkedInSprite, overwatchSprite, twitterSprite, workSprite, youtubeSprite;
    private List<Sprite> activities;

    private PlayerMovement pMovement;
    private PlayerStats pStats;

    public List<Sprite> Activities {
        get {
            if (activities != null)
            { return activities; }
            return new List<Sprite>();
        }
    }

    public RaycastHit Hit { get { return hit; } }
    public PlayerMovement PMovement { get { return pMovement; } }
    public PlayerStats PStats { get { return pStats; } }

	void Start () {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        pMovement = FindObjectOfType<PlayerMovement>();
        pStats = FindObjectOfType<PlayerStats>();

        activities = new List<Sprite>();
        activities.Add(bugSprite);      
        activities.Add(inNOutSprite);
        activities.Add(laCroixSprite);  
        activities.Add(linkedInSprite);
        activities.Add(overwatchSprite);
        activities.Add(twitterSprite);  
        activities.Add(workSprite);     
        activities.Add(youtubeSprite);  

        InternPositions = new Dictionary<Intern, Transform>();

        for(Intern f = Intern.David; f < Intern.Vera + 1; f ++)
        {
            InternPositions[f] = GameObject.Find(f.ToString()).transform.Find("playerDestination");
            //Debug.Log("Position for " + f + ": " + InternPositions[f].position);
        }

        dateTime = DateTime.Now;
        
        startTime = (dateTime.Hour * 60) + dateTime.Minute;
        currentTime = 0;

        endTime = startTime + totalTime;
    }
	
	void Update () {
        UpdateTime();
        UpdateDisplayTime((int)Mathf.Round((startTime + currentTime) / 60), (int)Mathf.Round((startTime + currentTime) % 60));
        GetInput();
        UIManager.instance.UpdateClockProgress(1 - (currentTime / totalTime));
	}

    void UpdateTime()
    {
        timeInterval = Time.deltaTime;
        currentTime += (timeInterval * timeMultiplier);
    }

    // Display time in 24 hour format
    void UpdateDisplayTime(int hour, int minutes)
    {
        displayTime = (minutes < 10) ? hour + ":0" + minutes : hour + ":" + minutes;
        //Debug.Log("Current time: " + displayTime);
    }

    void GetInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction* 1000);
        Physics.Raycast(ray, out hit);
        if (hit.transform != null)
        {
            if (hit.transform.GetComponent<Interactible>())
            {
                Interactible interactible = hit.transform.GetComponent<Interactible>();
                if (Input.GetMouseButtonDown(0))
                {
                    interactible.LeftClickSelect();
                    //Debug.Log(hit.transform.name + " clicked");
                }
                else if(Input.GetMouseButtonDown(1))
                {
                    interactible.RightClickSelect();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (interactible.LeftClicked)
                    {
                        interactible.LeftClickDeselect();
                    }
                    //Debug.Log(hit.transform.name + " released");
                }
                else if(Input.GetMouseButtonUp(1))
                {
                    if(interactible.RightClicked)
                    {
                        interactible.RightClickDeselect();
                    }
                }
            }
        }
    }

    public void DisableInterns()
    {
        foreach(Interactible interact in FindObjectsOfType<Interactible>())
        {
            interact.enabled = false;
        }
        foreach(InternBehaviour ib in FindObjectsOfType<InternBehaviour>())
        {
            ib.enabled = false;
        }
    }

    public void AddLargeIntervalToPStats()
    {
        pStats.AddToInsanityRate(pStats.largeInsanityInterval);
    }

    public void AddSmallIntervalToPStats()
    {
        pStats.AddToInsanityRate(pStats.smallInsanityInterval);
    }

    public void SubtractSmallIntervalFromPStats()
    {
        pStats.AddToInsanityRate(-1 * pStats.smallInsanityInterval);
    }

    public void SubtractLargeIntervalFromPStats()
    {
        pStats.AddToInsanityRate(-1 * pStats.largeInsanityInterval);
    }

    public void SubtractSmallFromInsanityLevel()
    {
        pStats.AddToInsanityLevel(-1 * pStats.smallInsanityInterval);
    }

    public void SubtractLargeFromInsanityLevel()
    {
        pStats.AddToInsanityLevel(-1 * pStats.largeInsanityInterval);
    }
}
