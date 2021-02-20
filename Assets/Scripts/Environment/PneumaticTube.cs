using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class PneumaticTube : MonoBehaviour
{
    #region Inspector
    [Header("References")]
    [SerializeField] private BlobManagerDisplay managerDisplay;
    [Header("Suck Up Settings")]
    [SerializeField] private Transform tubeOpening;
    [SerializeField] private float carryObjectShakeDuration = 1.5f;
    [SerializeField] private float carryObjectSuckUpDuration = 0.5f;
    #endregion

    internal void OnTriggerEnter(Collider other)
    {
        var carryObject = other.GetComponent<CarryObject>();
        if (carryObject != null)
        {
            SuckUp(carryObject);
            return;
        }

        var blob = other.GetComponent<BlobBase>();
        if (blob != null && blob.State == BlobState.GoingToTube)
        {
            SuckUp(blob.transform);
            blob.PlayThrowSound();
            BlobManager.ForgetBlob(blob);
            return;
        }

        var player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            managerDisplay.gameObject.SetActive(true);
            managerDisplay.ActivateForPlayer(player);
            return;
        }
    }

    internal void SuckUp(CarryObject carryObject)
    {
        carryObject.OnSuckedIntoTube();

        carryObject.transform
            .DOShakePosition(carryObjectShakeDuration, 0.1f)
            .OnComplete(() =>
            {
                carryObject.transform.DOMove(tubeOpening.position, carryObjectSuckUpDuration).SetEase(Ease.OutExpo);
                carryObject.transform.DOScale(0.1f, carryObjectSuckUpDuration).SetEase(Ease.OutExpo)
                    .OnComplete(() => 
                    {
                        Destroy(carryObject.gameObject);
                    });
            });
    }

    internal void SuckUp(Transform other)
    {
        other.DOShakePosition(carryObjectShakeDuration, 0.1f)
            .OnComplete(() =>
            {
                other.DOMove(tubeOpening.position, carryObjectSuckUpDuration).SetEase(Ease.OutExpo);
                other.DOScale(0.1f, carryObjectSuckUpDuration).SetEase(Ease.OutExpo)
                    .OnComplete(() =>
                    {
                        Destroy(other.gameObject);
                    });
            });
    }

    public void SpawnBlob(BlobType type, PlayerController player, Action callback)
    {
        var blob = Instantiate(BlobManager.GetBlobPrefab(type), tubeOpening.position, Quaternion.identity);
        blob.PlayThrowSound();
        blob.transform.localScale = Vector3.zero;
        blob.transform.DOMove(transform.position, carryObjectSuckUpDuration).SetEase(Ease.OutBounce);
        blob.transform.DOScale(1f, carryObjectSuckUpDuration).SetEase(Ease.OutExpo)
            .OnComplete(() =>
            {
                BlobManager.RememberBlob(blob);
                player.AddBlobToFollowers(blob);
                callback?.Invoke();
            });
    }
}
