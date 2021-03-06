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

    private float startingDrowsyValue = 50;

    private bool isAnimating = false;

    private float maxA = 135;

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
            alphaChanger.alpha = ((maxA / startingDrowsyValue) * (startingDrowsyValue - energy)) / 135;

            if (!isAnimating)
            {
                isAnimating = true;
                StartCoroutine(AnimateColor(energy));
            }     
        }
        else
        {
            isAnimating = false;
            image.color = baseColor;
        }
    }

    IEnumerator AnimateColor(float energy)
    {
        while (isAnimating)
        {
            image.color = Color.Lerp(baseColor, drowsyColor, Mathf.PingPong(Time.time, 1));
            yield return new WaitForSeconds(0.01f);
        } 
    }
}
