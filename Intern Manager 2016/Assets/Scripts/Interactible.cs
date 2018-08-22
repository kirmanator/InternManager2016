using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour {

    [SerializeField]
    private Sprite highlightedSprite;
    GameObject highlightObject;
    public bool highlighted;

    private bool leftClicked, leftReleased, rightClicked, rightReleased;

    public bool LeftClicked { get { return leftClicked; } set { leftClicked = value; } }
    public bool LeftReleased { get { return leftReleased; } set { leftReleased = value; } }
    public bool RightClicked { get { return rightClicked; } set { rightClicked = value; } }
    public bool RightReleased { get { return rightReleased; } set { rightReleased = value; } }

    void Start () {
        highlightObject = transform.Find("Highlight").gameObject;
        highlightObject.GetComponent<SpriteRenderer>().sprite = highlightedSprite;

	}

	void Update () {

        UpdateHighlight();
        Highlight();

        if (LeftClickSelected())
        {
            if (GetComponent<InternBehaviour>())
            {
                FindObjectOfType<PlayerMovement>().SetPlayerDestination(GetComponent<InternBehaviour>().internName, false);
            }
            else
            {
                FindObjectOfType<PlayerMovement>().SetPlayerToOrigin();
            }
            leftClicked = false;
            leftReleased = false;
        }
        if(RightClickSelected())
        {
            if(GetComponent<InternBehaviour>())
            {
                if (GetComponent<InternBehaviour>().CurrentActivity != Activity.Work && GetComponent<InternBehaviour>().CurrentActivity != Activity.Bug)
                {
                    FindObjectOfType<PlayerMovement>().SetPlayerDestination(GetComponent<InternBehaviour>().internName, true);
                }
            }
            rightClicked = false;
            rightReleased = false;
        }

    }

    public void Highlight()
    {
        if (highlighted)
        {
            if (highlightObject.activeSelf)
            {
                return;
            }
            highlightObject.SetActive(true);
        }
        else
        {
            if (!highlightObject.activeSelf)
            {
                return;
            }
            highlightObject.SetActive(false);
        }
    }

    void UpdateHighlight()
    {
        if (GameManager.instance.Hit.transform != null)
        {
            if (GameManager.instance.Hit.transform.name == this.gameObject.name)
            {
                highlighted = true;
            }
            else
            {
                highlightObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                highlighted = false;
                leftClicked = false;
                leftReleased = false;
                rightClicked = false;
                rightReleased = false;
            }
        }
    }

    public void LeftClickSelect()
    {
        highlightObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        leftClicked = true;
        //Debug.Log(gameObject.name + " Lclicked");
    }

    public void LeftClickDeselect()
    {
        highlightObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        leftReleased = true;
        //Debug.Log(gameObject.name + " Lreleased");
    }

    public void RightClickSelect()
    {
        highlightObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        rightClicked = true;
        //Debug.Log(gameObject.name + " Rclicked");
    }

    public void RightClickDeselect()
    {
        highlightObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        rightReleased = true;
        //Debug.Log(gameObject.name + " Rreleased");
    }

    bool LeftClickSelected()
    {
        return (leftClicked && leftReleased);
    }

    bool RightClickSelected()
    {
        return (rightClicked && rightReleased);
    }
}
