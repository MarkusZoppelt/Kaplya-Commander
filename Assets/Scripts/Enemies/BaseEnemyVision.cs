using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyVision : MonoBehaviour
{

    [SerializeField] internal float range;
    [SerializeField] internal LayerMask layerMask;
    [SerializeField] internal LayerMask obstacleLayerMask;
    internal GameObject[] targets;

    public virtual GameObject[] AcquireTargets()
    {
        var hits = Physics.SphereCastAll(transform.position, range, Vector3.up, 0, layerMask);

        var greatestHits = new List<GameObject>();
        foreach (var hit in hits) 
        {
            if (hit.collider.gameObject.tag != "friendly")
                continue;
                
            var targetDirection = (hit.point - transform.position).normalized;
            if (Physics.Raycast(transform.position, targetDirection, range+1, obstacleLayerMask))
                continue;

            greatestHits.Add(hit.collider.gameObject);
        }
        return greatestHits.ToArray();
    }
}
