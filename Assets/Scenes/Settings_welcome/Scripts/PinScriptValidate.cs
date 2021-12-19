using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//whenever we change the scenes in unity, we need to use this package
using UnityEngine.UI;

public class PinScriptValidate : MonoBehaviour
{
    public Slider repSlider;
    public Text reps;
    private string passwordEntered;
    public Text inputField;
    //public GameObject textDisplay;
    public GameObject soundCheckboxes;
    public GameObject streamCheckboxes;
    public GameObject objecttoActivate;
    public GameObject objecttoDeactivate;
    public GameObject objecttoActivate1;
    public GameObject objecttoDeactivate1;
    public DataManager dataManager; 
    public TMPro.TMP_Dropdown myDrop;
    private void Start()
    {
    }

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
        List<string> selectedWords = new List<string>();
        List<string> deletedWords = new List<string>();
        foreach (Toggle checkbox in checkboxList) {
            if (checkbox.isOn)
            {
                selectedWords.Add(checkbox.name); 
            }
            else {
                deletedWords.Add(checkbox.name);
                //dataManager.deleteWord("_checkbox_" + checkbox.name);
            }
        }
        dataManager.saveWords(selectedWords);
        dataManager.deleteWords(deletedWords);
    }

    public void RepetitionSelection()
    {
        reps.text = "" + (int)repSlider.value;
        dataManager.saveReps((int)repSlider.value);
    }

    public void StreamSelection()
    {
        Toggle[] checkboxList = streamCheckboxes.GetComponentsInChildren<Toggle>();
        bool isLongStream = false;
        if (checkboxList[0].isOn) isLongStream = true;
        if (isLongStream) dataManager.saveReps(1);



    }
}






