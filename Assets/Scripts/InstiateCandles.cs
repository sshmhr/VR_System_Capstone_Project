using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstiateCandles : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject candlesBig;
    public GameObject candlesLong;
    public GameObject candlesMed;
    
    public GameObject candles1;
    public GameObject candles2;
    public GameObject candles3;



    public GameObject lamp;
    private float groundLevelY = 0.2673666f;
    private float groundLevelYLamp = 0.336f;
    private float twirlFactor = 0.5f; // This is used to make snake like circle
    private float twirlFactorLamp = 2f; // This is used to make snake like circle

    //0.3

    void Start()
    {

        // demo circles in ground
        // Need to adjust ground level according to the prefab object. TODO Maybe prefab.size.y/2
        InstantiateInCircle(prefab: candlesBig, location:new Vector3(0, groundLevelY, 0), howMany:20, radius:7.5f, yPosition : groundLevelY);

        //demo twirly circle in ground
        //InstantiateInTwirlyCircle(prefab: prefabToInstantiate, location:new Vector3(0, groundLevelY, 0), howMany:20, radius:7.5f, yPosition : groundLevelY);

        //instanciate 4 lamps in ground
        //InstantiateInCircle(prefab: lamp, location: new Vector3(0, groundLevelYLamp, 0), howMany: 5, radius: 5f, yPosition: groundLevelYLamp);

        //Twirly  4 lamps in ground
        InstantiateInCircle(prefab: lamp, location: new Vector3(0, groundLevelYLamp, 0), howMany: 5, radius: 5f, yPosition: groundLevelYLamp);



        // floating harry porter like demo
        InstantiateInCircle(prefab: candlesMed, location: new Vector3(4, 5,1), howMany: 10, radius: 0.5f, yPosition: groundLevelY);
        InstantiateInTwirlyCircle(prefab: candles3, location: new Vector3(-3, 5, 1.5f), howMany: 10, radius: 0.5f, yPosition: groundLevelY);


        InstantiateInTwirlyCircle(prefab: candlesMed, location: new Vector3(0.04f,  4.5f, 5), howMany: 10, radius: 0.5f, yPosition: groundLevelY);
        InstantiateInCircle(prefab: candles3, location: new Vector3(-0.04f,  4.5f, -5f), howMany: 10, radius: 0.5f, yPosition: groundLevelY);


        //-0/04, -5
        //
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // https://answers.unity.com/questions/1068513/place-8-objects-around-a-target-gameobject.html
    /// <summary>
    ///     Instantiates prefabs around center splited equality. 
    ///     The number of times indicated in <see cref="howMany" /> var is
    ///     the number of parts will be the circle cuted, with taking as a center the location,
    ///     and adding radius from it
    /// </summary>
    /// <param name="prefab">The object it will be intantiated</param>
    /// <param name="location">The center point of the circle</param>
    /// <param name="howMany">The number of parts the circle will be cut</param>
    /// <param name="radius">
    ///     The margin from center, if your center is at (1,1,1) and your radius is 3 
    ///     your final position can be (4,1,1) for example
    /// </param>
    /// <param name="yPosition">The yPostion for the instantiated prefabs</param>
    public void InstantiateInCircle(GameObject prefab, Vector3 location, int howMany, float radius, float yPosition)
    {
        float angleSection = Mathf.PI * 2f / howMany;
        for (int i = 0; i < howMany; i++)
        {
            float angle = i * angleSection;
            Vector3 newPos = location + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            //newPos.y = yPosition;
            Debug.Log(newPos);
            Instantiate(prefab, newPos, prefab.transform.rotation);
        }
    }

    

    // NOT working as expected. As in some place we have to change x and in some place y
    public void InstantiateInTwirlyCircle(GameObject prefab, Vector3 location, int howMany, float radius, float yPosition)
    {
        float angleSection = Mathf.PI * 2f / howMany;
        int counter = 0;
        for (int i = 0; i < howMany; i++)
        {
            float angle = i * angleSection;
            Vector3 newPos = location + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            //newPos.y = yPosition;
            // change z direction alternatively to make instantiation like snake
            Debug.Log(counter % 2 == 0);
            if (counter % 2 == 0)
            {
                newPos.z = newPos.z + twirlFactor;
                newPos.x = newPos.x + twirlFactor;
            }
            else
            {
                newPos.z = newPos.z - twirlFactor;
                newPos.x = newPos.x - twirlFactor;
            }
            Debug.Log(newPos);
            Instantiate(prefab, newPos, prefab.transform.rotation);
            counter += 1;
        }
    }
}
