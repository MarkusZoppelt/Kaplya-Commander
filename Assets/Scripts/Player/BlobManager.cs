using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BlobType
{
    Conductor,
    Rock
}

public class BlobManager : MonoBehaviour
{
    #region Singleton
    private static BlobManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(this);
            instance = this;
            instance.Initialize();
        }
    }
    #endregion

    [Header("Prefabs")]
    [SerializeField] private BlobBase conductorBlobPrefab;
    [SerializeField] private BlobBase rockBlobPrefab;
    [Header("Type Images")]
    [SerializeField] private Sprite conductorSprite;
    [SerializeField] private Sprite rockSprite;
    [Header("Data")]
    [SerializeField] private List<BlobBase> blobsInField = new List<BlobBase>();

    private Dictionary<BlobType, int> reserves;

    private void Initialize()
    {
        reserves = new Dictionary<BlobType, int>();
        reserves.Add(BlobType.Conductor, 0);
        reserves.Add(BlobType.Rock, 0);
    }

    public static void AddBlobsToReserve(int amount, BlobType type)
    {
        Debug.Log($"Adding {amount} of {type} to reserves");
        instance.reserves[type] += amount;
    }

    public static int GetCurrentReserves(BlobType type)
    {
        return instance.reserves[type];
    }

    public static int GetCurrentReserveCount()
    {
        return GetCurrentReserves(BlobType.Rock) + GetCurrentReserves(BlobType.Conductor);
    }

    public static bool HasReserves(BlobType type)
    {
        return instance.reserves[type] > 0;
    }

    public static bool CallBlob(BlobType type)
    {
        if (!HasReserves(type))
        {
            return false;
        }

        instance.reserves[type]--;
        return true;
    }

    public static BlobBase GetBlobPrefab(BlobType type)
    {
        switch (type)
        {
            case BlobType.Conductor:
                return instance.conductorBlobPrefab;
            case BlobType.Rock:
                return instance.rockBlobPrefab;
            default:
                Debug.LogWarning("Trying to access unknown blob prefab of type " + type.ToString());
                return null;
        }
    }

    public static Sprite GetTypeSprite(BlobType type)
    {
        switch (type)
        {
            case BlobType.Conductor:
                return instance.conductorSprite;
            case BlobType.Rock:
                return instance.rockSprite;
            default:
                Debug.LogWarning("Trying to access unknown blob type sprite of type " + type.ToString());
                return null;
        }
    }

    public static void RememberBlob(BlobBase blob)
    {
        instance.blobsInField.Add(blob);
    }

    public static void ForgetBlob(BlobBase blob)
    {
        instance.blobsInField.Remove(blob);
    }

    public static void CallAllBlobsToTube(PneumaticTube tube)
    {
        foreach(var blob in instance.blobsInField)
        {
            blob.StartFollowing(tube.transform);
            blob.State = BlobState.GoingToTube;
        }

        instance.blobsInField.Clear();
    }

    public static int GetBlobsInFieldCount()
    {
        return instance.blobsInField.Count;
    }
}
