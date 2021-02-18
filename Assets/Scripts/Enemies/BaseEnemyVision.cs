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
    internal RaycastHit obstacleHit;

    public virtual GameObject[] AcquireTargets()
    {
        var hits = Physics.SphereCastAll(transform.position, range, Vector3.up, 0, layerMask);

        var greatestHits = new List<GameObject>();
        foreach (var hit in hits) 
        {
            if (hit.collider.gameObject.tag != "friendly")
                continue;
                
            var targetDirection = (hit.collider.transform.position - transform.position).normalized;
            if (Physics.Raycast(transform.position,  targetDirection, out obstacleHit, range +1, obstacleLayerMask))
            {
                if (Vector3.Distance(hit.collider.transform.position, transform.position) > Vector3.Distance(obstacleHit.point, transform.position))
                continue;
            }

            greatestHits.Add(hit.collider.gameObject);
        }
        return greatestHits.ToArray();
    }
}
