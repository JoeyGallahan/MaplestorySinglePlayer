using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEffects : MonoBehaviour
{
    [SerializeField] GameObject fade;
    Image fadeImage;

    [SerializeField] float fadeLength = 1.0f;
    [SerializeField] float fadeDuration = 0f;
    public bool fadingOut = false;
    public bool fadingIn = false;

    private void Awake()
    {
        fadeImage = fade.GetComponentInChildren<Image>();
    }

    private void Start()
    {
        fade.SetActive(false);
    }

    private void Update()
    {
        UpdateCounters();
    }

    private void UpdateCounters()
    {
        //If you're fading out to black
        if (fadingOut)
        {
            if (!fade.activeInHierarchy) //If the fade image is inactive
            {
                fade.SetActive(true); //activate it
            }

            if (fadeDuration < fadeLength) //If you've still got some time
            {
                fadeDuration += Time.deltaTime; 
                Fade(); //Continue fading out
            }
            else
            {
                fadingOut = false;
            }
        }
        //Else if youre fading in from black
        else if (fadingIn)
        {
            if (fadeDuration > 0.0f) //We're reversing the time here so we can just re-use the same method
            {
                fadeDuration -= Time.deltaTime;
                Fade();
            }
            else
            {
                fadeDuration = 0.0f;
                fadingIn = false;
            }
        }
    }

    private void Fade()
    {
        Color currentColor = fadeImage.color; //Get the color of the fading out image
        currentColor.a = fadeDuration / fadeLength; //Get the percentage of time that has passed compared to how long we want it to fade

        fadeImage.color = currentColor; //Update the alpha of the fade image to align with the time passed
    }
}
