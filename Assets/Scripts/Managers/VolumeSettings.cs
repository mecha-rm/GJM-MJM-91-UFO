using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a file that holds the audio settings for a game.
public class VolumeSettings
{
    // instance of singleton
    private static VolumeSettings instance = null;

    // the background music volume.
    private float bgmVol = 1.0F;

    // the sound effect volume.
    private float sfxVol = 1.0F;

    // the voice volume.
    private float vceVol = 1.0F;

    // constructor
    private VolumeSettings()
    {
        Start();
    }

    // gets the instance
    public static VolumeSettings GetInstance()
    {
        // no instance generated
        if (instance == null)
        {
            // generates instance
            instance = new VolumeSettings();
        }

        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        // entered new scene, so set initial audio.
        OnSceneEnter();
    }

    // called to initialize audio levels when a new scene is entered.
    // call this everytime you enter a new scene.
    public void OnSceneEnter()
    {
        // grabs the audio listeners in the scene.
        AudioSource[] audios = Object.FindObjectsOfType<AudioSource>();

        // goes through all audio source components.
        foreach (AudioSource audio in audios)
        {
            if (audio.tag == "BGM") // background music
            {
                audio.volume *= bgmVol;
            }
            else if (audio.tag == "SFX") // sound effects
            {
                audio.volume *= sfxVol;
            }
            else if (audio.tag == "VCE") // voice
            {
                audio.volume *= vceVol;
            }

        }
    }

    // applies all the audio levels.
    public void ApplyAudioLevels(float bgmNew, float sfxNew, float vceNew)
    {
        // use the universal volume slider if all types are being changed at once (AudioListener.volume).
        // for this to work, you need to first undo the last audio change, then apply the new one.
        /*
         * e.g. assume the audio is a sound effect that has a base level of 0.7. Said audio is set to 60% first, then 80%.
         *  - 0.7 * 0.6 = 0.42 (change to 60% of base level)
         *  - 0.42 / 0.6 = 0.7 (return to 100%/base level)
         *  - 0.7 * 0.8 = 0.56 (change to 80%)
         */


        // set to max volume first, then set to proper volume.
        float bgmCurr = bgmVol; // background music volume current.
        float sfxCurr = sfxVol; // sound effect music volume current.
        float vceCurr = vceVol; // voice volume current.

        // overwrite values
        bgmVol = Mathf.Clamp01(bgmNew);
        sfxVol = Mathf.Clamp01(sfxNew);
        vceVol = Mathf.Clamp01(vceNew);


        // grabs the audio listeners in the scene.
        AudioSource[] audios = Object.FindObjectsOfType<AudioSource>();

        // goes through all audio source components.
        foreach (AudioSource audio in audios)
        {
            if (audio.tag == "BGM") // background music
            {
                audio.volume /= bgmCurr;
                audio.volume *= bgmVol;
            }
            else if (audio.tag == "SFX") // sound effects
            {
                audio.volume /= sfxCurr;
                audio.volume *= sfxVol;
            }
            else if (audio.tag == "VCE") // voice
            {
                audio.volume /= vceCurr;
                audio.volume *= vceVol;
            }

        }

    }

    // background music volume
    public float BackgroundMusicVolume
    {
        get
        {
            return bgmVol;
        }

        set
        {
            // checks if the value has changed.
            if(value != bgmVol) // changed
            {
                ApplyAudioLevels(value, sfxVol, vceVol);
            }
        }
    }

    // sound effect volume volume
    public float SoundEffectVolume
    {
        get
        {
            return sfxVol;
        }

        set
        {
            // checks if the value has changed.
            if (value != sfxVol) // changed
            {
                ApplyAudioLevels(bgmVol, value, vceVol);
            }
        }
    }

    // voice volume.
    public float VoiceVolume
    {
        get
        {
            return vceVol;
        }

        set
        {
            // checks if the value has changed.
            if (value != vceVol) // changed
            {
                ApplyAudioLevels(bgmVol, sfxVol, value);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
