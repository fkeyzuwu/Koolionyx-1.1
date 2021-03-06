using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrowsyScreenEffect : MonoBehaviour
{
    private Image image;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color drowsyColor;

    private bool isAnimating = false;

    private float maxA = 0.6f; //TODO: Based on percentage of the energy, once its under 50 make the alpha bigger in order to show more of the image

    void Awake()
    {
        image = GetComponent<Image>();
        baseColor = image.color;
    }

    public void DoEffect(float energy)
    {
        if(energy <= 50)
        {
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
            //change alpha based on how much energy the player has
            yield return new WaitForSeconds(0.01f);
        } 
    }
}
