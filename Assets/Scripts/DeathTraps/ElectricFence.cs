using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricFence : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject fence;
    [SerializeField] private MeshFilter fenceMeshFilter;
    [SerializeField] private MeshCollider fenceCollider;
    [SerializeField] private Transform baseA;
    [SerializeField] private Transform baseB;
    [Header("Settings")]
    [SerializeField] private float fenceHeight = 4f;
    [SerializeField] private float shockTime = 2f;
    [SerializeField] private float pauseTime = 5f;

    // Mesh fields
    private Mesh fenceMesh;
    private List<Vector3> fenceVertices = new List<Vector3>();
    private List<int> fenceTriangles = new List<int>();
    private List<Vector2> fenceUVs = new List<Vector2>();

    // Behaviour fields
    private Coroutine shockingRoutine;

    private void Awake()
    {
        fenceMeshFilter.mesh = fenceMesh = new Mesh();
        TriangulateMesh();
        StartShocking();
    }

    #region Mesh creation
    private void TriangulateMesh()
    {
        // Calculate vertex positions
        Vector3 v1, v2, v3, v4;
        v1 = v3 = baseA.localPosition;
        v2 = v4 = baseB.localPosition;
        v3.y += fenceHeight;
        v4.y += fenceHeight;

        // Add to mesh data
        fenceVertices.Add(v1);
        fenceVertices.Add(v2);
        fenceVertices.Add(v3);
        fenceVertices.Add(v4);

        // Add UV data
        fenceUVs.Add(Vector2.zero);
        fenceUVs.Add(new Vector2(1, 0));
        fenceUVs.Add(new Vector2(0, 1));
        fenceUVs.Add(Vector2.one);

        // Add triangleData
        fenceTriangles.Add(0);
        fenceTriangles.Add(2);
        fenceTriangles.Add(1);
        fenceTriangles.Add(1);
        fenceTriangles.Add(2);
        fenceTriangles.Add(3);

        ApplyMeshData();
    }

    private void ApplyMeshData()
    {
        fenceMesh.vertices = fenceVertices.ToArray();
        fenceMesh.SetUVs(0, fenceUVs);
        fenceMesh.triangles = fenceTriangles.ToArray();
        fenceCollider.sharedMesh = fenceMesh;
        fenceCollider.convex = true;
        fenceCollider.isTrigger = true;
    }
    #endregion

    #region Shocking
    public void StartShocking()
    {
        shockingRoutine = StartCoroutine(ShockingRoutine());
    }

    public void StopShocking()
    {
        fence.SetActive(false);
        if(shockingRoutine != null)
        {
            StopCoroutine(shockingRoutine);
            shockingRoutine = null;
        }
    }

    private IEnumerator ShockingRoutine()
    {
        while (true)
        {
            fence.SetActive(true);
            yield return new WaitForSeconds(shockTime);

            fence.SetActive(false);
            yield return new WaitForSeconds(pauseTime);
        }
    }
    #endregion
}
