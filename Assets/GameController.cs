using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private const int ACTIVITYCOUNT = 5;
    private const int BOOKACTIVITY = 0;
    private const int COULDRONACTIVITY = 1;
    private const int FEATHERACTIVITY = 2;
    private const int LIGHTACTIVITY = 3;
    private const int TREEACTIVITY = 4;
    private int currentActivity;
    // Start is called before the first frame update
    void Start()
    {
        currentActivity = BOOKACTIVITY;
    }

    void loadNextActivity() {
        hideCurrentActivity();
        currentActivity = (currentActivity + 1) % ACTIVITYCOUNT ;
    }

    private void hideCurrentActivity()
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
