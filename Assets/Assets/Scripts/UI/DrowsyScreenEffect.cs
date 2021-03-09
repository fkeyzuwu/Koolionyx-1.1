using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrowsyScreenEffect : MonoBehaviour
{
    private Image image;
    private CanvasGroup alphaChanger;

    [SerializeField] private Color baseColor;
    [SerializeField] private Color drowsyColor;

    private float maxA = 135;
    private float startingDrowsyValue = 50;

    private bool isAnimatingColor = false;

    void Awake()
    {
        image = GetComponent<Image>();
        alphaChanger = GetComponentInParent<CanvasGroup>();
        image.color = baseColor;
    }

    public void DoEffect(float energy)
    {
        if(energy < startingDrowsyValue)
        {
            StartCoroutine(AnimateAlpha(energy));

            if (!isAnimatingColor)
            {
                isAnimatingColor = true;
                StartCoroutine(AnimateColor(energy));
            }     
        }
        else
        {
            isAnimatingColor = false;
            image.color = baseColor;
            alphaChanger.alpha = 0;
        }
    }

    IEnumerator AnimateColor(float energy)
    {
        while (isAnimatingColor)
        {
            image.color = Color.Lerp(baseColor, drowsyColor, Mathf.PingPong(Time.time, 1));
            yield return new WaitForSeconds(0.01f);
        } 
    }

    IEnumerator AnimateAlpha(float energy)
    {
        float alphaValue = ((maxA / startingDrowsyValue) * (startingDrowsyValue - energy)) / maxA;

        if (alphaChanger.alpha > alphaValue)
        {
            while (alphaChanger.alpha >= alphaValue)
            {
                alphaChanger.alpha -= (0.01f);
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            while (alphaChanger.alpha <= alphaValue)
            {
                alphaChanger.alpha += (0.01f);
                yield return new WaitForSeconds(0.01f);
            }
        }

        alphaChanger.alpha = alphaValue;
    }
}
