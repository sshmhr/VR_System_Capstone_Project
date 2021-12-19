using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static System.Math;

// Added by Anjali, Amit Oct 28,2021
// Updated and altered by suyash on 11-11 21
public class BookMover : MonoBehaviour
{
    private GameController gameController;
    public SoundController SoundController;
    public GameObject book;
    public GameObject couldron;
    public GameObject feather;
    public GameObject plant;
    public GameObject light;

    private GameObject currentObject;

    public Vector3[] bookFinalTransform = new Vector3[3];
    public Vector3[] couldronFinalTransform = new Vector3[3];
    public Vector3[] featherFinalTransform = new Vector3[3];
    public Vector3[] plantFinalTransform = new Vector3[3];
    public Vector3[] lightFinalTransform = new Vector3[3];

    public Vector3[] bookBaseTransform = new Vector3[3];
    public Vector3[] couldronBaseTransform = new Vector3[3];
    public Vector3[] featherBaseTransform = new Vector3[3];
    public Vector3[] plantBaseTransform = new Vector3[3];
    public Vector3[] lightBaseTransform = new Vector3[3];

    private Vector3[] currentObjectFinalTransform, currentObjectBaseTransform;
    private Vector3[] cuyrrentObjectStep = new Vector3[3];


    private Vector3 objectStartPosition; // to be set by the handleRep Function
    private Vector3 objectEndRotation;
    private Vector3 objectEndScale;
    private Vector3 objectEndPosition; // to be set by the handleRep Function

    
    // handles sound start and end
    private bool hasSoundStarted = false;
    private bool hasSoundEnded = true;

    // Number of sounds to do in this play
    private int numOfSoundsToPlay = 0;
    public int numOfSoundsRemainingToPlay = 0;
    private List<string> sounds;

    public float maxBrightness = 10;
    public float movementSpeed = 2;
    private int currentActivity;
    public int maxStepsAllowed = 10;
    public int Movementspeed;
    private int stepsLeft;
    public DataManager dataManager;
    private bool hasActivityEnded = false;
    private bool isGameOver = false; // to be used by function (probably) UI part,
    private Light[] lights;
                                     // which restarts the game or starts from welcome screen

    // Update is called once per frame


    void Start()
    {
        maxStepsAllowed = dataManager.getMaxStepsAllowed();
        gameController = gameObject.GetComponent<GameController>();
        stepsLeft = maxStepsAllowed;
        currentActivity = gameController.getCurrentActivity();
        HandleNSounds(-1); 
    }

    void Update()
    {
        isGameOver = CheckGameOver();
        if (!isGameOver)
        {
            // unless sound is played, Dont to anything
            if (!SoundController.hasSoundFinishedPlaying()) return;
            // getDistance(currentObject.transform.position, currentObjectFinalTransform[0]) < 0.01
            if (hasActivityEnded)
                handleEventEnd();
            if(currentObject!=null) CheckMovement();
        }
        else
        {
            Debug.Log("Game Over");
            // Do stuff , Show menu and retry option
        }
    }


    public void handleRep()
    {
        handleGameState();
        if (currentActivity == GameController.LIGHTACTIVITY)
            handleLightActivity();

        handleDefaultActivities();
    }

    private void handleLightActivity()
    {
        float lightStepValue = maxBrightness / maxStepsAllowed;
        if (lights == null) lights = (Light[])FindObjectsOfType(typeof(Light));
        foreach (Light light in lights) {
            light.intensity += lightStepValue;
        }
    }

    private void handleDefaultActivities()
    {
        detectCurrentObject();
        //move = true;

        Vector3[] moveFactor = calculateMoveFactor();
        objectEndPosition = currentObject.transform.position + moveFactor[0];
        objectEndRotation = currentObject.transform.rotation.eulerAngles + moveFactor[1];
        objectEndScale = currentObject.transform.localScale + moveFactor[2];
    }

    void CheckMovement()
    {

        // Check and stop the movement with help of StopMovement function
        //else move the object with animation
        if (currentObject == null) return;
        playVFX();
        if (Math.Abs(Vector3.Distance(currentObject.transform.position, objectEndPosition)) > 0.01)
        {
            // Debug.Log("ERR" + currentObject.transform.position + " " + objectEndPosition);
            MoveObject();
        }

        if (Math.Abs(Vector3.Distance(currentObject.transform.rotation.eulerAngles, objectEndRotation)) > 0.01)
        {
            Debug.LogError("currentObject.transform.rotation.eulerAngles " + currentObject.transform.rotation.eulerAngles);
            Debug.LogError(" objectEndPosition " + objectEndRotation);
            rotateObject();
        }

        if (Math.Abs(Vector3.Distance(currentObject.transform.localScale, objectEndScale)) > 0.01)
        {
            //Debug.LogError("Dist " + Vector3.Distance(currentObject.transform.localScale, objectEndScale));
            scaleObject();
        }


    }

    private void playVFX()
    {
        //currentObject.GetComponent<ParticleSystem>().Play();
    }

