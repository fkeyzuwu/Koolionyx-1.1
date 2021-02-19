using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    private Image image;
    Color32 minColor = new Color32(67, 212, 130, 255);
    Color32 maxColor = new Color32(67, 212, 212, 255);

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        image.color = Color.Lerp(minColor, maxColor, Mathf.PingPong(Time.time, 1));
    }
}
