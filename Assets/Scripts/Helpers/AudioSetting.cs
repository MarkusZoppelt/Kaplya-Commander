using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Regulates the volume of a mixer group.
/// Persists the volume settings via PlayerPrefs.
/// Total volume is split into user- and game-volume.
/// </summary>
[System.Serializable]
public class AudioSetting
{
    #region Inspector
    [Header("Settings")]
    [SerializeField] internal string groupName;
    [SerializeField] private float volume = 0f;
    [SerializeField] private bool defaultIsMuted = false;
    #endregion

    private bool currentIsMuted;
    public bool IsMuted { get { return currentIsMuted; } }

    private AudioManager manager;

    /// <summary>
    /// Load and apply the mute settings
    /// </summary>
    public void Initialize(AudioManager manager)
    {
        this.manager = manager;

        // A value lower than 0 means mute
        if (PlayerPrefs.GetInt($"{groupName}IsMuted", 0) < 0)
        {
            Mute();
        }
        else
        {
            Unmute();
        }
    }

    public void Mute()
    {
        manager.SetVolume(groupName, -80f);
        currentIsMuted = true;
        PlayerPrefs.SetInt($"{groupName}IsMuted", -1);
    }

    public void Unmute()
    {
        manager.SetVolume(groupName, volume);
        currentIsMuted = false;
        PlayerPrefs.SetInt($"{groupName}IsMuted", 1);
    }
}