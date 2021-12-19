using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    PlayerData playerData = new PlayerData();
    public GameObject soundCheckboxes;

    private string getAllWords() {
        List<string> res = new List<string>();
        foreach (Toggle checkbox in soundCheckboxes.GetComponentsInChildren<Toggle>())
        {
            res.Add(checkbox.name);
        }
        return serializeString(res);
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("data"))
        {
            playerData = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("data"));
        }
        else {
            playerData.reps = 5;
            playerData.selectedWordList = getAllWords();
        }
    }

    
    private string serializeString(List<string> list) {
        string res = "";
        foreach (string str in list) {
            res = res + str + ",";
        }
        return res;
    }

    private string[] deserializeString(string str) {
        char[] delemeter = { ','};
        return str.Split(delemeter, StringSplitOptions.RemoveEmptyEntries);
    }
    public void saveWords(List<string> selectedWords)
    {
        playerData.selectedWordList = serializeString(selectedWords);
    }

    public void saveReps(int reps)
    {
        playerData.reps = reps;
    }

    public void saveGameState()
    {
        string SerializedData = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString("data", SerializedData);
        PlayerPrefs.Save();
    }

    public List<string> getSelectedSounds()
    {
        PlayerData data = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("data"));
        return new List<string>(deserializeString(data.selectedWordList));
    }

    public void deleteWords(List<string> deletedWords)
    {
        playerData.deletedWordList = serializeString(deletedWords);
    }

    public int getMaxStepsAllowed() { 
        return JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("data")).reps;
    }
}
