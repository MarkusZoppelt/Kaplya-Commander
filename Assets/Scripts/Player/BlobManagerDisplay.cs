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
    [Header("References - Scene")]
    [SerializeField] private PneumaticTube tube;

    private PlayerController player;
    private BlobType currentBlobType;

    private void Awake()
    {
        switchButton.onClick.AddListener(SwitchBlobType);
        plusButton.onClick.AddListener(AddBlob);
        minusButton.onClick.AddListener(RemoveBlob);
    }

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
        currentBlobType = player.CurrentBlobType;
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        // TODO check if buttons should be pressable
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
        UpdateButtons();
    }
}
