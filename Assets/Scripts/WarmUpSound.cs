using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Scripted by Suyash
// Edited by Anjali, Amit Oct 28,2021
public class WarmUpSound : AnalyzeSound
{
    //private float lastFeedback = 0;
    private float lowFrequency = 0;
    private float highFrequency = 15000;
    private float pitchLowThreshold = 250; // increased by Amit for testing(construction noise nearby) Original Value : 100
    //private float pitchHighThreshold = 2000;

    public bool startedSpeaking = false;
    public bool stoppedSpeaking = true;
    public bool speaking = false;
    public float minSpeakingSeconds = 1; // To avoid registering small sound like ah, ahms or any unwanted sounds

    public int startedSpeakingTime = 0;

    private BookMover BookMoverScript;
    public override int AnalyzePitch()
    {
        if (isListening)
        {

            currentPitch = GetPitch(lowFrequency, highFrequency);
            Debug.Log(currentPitch);
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

        CheckRepStatus();
        //if (AnalyzePitch()==0)
        //{
        //    BookMoverScript = GameObject.Find("Book").GetComponent<BookMover>();
        //    BookMoverScript.SetMovement(moveFactor: 0.1f, speed: 2);
        //}
        //Debug.LogError("player speaking status" + AnalyzePitch());
        // if startedspeaking and now stopped speaking(no sound speaking) -->
    }


    void CheckRepStatus()
    {
        //check if the player has completed the rep[i.e if player had started speaking in past and  few seconds(minSpeakingSeconds) has passed.]
        Debug.Log("Checking for Stuff Speaking=" + speaking.ToString() + " stoppedSpeaking= " + stoppedSpeaking.ToString() + " startedSpeaking= " + startedSpeaking.ToString());

        if (speaking && CheckTime())
        { // if player has been speaking and the time is acceptable
            Debug.Log("Spearking");
            stoppedSpeaking = CheckIfStoppedSpeaking();
            if (stoppedSpeaking && !speaking)
            {
                Debug.Log("Stopped Spearking");
                BookMoverScript = GameObject.Find("Book").GetComponent<BookMover>();
                BookMoverScript.SetMovement(moveFactor: 0.1f, speed: 2);
                startedSpeaking = false;
                stoppedSpeaking = true;
                speaking = false;
            }
            //do nothing
        }
        else
        {
            startedSpeaking = CheckIfStartedSpeaking();
        }


    }

    bool CheckIfStartedSpeaking()
    {
        // if the user has started speaking and stoppedSpeaking is true, return true else return false
        // we dont want to check during the rep(when the player is speaking) .
        if (speaking)
        {
            return true;
        }
        else if (AnalyzePitch() == 1 && stoppedSpeaking)
        {
            startedSpeakingTime = GetUnixTime();
            speaking = true;
            //stoppedSpeaking = false;
            return true;
        }
        else if (AnalyzePitch() == 1 && !stoppedSpeaking && !speaking && !startedSpeaking)
        {
            speaking = true;
            startedSpeakingTime = GetUnixTime();
            //stoppedSpeaking = false;
            return true;
        }
        return false;
    }

    bool CheckIfStoppedSpeaking()
    {
        // if the user has started speaking and stoppedSpeaking is true, return true else return false
        // we dont want to check during the rep(when the player is speaking) .
        //Debug.Log("Checking for Silence");

        if (AnalyzePitch() == 0 && speaking && startedSpeaking)
        {
            speaking = false;
            startedSpeaking = false;
            return true;
        }
        else if (!speaking && !startedSpeaking)
        {
            return true;
        }
        return false;
    }

    bool CheckTime()
    {
        //return true;
        // if the gap between the startedspeaking and stoppedspeaking time is acceptable. return true, else false
        int currentTime = GetUnixTime();
        if (startedSpeakingTime != 0) //only do the time check if this variable was overridden after initialization
        {
            Debug.Log("Checking time Start: " + startedSpeakingTime.ToString() + " Curr= " + currentTime.ToString() + " Diff=" + (currentTime - startedSpeakingTime).ToString());
            if ((currentTime - startedSpeakingTime) > minSpeakingSeconds)
            {
                return true;
            }
        }

        return false;
    }

    int GetUnixTime()
    {
        return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
    }
}
