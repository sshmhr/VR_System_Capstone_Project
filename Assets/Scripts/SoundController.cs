using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sound
{
    public string name;
    public string soundPath; // add the path after media. All media files go here
    public System.DateTime modifiedDate;
    public string soundType; // Long or short.
    public string description;
}

[System.Serializable]
public class SoundController : MonoBehaviour
{
    // Start is called before the first frame update
    // TODO 
    // List is not a good choice. We might need to store more info like type of sound, name, description later.
    // Switch away from LIST TO a class or JSON whichever is easier
    private  List<string> soundData = new List<string>();
    private string soundDirectory = "AudioFiles";
    private AudioSource audioSource;
    public DataManager dataManager;
    public void SetUpSound()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.LogError(audioSource);
        foreach (string soundName in dataManager.getSelectedSounds()) {
            Debug.Log(soundName);
            soundData.Add($"{soundDirectory}/{soundName}");
        }
        // to (re)create the sound json file
        
        soundData.Add($"{soundDirectory}/say ka");
        //soundData.Add($"{soundDirectory}/say la");
        //soundData.Add($"{soundDirectory}/say ma");
        //soundData.Add($"{soundDirectory}/say ta");

        
        

        Debug.LogError(soundData);

        //soundData.Add(
        //     new Sound()
        //     {
        //         name = "ka",
        //         soundPath = "say ka",
        //         modifiedDate = "1998/03/03",
        //         soundType="short", // I am not sure what it is called. So assuming short and long now
        //         description = "put the back of your tongue against the top of your mouth towards the back on what is called your soft palate. With your tongue in this position, draw air through your mouth and release it by lowering your tongue", // can bse used to store hint or some message that can be used in future, Just a demo
        //     }
        //     ); 


    }
    //public void AddSound(string name,string soundPath, string modifiedDate, string soundType , string description)
    //{
    //    soundData.Add(
    //         new Sound()
    //         {
    //             name = name,
    //             soundPath = soundPath,
    //             modifiedDate = modifiedDate,
    //             soundType = soundType, // I am not sure what it is called. So assuming short and long now
    //             description = description

    //         }
    //         );
    //}

    public void  PlaySound(string soundPath)
    {

        AudioClip currentClip = Resources.Load<AudioClip>(soundPath);
        Debug.Log(currentClip);
        audioSource.clip = currentClip;
        audioSource.PlayOneShot(audioSource.clip);
    }
    public int GetSoundCount()
    {
        return soundData.Count;
    }

    public List<string> GetNSound(int n=-1)
    {
        //if n was not specified return everything
        if(n==-1)n = soundData.Count;
        if (n >= soundData.Count) n = soundData.Count;
        return soundData.GetRange(0, n);
    }

    public bool hasSoundFinishedPlaying()
    {
        return !audioSource.isPlaying;
    }
}
