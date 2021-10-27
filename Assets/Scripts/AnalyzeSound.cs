using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public abstract class AnalyzeSound : MonoBehaviour
{

    //necessary variables.
    private const int QSamples = 2048;//1024, 512, 2048
    private const float Threshold = 0.001f;//0.02
    private float[] spectrum;
    private float[] _samples;
    private float fSample;
    private const float RefValue = 0.1f;
    private float lowdB = 0f;
    private float lowdBRange = 10f;
    private float lastdB = -80.0f;
    private bool pitchInRange = false;
    private float lastPitch;
    protected float currentPitch;
    private float timeSilent;//silent time to detect when someone has stopped talking
    private AudioSource voice;

    public int attempt = 1;
    protected int totalAttempts = 2;//needs to be overriden if there is more attempts required

    bool listeningForEnd;//is listening for the user to stop making sounds
    private const float silenceTime = 2;

    [HideInInspector]
    public UnityEvent attemptComplete;
    [HideInInspector]
    public UnityEvent allAttemptComplete;

    //wait for start button to be pressed before analysing sound
    protected bool isListening = false;

    //option to toggle the microphone listenter on startup or not
    public bool startMicOnStartup = true;

    //allows start and stop of listener at run time within the unity editor
    public bool stopMicListener = false;
    public bool startMicListener = false;

    private bool micListenerOn = false;

    //public to allow temporary listening over the speakers if you want of the mic output
    //but internally it toggles the output sound to the speakers of the audiosource depending
    //on if the microphone listener is on or off
    public bool disableOutputSound = false;

    //make an audio mixer from the "create" menu, then drag it into the public field on this script.
    //double click the audio mixer and next to the "groups" section, click the "+" icon to add a 
    //child to the master group, rename it to "microphone".  Then in the audio source, in the "output" option, 
    //select this child of the master you have just created.
    //go back to the audiomixer inspector window, and click the "microphone" you just created, then in the 
    //inspector window, right click "Volume" and select "Expose Volume (of Microphone)" to script,
    //then back in the audiomixer window, in the corner click "Exposed Parameters", click on the "MyExposedParameter"
    //and rename it to "Volume"
    public AudioMixer masterMix;

    private float timeSinceRestart = 0f;

    // Public variables for the Editor
    //variable to score*** change for collecting coins
    [HideInInspector]
    public int scoreCoins = 0;
    //Display Score on the player Canvas
    public TextMeshProUGUI scoreText;


    // Variables to change within each class that inherits this
    [HideInInspector]
    public float highestPitch = 500f; // highest possible pitch is different for each scene

    // Method for detemining pitch in each scene
    public abstract int AnalyzePitch();

    /// <summary>
    /// called when the end of speaking has completed.
    /// </summary>
    /// <param name="attempt">the attempt number, starts at 1 ;)</param>
    public abstract void HandleAttemptCompletion();

    // Start is called before the first frame update
    public virtual void Start()
    {
        _samples = new float[QSamples];

        // MicrophonePermission();
        spectrum = new float[QSamples];
        fSample = AudioSettings.outputSampleRate;

        //pitchDetector.onPitchDetected.AddListener(GetPitchHPVD);

        foreach (var microphone in Microphone.devices)
        {
            Debug.LogWarning(microphone);
        }


        // Start listener
        if (startMicOnStartup)
        {
            StartMicrophoneListener();
        }
    }

    public virtual void Update()
    {
        Debug.LogError(isListening + " is Listening ? ");
        //can use these variables that appear in the inspector, or can call the public functions directly from other scripts
        if (stopMicListener)
        {
            StopMicrophoneListener();
        }
        if (startMicListener)
        {
            StartMicrophoneListener();

        }
        //reset to false
        stopMicListener = false;
        startMicListener = false;

        if (listeningForEnd && timeSilent >= silenceTime)
        {
            HandleAttemptCompletion();
            timeSilent = 0;
        }

        //must run in update otherwise it doesnt seem to work
        MicrophoneIntoAudioSource(micListenerOn);

        //can choose to unmute sound from inspector if desired
        DisableSound(!disableOutputSound);
    }
    public void AttemptBegin()
    {
        StartListening();
    }

    protected void AttemptComplete()
    {
        listeningForEnd = false;
        attemptComplete.Invoke();
        StopListening();
    }

    protected void AllAttemptsComplete()
    {
        listeningForEnd = false;
        allAttemptComplete.Invoke();
        StopListening();
    }

    protected void OnDestroy()
    {
        StopMicrophoneListener();
    }

    /// <summary>
    /// Takes the input voice and measures it's frequency.
    /// </summary>
    protected float GetPitch(float lowestFreq, float highestFreq)
    {



        // Solution: Resample raw voice clip data to range between where two ABS(val) > 0.0001 values occur two in a row
        // Possible problem: noting that the audio analysis is attached to the audiosource, not the audioclip
        // Can we change the audioclip while voice.Play() is running? or do we need to voice.Stop() in order to change the clip? would that work in that case?
        // Unfortunately, this wouldn't work for long tones as the max value appears to decrease over longer time. DAMMIT

        // Looks like the only way is the cheat way: if the pitch dips to zero while sound is still happening,
        // then assign latest pitch value until volume is zero (i.e. below a certain threshold - possibly lowest recorded above -160)
        // Unfortunately, this may require some manually adjusting the threshold.
        // The inherent issue is in the sampled raw voice clip data: it get messed up very easily it seems
        // Parameters to adjust: QSamples, lowdBRange

        float pitchValue = 0f;

        
        //if no mic found, skip
        if (!(Microphone.GetPosition(null) > 0))
        {
            Debug.LogError("no Microphone detected");
            return 0f;
        }
        
        voice.GetOutputData(_samples, 0); // fill array with samples
        float sum = 0;
        for (int i = 0; i < QSamples; i++)
        {
            sum += _samples[i] * _samples[i]; // sum squared samples
        }
        float RmsValue = Mathf.Sqrt(sum / QSamples); // rms = square root of average
        float DbValue = 20 * Mathf.Log10(RmsValue / RefValue); // calculate dB
        lastdB = DbValue;//update the dB value for detecting when talking ceases
        if (DbValue < -160f) DbValue = -160f; // clamp it to -160dB min
        else
        {
            if (DbValue < lowdB)
            {
                lowdB = DbValue;
            }


        }

        // get sound spectrum
        voice.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0;
        var maxN = 0;

        for (int i = 0; i < QSamples; i++)
        {
            // find max 
            if (!(spectrum[i] > maxV) || !(spectrum[i] > Threshold))
                continue;

            maxV = spectrum[i];

            maxN = i; // maxN is the index of max
        }

        // pass the index to a float variable
        float freqN = maxN;

        if (maxN > 0 && maxN < QSamples - 1)
        {
            // interpolate index using neighbours
            var dL = spectrum[maxN - 1] / spectrum[maxN];
            var dR = spectrum[maxN + 1] / spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }

        // convert index to frequency
        pitchValue = freqN * (fSample / 2) / QSamples;
        //Debug.Log("pitchValue: " + pitchValue + "\nlowdB = " + lowdB);

        //****This is becoming an unsolvable massive issue; so we cheat!
        //**** check start of pitch, if mic loses the pitch, reassign the old value (i.e. oldPitch) from when it was working
        //**** then check for when the user stops, at which point reassign oldPitch back to 0

        if (pitchValue > lowestFreq && pitchValue < highestFreq)
        {
            pitchInRange = true;
            lastPitch = pitchValue;
        }
        else if ((pitchValue == 0f) || (pitchValue > highestPitch))
        {
            if ((DbValue > (lowdB + lowdBRange)) && pitchInRange)
            {
                pitchValue = lastPitch;
                timeSilent = 0;
                listeningForEnd = true;
            }
            else
            {
                lastPitch = pitchValue;
                pitchInRange = false;
                timeSilent += Time.deltaTime;
            }
        }
        return pitchValue;
    }


    protected void StartMicrophoneListener()
    {
        Debug.Log("StartMicrophoneListener()");
        //start the microphone listener
        micListenerOn = true;
        //disable sound output (dont want to hear mic input on the output!)
        disableOutputSound = true;
        //reset the audiosource
        AttemptBegin();

        RestartMicrophoneListener();

        //pitchDetector.Record = true;
    }

    //stops everything and returns audioclip to null
    protected void StopMicrophoneListener()
    {
        //stop the microphone listener
        micListenerOn = false;
        //reenable the master sound in mixer
        disableOutputSound = false;
        //remove mic from audiosource clip
        voice.Stop();
        voice.clip = null;

        //pitchDetector.Record = false;

        Microphone.End(null);
    }

    //controls whether the volume is on or off, use "off" for mic input (dont want to hear your own voice input!) 
    //and "on" for music input
    protected void DisableSound(bool SoundOn)
    {

        float volume = 0;

        if (SoundOn)
        {
            volume = 0.0f;
        }
        else
        {
            volume = -80.0f;
        }

        masterMix.SetFloat("Volume", volume);
    }

    // restart microphone removes the clip from the audiosource
    protected void RestartMicrophoneListener()
    {

        voice = GetComponent<AudioSource>();

        //remove any soundfile in the audiosource
        voice.clip = null;

        timeSinceRestart = Time.time;

    }

    //puts the mic into the audiosource
    protected void MicrophoneIntoAudioSource(bool MicrophoneListenerOn)
    {
    
        if (MicrophoneListenerOn)
        {
            //pause a little before setting clip to avoid lag and bugginess
            if (Time.time - timeSinceRestart > 0.5f && !Microphone.IsRecording(null))
            {
                voice.clip = Microphone.Start(null, true, 1, 48000); // Oculus quest appears to sample at 48kHz
                voice.loop = true;
                voice.mute = false;

                //wait until microphone position is found (?)
                while (!(Microphone.GetPosition(null) > 0))
                {
                    Debug.Log("mic not found");
                }



                voice.Play();
            }
            else {
                AttemptBegin();
            }
        }
    }

    /// <summary>
    /// Allows the sound to be analyzed
    /// </summary>
    public void StartListening()
    {
        Debug.Log("started listening");
        isListening = true;
    }

    /// <summary>
    /// Disables the microphone listening
    /// </summary>
    public void StopListening()
    {
        isListening = false;
    }
}
