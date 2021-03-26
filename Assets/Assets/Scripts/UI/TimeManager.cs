using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private float currentEnergy;
    private float slowdownStartingValue = 30;
    private bool isTimeScaleChanging = false;

    private float slowdownFactor;
    private float minTimeScale = 0.5f;
    private float maxTimeScale = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Slowdown(float energy)
    {
        currentEnergy = energy;

        if(energy < slowdownStartingValue)
        {
            if (!isTimeScaleChanging)
            {
                isTimeScaleChanging = true;
                StartCoroutine(TimeScaleChange());
            }
        }
        else
        {
            isTimeScaleChanging = false;
            Time.timeScale = 1;
        }
    }

    IEnumerator TimeScaleChange()
    {
        while (isTimeScaleChanging)
        {
            float newTimeScale = minTimeScale * (slowdownStartingValue - currentEnergy);
            Time.fixedDeltaTime = newTimeScale * 0.2f;

            if (Time.timeScale > newTimeScale)
            {
                while(Time.timeScale > newTimeScale)
                {
                    Time.timeScale -= 0.01f;
                    yield return new WaitForSeconds(Time.fixedDeltaTime);
                }
                
            }
            else if(Time.timeScale < newTimeScale)
            {
                while(Time.timeScale < newTimeScale)
                {
                    Time.timeScale += 0.01f;
                    yield return new WaitForSeconds(Time.fixedDeltaTime);
                }
            }

            Time.timeScale = newTimeScale;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }
}
