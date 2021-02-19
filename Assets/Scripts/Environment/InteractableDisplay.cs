using TMPro;
using UnityEngine;

public class InteractableDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI assignedBlobsText;
    [SerializeField] private TextMeshProUGUI neededBlobsText;
    [Header("Settings")]
    [SerializeField] private Color noneAssignedColor = Color.white;
    [SerializeField] private Color notEnoughColor = Color.red;
    [SerializeField] private Color enoughColor = Color.green;

    private int neededBlobs = 0;
    public int NeededBlobs
    {
        get
        {
            return neededBlobs;
        }

        set
        {
            neededBlobsText.text = value.ToString();
            neededBlobs = value;
        }
    }

    private int assignedBlobs = 0;
    public int AssignedBlobs
    {
        get
        {
            return assignedBlobs;
        }

        set
        {
            assignedBlobsText.text = value.ToString();
            assignedBlobs = value;

            if (assignedBlobs == 0)
            {
                assignedBlobsText.color = noneAssignedColor;
            }
            else if (assignedBlobs < neededBlobs)
            {
                assignedBlobsText.color = notEnoughColor;
            }
            else
            {
                assignedBlobsText.color = enoughColor;
            }
        }
    }
}
