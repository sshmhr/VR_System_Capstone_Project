using UnityEngine.Android;
using UnityEngine;

// Edited By Suyash
// This script should be the first thing that runs inour project, so add it to our starting scene   
public class MicrophonePermissions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MicrophonePermission();
    }

    void MicrophonePermission()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
    }
}