    private void detectCurrentObject()
    {
        switch (currentActivity)
        {
            case GameController.BOOKACTIVITY:
                currentObjectBaseTransform = bookBaseTransform;
                currentObjectFinalTransform = bookFinalTransform;
                currentObject = book;
                break;
            case GameController.COULDRONACTIVITY:
                currentObjectBaseTransform = couldronBaseTransform;
                currentObjectFinalTransform = couldronFinalTransform;
                currentObject = couldron;
                break;
            case GameController.FEATHERACTIVITY:
                currentObjectBaseTransform = featherBaseTransform;
                currentObjectFinalTransform = featherFinalTransform;
                currentObject = feather;
                break;
            case GameController.TREEACTIVITY:
                currentObjectBaseTransform = plantBaseTransform;
                currentObjectFinalTransform = plantFinalTransform;
                currentObject = plant;
                break;

            case GameController.LIGHTACTIVITY:
                currentObjectBaseTransform = lightBaseTransform;
                currentObjectFinalTransform = lightFinalTransform;
                currentObject = light;
                break;
        }
    }

    public bool CheckGameOver()
    {
        if (numOfSoundsRemainingToPlay == -1) // when the last sound is played
        {
            return true;
        }
        return false;
    }

    // call this from the UI, if you want to let the kind practice n sounds.
    public void HandleNSounds(int n)
    {
        SoundController.SetUpSound(); // can be setup in soundconttroller or outside
        isGameOver = false;
        sounds = SoundController.GetNSound(n);
        Debug.Log(sounds);
        numOfSoundsToPlay = sounds.Count; // max sound that can be played is what we have on the soundcontroller.cs
        numOfSoundsRemainingToPlay = sounds.Count;
        gameController.showGameObjects(gameController.findCurrentGameObjects());
        SoundController.PlaySound(sounds[numOfSoundsToPlay - numOfSoundsRemainingToPlay]); 
    }

    public void setCurrentActivityType(int activity)
    {
        currentActivity = activity;
    }

    private void handleEventEnd()
    {
        resetActiviyState();
        gameController.loadNextActivity();

        // Play Sound and dont let activity start untill sound is played
        numOfSoundsRemainingToPlay -= 1;
        SoundController.PlaySound(sounds[numOfSoundsToPlay - numOfSoundsRemainingToPlay]);
        

        currentActivity = gameController.getCurrentActivity();

    }

    private void resetActiviyState()
    {
        if (currentActivity == GameController.LIGHTACTIVITY)
            resetBrightness();
        else
            resetTransforms();
        hasActivityEnded = false;
        stepsLeft = maxStepsAllowed;
        currentObject = null;
        currentObjectFinalTransform = null;
    }

    private void resetBrightness()
    {
        foreach (Light light in lights)
            light.intensity -= maxBrightness;
    }

    private void resetTransforms()
    {
        currentObject.transform.localPosition = currentObjectBaseTransform[0];
        currentObject.transform.localRotation = Quaternion.Euler(currentObjectBaseTransform[1]);
        currentObject.transform.localScale = currentObjectBaseTransform[2];

    }


    private float getDistance(Vector3 vector1, Vector3 vector2)
    {
        return Math.Abs(Vector3.Distance(vector1, vector2));
    }

    private Vector3[] calculateMoveFactor()
    {
        //if (currentObject == couldron)
        //    return calculateCouldronMoveFactor();
        //else
            return calculateDefaultMoveFactor(maxStepsAllowed);
    }

    private void handleGameState()
    {
        if (stepsLeft <= 0)
        {
            hasActivityEnded = true;
        }
        else
            stepsLeft--;
    }

    //private Vector3[] calculateCouldronMoveFactor()
    //{
    //    if (stepsLeft<=maxStepsAllowed/2)
    //        hasCouldronReachedMid = true;

    //    int steps = maxStepsAllowed;
    //    if (steps == 1) return calculateDefaultMoveFactor(steps);
    //    steps = (int)(steps / 2);

    //    if (hasCouldronReachedMid)
    //    {
    //        currentObjectBaseTransform = couldronMiddleTransform;
    //        currentObjectFinalTransform = couldronFinalTransform;
    //    }
    //    else
    //    {
    //        currentObjectBaseTransform = couldronBaseTransform;
    //        currentObjectFinalTransform = couldronMiddleTransform;
    //        steps += maxStepsAllowed % 2;
    //    }

    //    return calculateDefaultMoveFactor(steps);
    //}

    private Vector3[] calculateDefaultMoveFactor(int maxStepsAllowed)
    {
        Vector3[] moveFactor = new Vector3[3];
        for (int i = 0; i < moveFactor.Length; i++)
        {
            moveFactor[i] = (currentObjectFinalTransform[i] - currentObjectBaseTransform[i]) / maxStepsAllowed;        
        }
        return moveFactor;
    }





    // ---------------------------------------------------------------------------------------//

    private void scaleObject()
    {
        currentObject.transform.localScale = Vector3.Lerp(currentObject.transform.localScale, objectEndScale, movementSpeed * Time.deltaTime);
    }

    private void rotateObject()
    {
        //Debug.LogError(" errr " + currentObject.transform.rotation.eulerAngles + " , " + objectEndRotation);

        currentObject.transform.rotation = Quaternion.Lerp(Quaternion.Euler(currentObject.transform.rotation.eulerAngles), Quaternion.Euler(objectEndRotation), movementSpeed*Time.deltaTime);
    }

    void MoveObject()
    {
        //Debug.Log("ERR" + currentObject.transform.position + " " + objectEndPosition);
        currentObject.transform.position = Vector3.Lerp(currentObject.transform.position, objectEndPosition, movementSpeed * Time.deltaTime);
    }
}
