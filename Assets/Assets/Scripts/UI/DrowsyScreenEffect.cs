using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrowsyScreenEffect : MonoBehaviour
{
    private Image image;
    private CanvasGroup alphaChanger;
    private float currentEnergy;

    [SerializeField] private Color baseColor;
    [SerializeField] private Color drowsyColor;

    private float maxA = 135;
    private float startingDrowsyValue = 30;

    private bool isAnimating = false;

    void Awake()
    {
        image = GetComponent<Image>();
        alphaChanger = GetComponentInParent<CanvasGroup>();
        image.color = baseColor;
    }

    public void DoEffect(float energy)
    {
        currentEnergy = energy;

        if (energy < startingDrowsyValue)
        {
            if (!isAnimating)
            {
                isAnimating = true;
                StartCoroutine(AnimateColor());
                StartCoroutine(AnimateAlpha2());
            }     
        }
        else if(isAnimating)
        {
            isAnimating = false;
            image.color = baseColor;
            alphaChanger.alpha = 0;
        }
    }

    IEnumerator AnimateColor()
    {
        while (isAnimating)
        {
            image.color = Color.Lerp(baseColor, drowsyColor, Mathf.PingPong(Time.time, 1));
            yield return new WaitForSeconds(0.01f);
        } 
    }

    IEnumerator AnimateAlpha2()
    {
        while (isAnimating)
        {
            float alphaValue = ((maxA / startingDrowsyValue) * (startingDrowsyValue - currentEnergy)) / maxA;

            if (alphaChanger.alpha > alphaValue)
            {
                while (alphaChanger.alpha > alphaValue)
                {
                    alphaChanger.alpha -= (0.01f);
                    yield return new WaitForSeconds(0.01f);
                }
            }
            else if(alphaChanger.alpha < alphaValue)
            {
                while (alphaChanger.alpha < alphaValue)
                {
                    alphaChanger.alpha += (0.01f);
                    yield return new WaitForSeconds(0.01f);
                }
            }
            alphaChanger.alpha = alphaValue;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
