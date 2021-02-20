using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    private static AudioManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

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
            setting.Initialize();
            settingsDictionary.Add(setting.groupName, setting);
        }
    }

    /// <summary>
    /// Returns whether a group is muted. Returns true if the group does not exist.
    /// </summary>
    public static bool IsMuted(string groupName)
    {
        if (!instance.settingsDictionary.ContainsKey(groupName))
        {
            Debug.LogWarning($"Trying to access the mute status of unknown audio group \"{groupName}\"");
            return true;
        }

        return instance.settingsDictionary[groupName].IsMuted;
    }

    public static void SetVolume(string groupName, float volume)
    {
        instance.audioMixer.SetFloat(groupName, volume);
    }
}
