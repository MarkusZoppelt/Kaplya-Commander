using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class GiantButton : Interactable
{
    [SerializeField] internal Transform buttonTransform;
    [SerializeField] internal Vector3 pressedOffset;
    [SerializeField] internal UnityEvent onButtonPressed;

    internal override void StartInteraction()
    {
        base.StartInteraction();
        buttonTransform.DOMove(buttonTransform.position + pressedOffset, 0.5f).SetEase(Ease.InElastic);
        onButtonPressed?.Invoke();
    }

    internal override void StopInteraction()
    {
        base.StopInteraction();
        buttonTransform.DOMove(buttonTransform.position - pressedOffset, 0.5f).SetEase(Ease.InElastic);
    }

    public override BlobState AssignBlob(BlobBase blob)
    {
        var state = base.AssignBlob(blob);
        blob.StartInteracting(this);
        return state;
    }

    public override Vector3 GetBlobOffset()
    {
        float angle = assignedBlobs.Count * Mathf.PI * 2f / blobsNeeded;
        float distanceToCenter = Random.Range(0f, radius);
        return new Vector3(Mathf.Cos(angle) * distanceToCenter, 0, Mathf.Sin(angle) * distanceToCenter);
    }
}
