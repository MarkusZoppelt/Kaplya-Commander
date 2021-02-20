using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    #region Inspector
    [Header("References")]
    [SerializeField] internal AudioMixer audioMixer;
    [SerializeField] internal AudioSetting[] audioSettings;
    #endregion

    private Dictionary<string, AudioSetting> settingsDictionary;

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// Loads the user's audio settings and applies them
    /// </summary>
    private void Initialize()
    {
        settingsDictionary = new Dictionary<string, AudioSetting>();

        foreach (var setting in audioSettings)
        {
            setting.Initialize(this);
            settingsDictionary.Add(setting.groupName, setting);
        }
    }

    /// <summary>
    /// Returns whether a group is muted. Returns true if the group does not exist.
    /// </summary>
    public bool IsMuted(string groupName)
    {
        if (!settingsDictionary.ContainsKey(groupName))
        {
            Debug.LogWarning($"Trying to access the mute status of unknown audio group \"{groupName}\"");
            return true;
        }

        return settingsDictionary[groupName].IsMuted;
    }

    public void ToggleMute(string groupName)
    {
        if (!settingsDictionary.ContainsKey(groupName))
        {
            Debug.LogWarning($"Trying to access the mute status of unknown audio group \"{groupName}\"");
            return;
        }

        if (IsMuted(groupName))
        {
            settingsDictionary[groupName].Unmute();
        }
        else
        {
            settingsDictionary[groupName].Mute();
        }
    }

    public void SetVolume(string groupName, float volume)
    {
        audioMixer.SetFloat(groupName, volume);
    }
}
