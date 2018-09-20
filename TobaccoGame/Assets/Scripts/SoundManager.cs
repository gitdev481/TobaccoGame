using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls all the sounds played in the game.
/// </summary>
public class SoundManager : MonoBehaviour
{
    #region SINGLETON PATTERN
    public static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SoundManager>();
                if (_instance == null)
                {
                    GameObject container = new GameObject("SoundManager");
                    _instance = container.AddComponent<SoundManager>();
                }

            }
            return _instance;
        }
    }
    #endregion

    //List to contain all the audio sources.
    public List<AudioSource> soundList = new List<AudioSource>();

    /// <summary>
    /// Plays a given sound if it exists in the soundList.
    /// </summary>
    /// <param name="soundName">The name of the sound to be played</param>
    public void PlaySound(string soundName)
    {
        bool soundExists = false;
        for (int i = 0; i < soundList.Count; i++)
        {
            if (soundList[i].name == soundName)
            {
                if (!soundList[i].isPlaying)
                {
                    soundList[i].Play();
                }
                soundExists = true;
            }
        }
        if (!soundExists)
            Debug.LogWarning("Debug: No sound of that name exists! (" + soundName + ")");
        if (soundList.Count == 0)
            Debug.LogWarning("Debug: Sound list is empty so no sounds will play. Please fill the soundlist for sounds to play");
    }

    /// <summary>
    /// Stops a given sound if it exists in the soundList;
    /// </summary>
    /// <param name="soundName">The name of the sound to be played</param>
    public void StopSound(string soundName)
    {
        for (int i = 0; i < soundList.Count; i++)
        {
            if (soundList[i].name == soundName)
            {
                soundList[i].Stop();
            }
        }
    }

    /// <summary>
    /// Plays a given sound once.
    /// </summary>
    public void PlayOneShot(string soundName)
    {
        for (int i = 0; i < soundList.Count; i++)
        {
            if (soundList[i].name == soundName)
            {
                soundList[i].PlayOneShot(soundList[i].GetComponent<AudioClip>());
            }
        }

        if (soundList.Count == 0)
            Debug.LogWarning("WARNING: Sound list is empty so no sounds will play. Please fill the soundlist for sounds to play");
    }

    /// <summary>
    /// Force stop all sounds.
    /// </summary>
    public void StopAllSounds()
    {
        for (int i = 0; i < soundList.Count; i++)
        {
            soundList[i].Stop();
        }
    }
}
