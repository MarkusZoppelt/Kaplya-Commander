using UnityEngine;

public static class Helper
{
    public static T GetClosestObject<T>(T[] objects, Vector3 referencePoint) where T : MonoBehaviour
    {
        T closestObject = null;
        var closestDistance = float.MaxValue;
        foreach (var obj in objects)
        {
            var objDistance = Vector3.Distance(obj.transform.position, referencePoint);
            if (objDistance < closestDistance)
            {
                closestObject = obj;
                closestDistance = objDistance;
            }
        }
        return closestObject;
    }
}
