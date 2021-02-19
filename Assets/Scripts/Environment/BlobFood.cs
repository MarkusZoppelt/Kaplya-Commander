using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlobFood : CarryObject
{
    [Header("Blob Food")]
    [SerializeField] private BlobType type;
    [SerializeField] private int amount;
    [SerializeField] private TextMeshProUGUI amountDisplay;

    internal override void Awake()
    {
        base.Awake();
        amountDisplay.text = amount.ToString();
    }

    public override void OnSuckedIntoTube()
    {
        BlobManager.AddBlobsToReserve(amount, type);
        base.OnSuckedIntoTube();
    }
}
