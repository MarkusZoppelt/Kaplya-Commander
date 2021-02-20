using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI fieldBlobCountText;
    [SerializeField] private TextMeshProUGUI reserveBlobCountText;
    [SerializeField] private TextMeshProUGUI followingBlobCountText;
    [SerializeField] private Sprite followingBlobTypeSprite;
    [SerializeField] private Destructable destructablePlayer;
    [SerializeField] private PlayerController player;

    public void Update()
    {
        currentHealthText.text = destructablePlayer.CurrentHealth.ToString();
        fieldBlobCountText.text = BlobManager.GetBlobsInFieldCount().ToString();
        reserveBlobCountText.text = BlobManager.GetCurrentReserveCount().ToString();
        followingBlobCountText.text = player.followingBlobs.Count.ToString();
    }
    
}
