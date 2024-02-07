using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI FpsText;

    private float pollingTime = 1f;
    private float time;
    private int frameCount;


    void Update()
    {
        time += Time.deltaTime;

        frameCount++;

        if (time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt((float)frameCount / time);
            FpsText.text = frameRate.ToString() + "\nFPS";

            time -= pollingTime;
            frameCount = 0;
        }
    }
}
