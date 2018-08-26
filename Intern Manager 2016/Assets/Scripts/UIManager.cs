using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance;

    public Image insanityProgressImg;
    public Image clockProgressImg;
    public Image razbury;
    private GameObject fadeObject;
    private SpriteRenderer fadeObjectRndr;
    private bool isFading;
    [SerializeField]
    private float alphaRate;
    private float currentAlpha;
    
    [SerializeField]
    private List<Sprite> razburySprites;

	void Start () {
		if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        fadeObject = transform.Find("Fade").gameObject;
        fadeObjectRndr = fadeObject.GetComponent<SpriteRenderer>();

        currentAlpha = 0;
        UpdateAlpha();
	}

	void Update () {
		if(isFading)
        {
            if(currentAlpha >= 1)
            {
                currentAlpha = 1;
                FindObjectOfType<MenuManager>().GoToScene(2);
                return;
            }
            currentAlpha += Time.deltaTime / alphaRate;
            UpdateAlpha();
        }
	}

    public void UpdateInsanityProgress(float progress)
    {
        insanityProgressImg.fillAmount = 1 - progress;
        //Debug.Log("Progress: " + progress);
        razbury.sprite = razburySprites[(int)(Mathf.Floor(progress * (razburySprites.Count + 0.5f) >= razburySprites.Count ? razburySprites.Count - 1 : progress * (razburySprites.Count + 0.5f)))];
    }

    public void UpdateClockProgress(float progress)
    {
        clockProgressImg.fillAmount = progress;
    }

    void UpdateAlpha()
    {
        fadeObjectRndr.color = new Color(fadeObjectRndr.color.r, fadeObjectRndr.color.g, fadeObjectRndr.color.b, currentAlpha); 
    }

    public void FadeOut()
    {
        isFading = true;
    }


}
