using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static System.Math;

// Added by Anjali, Amit Oct 28,2021

public class BookMover : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject book;
    public bool move = true;

    public float maxMovementAllowed = 20;


    private Vector3 basePosition; // default position of object. Can be overidden in start. Might come handy late
    // if we have to reset the object position after shuffling activities.
    // Useful to move with animation
    // Override them everytime you need to move the object. We can reset the start position whenever we need to move
    public Vector3 bookStartPosition; // to be set by the SetMovement Function
    public Vector3 bookEndPosition; // to be set by the SetMovement Function


    public int Movementspeed; //Steps to complete 1 movement animation
    void Start()
    {
        //SetMovement(moveFactor: 1, speed: 2);

    }

    // Update is called once per frame
    void Update()
    {
        CheckMovement();
    }

    public void SetMovement(float moveFactor, int speed)
    {
        Debug.Log("Setting Movement");
        Movementspeed = speed;
        bookEndPosition = transform.position + new Vector3(moveFactor, 0, 0);
        Debug.Log("bookStartPosition");
        Debug.Log(bookStartPosition);
        Debug.Log("bookEndPosition");
        Debug.Log(bookEndPosition);

    }


    void CheckMovement()
    {
        // Check and stop the movement with help of StopMovement function
        //else movethebook with animation
        if (Round(transform.position.x, 2) == Round(bookEndPosition.x, 2))
        {
            //Debug.Log("Stationary");
        }
        else
        {
            MoveBook();
            Debug.Log("Moving");

        }

    }

    void MoveBook()
    {
        bookStartPosition = transform.position;
        transform.position = Vector3.Lerp(transform.position, bookEndPosition, Movementspeed * Time.deltaTime);
    }
}
