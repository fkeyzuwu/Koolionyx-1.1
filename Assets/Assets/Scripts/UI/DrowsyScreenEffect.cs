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
    private bool isAnimatingAlpha = false;

    void Awake()
    {
        image = GetComponent<Image>();
        alphaChanger = GetComponentInParent<CanvasGroup>();
        image.color = baseColor;
    }

    public void DoEffect(float energy)
    {
        if(energy <= startingDrowsyValue)
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
        int operatorToDo;
        float alphaValue = ((maxA / startingDrowsyValue) * (startingDrowsyValue - energy)) / maxA;

        if (alphaChanger.alpha > alphaValue)
        {
            operatorToDo = -1;
        }
        else
        {
            operatorToDo = 1;
        }

        while (alphaChanger.alpha != alphaValue)
        {
            alphaChanger.alpha = alphaChanger.alpha + (0.01f * operatorToDo);
            yield return new WaitForSeconds(0.01f);
        }

        alphaChanger.alpha = alphaValue;
    }
}
