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
    public string le;
    public string aah;
    public string la;
    public string ta;
    public string ma;
    public string ka;
    public string reps;
    public string Streamselected;
    public GameObject le_object;
    public GameObject aah_object;
    public GameObject la_object;
    public GameObject ta_object;
    public GameObject ma_object;
    public GameObject ka_object;
    public GameObject reps_object;
    public TMPro.TMP_Dropdown myDrop;
   

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

    public void SoundSelection()
    {
        le= le_object.GetComponent<Text>().text;
        aah = aah_object.GetComponent<Text>().text;
        la = la_object.GetComponent<Text>().text;
        ta = ta_object.GetComponent<Text>().text;
        ma = ma_object.GetComponent<Text>().text;
        ka = ka_object.GetComponent<Text>().text;
        Debug.Log(le);

  }

    public void RepetitionSelection()
    {

        reps = reps_object.GetComponent<Text>().text;
        Debug.Log(reps);
    }

    public void StreamSelection()
    {   if (myDrop.value == 0)
            Streamselected = "Long Stream";

        else
            Streamselected = "Short Stream";
        
        Debug.Log(Streamselected);
    }
}






