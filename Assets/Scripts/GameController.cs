using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private const int ACTIVITYCOUNT = 4;
    private const int BOOKACTIVITY = 0;
    private const int COULDRONACTIVITY = 1;
    private const int FEATHERACTIVITY = 2;
    private const int LIGHTACTIVITY = 4;
    private const int TREEACTIVITY = 3;
    private int currentActivity;

    public GameObject[] bookObjects;
    public GameObject[] couldronObjects;
    public GameObject[] featherObjects;
    public GameObject[] lightObjects;
    public GameObject[] treeObjects;

    // Start is called before the first frame update
    void Start()
    {
        currentActivity = 0;
        showGameObjects(findCurrentGameObjects());
    }

    public void loadNextActivity()
    {
        hideGameObjects(findCurrentGameObjects());
        currentActivity = (currentActivity + 1) % ACTIVITYCOUNT;
        showGameObjects(findCurrentGameObjects());
    }

    // changed to public temporarily, removed static temporarily
    public void showGameObjects(GameObject[] currentActivityObjects)
    {
        foreach (GameObject gameObject in currentActivityObjects)
        {
            gameObject.SetActive(true);
        }
    }

    public GameObject[] findCurrentGameObjects()
    {
        GameObject[] currentGameObjects = null;
        switch (currentActivity) {
            case BOOKACTIVITY :
                currentGameObjects = bookObjects;
                break;
            case COULDRONACTIVITY:
                currentGameObjects = couldronObjects;
                break;
            case FEATHERACTIVITY:
                currentGameObjects = featherObjects;
                break;
            case LIGHTACTIVITY:
                currentGameObjects = lightObjects;
                break;
            case TREEACTIVITY:
                currentGameObjects = treeObjects;
                break;
        }

        return currentGameObjects;
    }

    // changed to public temporarily
    public void hideGameObjects(GameObject[] currentActivityObjects)
    {
        foreach (GameObject gameObject in currentActivityObjects)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal int getCurrentActivity()
    {
        return currentActivity;
    }
}
