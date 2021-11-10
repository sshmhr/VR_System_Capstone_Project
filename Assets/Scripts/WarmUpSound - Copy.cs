using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Scripted by Suyash

public class WarmUpSound : AnalyzeSound
{
    //private float lastFeedback = 0;
    private float lowFrequency = 0;
    private float highFrequency = 15000;
    private float pitchLowThreshold = 100;
    //private float pitchHighThreshold = 2000;


    public override int AnalyzePitch()
    {
        if (isListening)
        {
            currentPitch = GetPitch(lowFrequency, highFrequency);
            if (currentPitch < pitchLowThreshold) return 0;
            else return 1;
        }
        else return -1;
    }

    public override void HandleAttemptCompletion()
    {
        switch (attempt)
        {
            case 1:
                attempt++;
                AttemptComplete();
                break;
            case 2:
                attempt++;
                AllAttemptsComplete();
                break;
            default:
                attempt++;
                AllAttemptsComplete();
                break;
        }
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        Debug.LogError("player speaking status" + AnalyzePitch());
    }
}
