using DG.Tweening;
using UnityEngine;

public class PneumaticTube : MonoBehaviour
{
    #region Inspector
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
}
