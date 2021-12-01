using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//whenever we change the scenes in unity, we need to use this package
using UnityEngine.UI;
using System.Linq;
public class PinScriptValidate : MonoBehaviour
{
    public string theName;
    public GameObject inputField;
    //public GameObject textDisplay;
    public GameObject objecttoActivate;
    public GameObject objecttoDeactivate;
    public GameObject objecttoActivate1;
    public GameObject objecttoDeactivate1;
    // gameobjects to be activated or dectivated based on the password entered
    public string light1;
    public string book1;
    public string feather1;
    public string couldron1;
    public string plant1;
    public GameObject light_object;
    public GameObject book_object;
    public GameObject feather_object;
    public GameObject plant_object;
    public GameObject couldron_object;
    public void ValidatePin()
    {

        theName = inputField.GetComponent<Text>().text;
        
       
        if(theName=="12345")
        {
            objecttoActivate.SetActive(false);
            objecttoDeactivate.SetActive(true);
          //this is to activate and deactivate the objects by script
          //once the pin entered is correct, the current page will be deactivated and the control will go to the next screen changeactivites and this screen will be activated
        }
        else
        {
            objecttoActivate1.SetActive(false);
            objecttoDeactivate1.SetActive(true);
          //if the pin entered is wrong, then the control will return back to main menu and main menu screen object will be activated
        }
    }

    public void ChangeActivity()
    {
        light1= light_object.GetComponent<Text>().text;
        book1 = book_object.GetComponent<Text>().text;
        feather1 = feather_object.GetComponent<Text>().text;
        plant1 = plant_object.GetComponent<Text>().text;
        couldron1 = couldron_object.GetComponent<Text>().text;
     //this is to store the order number for each activity which is to be set by the clinicians  

  }
}




