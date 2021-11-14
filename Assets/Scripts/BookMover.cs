using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static System.Math;

// Added by Anjali, Amit Oct 28,2021
// Updated and altered by suyash on 11-11 21

// Try not to hardcode stuff
// make things public only if necessary
// comment the unnecessary print statments before pushing the code

public class BookMover : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject book;
    public GameObject couldron;
    public GameObject feather;

    public float movementSpeed = 2;
    private int currentActivity = 1;

    private GameObject currentObject;

    public Vector3[] bookFinalTransform = new Vector3[3];
    public Vector3[] couldronFinalTransform = new Vector3[3];
    public Vector3[] featherFinalTransform = new Vector3[3];

    public Vector3[] bookBaseTransform = new Vector3[3];
    public Vector3[] couldronBaseTransform = new Vector3[3];
    public Vector3[] featherBaseTransform = new Vector3[3];

    private Vector3[] currentObjectFinalTransform, currentObjectBaseTransform;
    private Vector3[] cuyrrentObjectStep = new Vector3[3];

    //private Vector3[][] basePositions; // default position of object. Can be overidden in start. Might come handy late
    //                                   // if we have to reset the object position after shuffling activities.
    //                                   // Useful to move with animation
    //                                   // Override them everytime you need to move the object. We can reset the start position whenever we need to move
    public bool move = false;
    public float maxStepsAllowed = 10;
    private Vector3 objectStartPosition; // to be set by the SetMovement Function
    private Vector3 objectEndRotation;
    private Vector3 objectEndScale;
    private Vector3 objectEndPosition; // to be set by the SetMovement Function


    public int Movementspeed; //Steps to complete 1 movement animation

    // Update is called once per frame

    public void setCurrentActivityType( int activity) {
        currentActivity = activity;
    }

    void Update()
    {
        if (currentObject && currentObjectFinalTransform != null && Math.Abs(Vector3.Distance(currentObject.transform.position, currentObjectFinalTransform[0])) < 0.01)
            handleEventEnd();
        else CheckMovement();
    }

    private void handleEventEnd()
    {
        Debug.LogError("end");
    }

    public void SetMovement()
    {
        //Debug.Log("Setting Movement");
        //Debug.Log("bookStartPosition");
        //Debug.Log(bookStartPosition);
        //Debug.Log("bookEndPosition");
        //Debug.Log(bookEndPosition);
        detectCurrentObject();
        move = true;
        Vector3[] moveFactor = calculateMoveFactor();
        objectEndPosition = currentObject.transform.position + moveFactor[0];
        objectEndRotation = currentObject.transform.rotation.eulerAngles + moveFactor[1];
        //Debug.LogError(Vector3.Distance(currentObject.transform.position, currentObjectFinalTransform[0]));
        objectEndScale = currentObject.transform.localScale + moveFactor[2];
    }

    private Vector3[] calculateMoveFactor()
    {
        Vector3[] moveFactor = new Vector3[3];
        for (int i = 0; i < moveFactor.Length; i++) {
            moveFactor[i] = (currentObjectFinalTransform[i] - currentObjectBaseTransform[i]) / maxStepsAllowed;
            //Debug.LogError(" a " + i + " " + moveFactor[i]);
        }
        return moveFactor;
    }

    private void detectCurrentObject()
    {
        switch (currentActivity) {
            case 0:
                currentObjectBaseTransform = bookBaseTransform;
                currentObjectFinalTransform = bookFinalTransform;
                currentObject = book;
                break;
            case 1:
                currentObjectBaseTransform = couldronBaseTransform;
                currentObjectFinalTransform = couldronFinalTransform;
                currentObject = couldron;
                break;
            case 2:
                currentObjectBaseTransform = featherBaseTransform;
                currentObjectFinalTransform = featherFinalTransform;
                currentObject = feather;
                break;
        }
    }

    void CheckMovement()
    {
        
        // Check and stop the movement with help of StopMovement function
        //else move the object with animation
        if (currentObject == null) return;
        //Debug.Log("ERR" + currentObject.transform.position + " " + objectEndPosition);
        if (Math.Abs(Vector3.Distance(currentObject.transform.position, objectEndPosition)) > 0.01)
        {
            Debug.Log("ERR" + currentObject.transform.position + " " + objectEndPosition);
            MoveObject();
        }

        if (Math.Abs(Vector3.Distance(currentObject.transform.rotation.eulerAngles, objectEndRotation)) > 0.01)
        {
            Debug.LogError("currentObject.transform.rotation.eulerAngles " + currentObject.transform.rotation.eulerAngles);
            Debug.LogError(" objectEndPosition " + objectEndRotation);
            rotateObject();
        }

        if (Math.Abs(Vector3.Distance(currentObject.transform.localScale, objectEndScale))>0.01) 
        {
            //Debug.LogError("Dist " + Vector3.Distance(currentObject.transform.localScale, objectEndScale));
            scaleObject();
        }


    }

    private void scaleObject()
    {
        currentObject.transform.localScale = Vector3.Lerp(currentObject.transform.localScale, objectEndScale, movementSpeed * Time.deltaTime);
    }

    private void rotateObject()
    {
        Debug.LogError(" errr " + currentObject.transform.rotation.eulerAngles + " , " + objectEndRotation);

        currentObject.transform.rotation = Quaternion.Lerp(Quaternion.Euler(currentObject.transform.rotation.eulerAngles), Quaternion.Euler(objectEndRotation), movementSpeed*Time.deltaTime);
    }

    void MoveObject()
    {
        Debug.Log("ERR" + currentObject.transform.position + " " + objectEndPosition);
        currentObject.transform.position = Vector3.Lerp(currentObject.transform.position, objectEndPosition, movementSpeed * Time.deltaTime);
    }
}
