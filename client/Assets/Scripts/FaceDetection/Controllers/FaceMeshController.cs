using UnityEngine;
using System.Collections.Generic;

public class FaceMeshController : MonoBehaviour
{
    [SerializeField] private FaceDataSO faceData;

    private Mesh faceMesh;
    private List<Vector3> vertexList = new List<Vector3>();
    [SerializeField] private float meshScale = -0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        faceMesh = GetComponent<MeshFilter>().mesh;
        vertexList.AddRange(faceMesh.vertices);
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateFaceMesh();
        //faceMesh.SetVertices(faceData.landmarkList);
    }

    private void UpdateFaceMesh()
    {
        for (var i = 0; i < faceData.landmarkList.Count -10; i++)
        {
            vertexList[i] = faceData.landmarkList[i] * meshScale + transform.position;
        }

        faceMesh.SetVertices(vertexList);

    }

}
