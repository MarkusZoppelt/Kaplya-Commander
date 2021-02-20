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

    /// <summary>
    /// Load and apply the mute settings
    /// </summary>
    public void Initialize()
    {
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
        AudioManager.SetVolume(groupName, -80f);
        PlayerPrefs.SetInt($"{groupName}IsMuted", -1);
    }

    public void Unmute()
    {
        AudioManager.SetVolume(groupName, volume);
        PlayerPrefs.SetInt($"{groupName}IsMuted", 1);
    }
}