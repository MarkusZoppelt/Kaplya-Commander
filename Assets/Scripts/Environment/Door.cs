using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform door;
    [SerializeField] private Transform openPosition;
    [SerializeField] private Transform closedPosition;
    [SerializeField] private float changeTime;

    public void Open()
    {
        door.DOMove(openPosition.position, changeTime).SetEase(Ease.InOutElastic);
    }

    public void Close()
    {
        door.DOMove(closedPosition.position, changeTime).SetEase(Ease.InOutElastic);
    }
}
