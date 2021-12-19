using UnityEngine;
using UnityEngine.EventSystems;
//whenever we change the scenes in unity, we need to use this package
using UnityEngine.UI;

public class PinScriptValidate : MonoBehaviour
{
    public Slider repSlider;
    public Text reps;
    public string passwordEntered;
    public Text inputField;
    //public GameObject textDisplay;
    public GameObject soundCheckboxes;
    public GameObject streamCheckboxes;
    public GameObject objecttoActivate;
    public GameObject objecttoDeactivate;
    public GameObject objecttoActivate1;
    public GameObject objecttoDeactivate1;

    public TMPro.TMP_Dropdown myDrop;

    public void getInputKey()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        inputField.text += name;
    }
    public void ValidatePin()
    {

        passwordEntered = inputField.text;
        inputField.text = "";
        string password = "12345";
        if (passwordEntered==password)
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
        Toggle[] checkboxList = soundCheckboxes.GetComponentsInChildren<Toggle>();
        Debug.Log(checkboxList.Length);
        foreach (Toggle checkbox in checkboxList) {
            Debug.Log(checkbox.isOn);
            Debug.Log(checkbox.name);
        }

  }

    public void RepetitionSelection()
    {
        reps.text = "" + repSlider.value;
    }

    public void StreamSelection()
    {
        Toggle[] checkboxList = streamCheckboxes.GetComponentsInChildren<Toggle>();
        if (checkboxList[0].isOn) Debug.Log("Longstream");
        if (checkboxList[1].isOn) Debug.Log("multisyllabic");

    }
}






