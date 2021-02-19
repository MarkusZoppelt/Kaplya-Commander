using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlobManagerDisplay : MonoBehaviour
{
    [Header("References - UI")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image blobTypeImage;
    [SerializeField] private TextMeshProUGUI blobsInReserveText;
    [SerializeField] private TextMeshProUGUI blobsInFieldText;
    [SerializeField] private Button switchButton;
    [SerializeField] private Button plusButton;
    [SerializeField] private Button minusButton;
    [SerializeField] private Button closeButton;
    [Header("References - Scene")]
    [SerializeField] private PneumaticTube tube;

    private PlayerController player;
    private BlobType currentBlobType;

    private void Update()
    {
        blobsInReserveText.text = BlobManager.GetCurrentReserves(currentBlobType).ToString();
        blobsInFieldText.text = $"{player.CurrentFollowerAmount}/{player.MaxFollowers}";
        blobTypeImage.sprite = BlobManager.GetTypeSprite(currentBlobType);
    }

    public void ActivateForPlayer(PlayerController player)
    {
        canvas.worldCamera = Camera.main;
        this.player = player;
        player.IsInMenu = true;
        currentBlobType = player.CurrentBlobType;
        UpdateButtons();
    }

    public void Deactivate()
    {
        player.IsInMenu = false;
        gameObject.SetActive(false);
    }

    private void UpdateButtons()
    {
        if (BlobManager.HasReserves(currentBlobType) && player.CurrentFollowerAmount < player.MaxFollowers)
        {
            plusButton.interactable = true;
        }
        else
        {
            plusButton.interactable = false;
        }

        if (player.CurrentFollowerAmount > 0)
        {
            minusButton.interactable = true;
        }
        else
        {
            minusButton.interactable = false;
        }
    }

    public void SwitchBlobType()
    {
        BlobManager.AddBlobsToReserve(BlobManager.GetBlobsInFieldCount(), currentBlobType);
        BlobManager.CallAllBlobsToTube(tube);
        player.SendAllBlobsToTube(tube);
        currentBlobType = (BlobType) (((int)currentBlobType + 1) % Enum.GetNames(typeof(BlobType)).Length);
        player.CurrentBlobType = currentBlobType;
        UpdateButtons();
    }

    public void AddBlob()
    {
        if (BlobManager.CallBlob(currentBlobType))
        {
            tube.SpawnBlob(currentBlobType, player, UpdateButtons);
        }
    }

    public void RemoveBlob()
    {
        player.SendBlobToTube(tube);
        BlobManager.AddBlobsToReserve(1, currentBlobType);
        UpdateButtons();
    }
}
