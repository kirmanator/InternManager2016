using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Intern { David, John, Nick, Vera}

public class PlayerMovement : MonoBehaviour {

    bool isMoving;
    Vector3 destination;
    Vector3 origPosition;

    [SerializeField]
    private int numInterns;

    private InternBehaviour currentIntern;
    AudioSource audioSource;
    public AudioSource loopingAudioSource;
    [SerializeField]
    AudioClip moveLoop;

    PlayerStats stats;

    [SerializeField]
    [Range(0.05f,0.5f)]
    private float movementSpeed;

	public Vector3 OrigPosition { get { return origPosition; } }
    public InternBehaviour CurrentIntern { get { return currentIntern; } }

	void Start () {
        stats = gameObject.GetComponent<PlayerStats>();
        origPosition = transform.position;
        destination = origPosition;

        audioSource = GetComponent<AudioSource>();
        //SetPlayerDestination(Intern.David);
	}
	
	// Update is called once per frame
	void Update () {
		if(isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed);
        }
        
        if(transform.position == destination)
        {
            if (destination == origPosition)
            {
                stats.isWorking = true;
                isMoving = false;
                //Debug.Log("Back to work again");
            }
            else
            {
                if(currentIntern.CurrentActivity != Activity.Work && currentIntern.CurrentActivity != Activity.Bug)
                {
                    // Slap their ass:
                    // Spawn slap sprite, play sound
                    audioSource.PlayOneShot(AudioManager.instance.slapClip);
                }
                if(!(stats.isReprimanding))
                {
                    stats.isHelping = true;
                }
                stats.isWorking = false;
                //Debug.Log("I have reached my destination");
                isMoving = false;
                
            }
            if (loopingAudioSource.enabled == true)
            {
                loopingAudioSource.Stop();
                loopingAudioSource.enabled = false;
            }
        }
        else
        {
            stats.isWorking = false;
        }
	}

    // Sets the new destination of the player if they are not already at that destination point
    public void SetPlayerDestination(Intern intern, bool reprimanding)
    {
        if (transform.position != GetInternDestination(intern))
        {
            foreach(InternBehaviour ib in GameObject.FindObjectsOfType<InternBehaviour>())
            {
                if(ib.internName == intern)
                {
                    currentIntern = ib;
                }
            }
            loopingAudioSource.enabled = true;
            loopingAudioSource.Play();
            isMoving = true;
            destination = GetInternDestination(intern);
            stats.isHelping = false;
            stats.isReprimanding = reprimanding;
        }
    }

    public void SetPlayerToOrigin()
    {
        if(transform.position != origPosition)
        {
            loopingAudioSource.enabled = true;
            loopingAudioSource.Play();
            isMoving = true;
            destination = origPosition;
            stats.isHelping = false;
            stats.isWorking = false;
        }
    }

    Vector3 GetInternDestination(Intern intern)
    {
        return (GameManager.instance.InternPositions[intern].position);
    }

    public bool IsHelpingIntern(InternBehaviour internB)
    {
        return (stats.isHelping && currentIntern == internB);
    }

    public bool IsRepremandingIntern(InternBehaviour internB)
    {
        return (stats.isReprimanding && currentIntern == internB && transform.position == destination);
    }
}
