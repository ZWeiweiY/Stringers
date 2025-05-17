using UnityEngine;

public class BackdropGenerate : MonoBehaviour
{
    [Header("Meteor")]
    public GameObject[] meteorPrefab;  

    [Header("Vertex Amount")]
    public int[] targetFaceIndices;

    [Header("Meteor Generate Settings")]
    public Vector2 randomScaleRange = new Vector2(0.5f, 2f);
    public bool isRandomRotation = true;

    void Start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf == null)
        {
            Debug.LogWarning("none meshfilter");
            return;
        }

        Mesh mesh = mf.mesh;
        Vector3[] vertices = mesh.vertices;

        if (targetFaceIndices != null && targetFaceIndices.Length > 0)
        {
            foreach (int idx in targetFaceIndices)
            {
                if (idx < 0 || idx >= vertices.Length)
                    continue; // skip invalid idx

                Vector3 vertexLocalPos = vertices[idx];
                Vector3 spawnPos = transform.TransformPoint(vertexLocalPos);

                float randomScale = Random.Range(randomScaleRange.x, randomScaleRange.y);

                Quaternion randomQuat = Quaternion.identity;
                if (isRandomRotation)
                {
                    float randomX = Random.Range(0f,360f);
                    float randomY = Random.Range(0f, 360f);
                    float randomZ = Random.Range(0f, 360f);
                    randomQuat = Quaternion.Euler(randomX, randomY, randomZ);
                }

                int randomIndex = Random.Range(0, meteorPrefab.Length);
                GameObject meteorToSpawn = meteorPrefab[randomIndex];

                GameObject obj = Instantiate(meteorToSpawn, spawnPos, randomQuat);
                obj.transform.localScale = Vector3.one * randomScale;
            }
        }
        else
        {
            Debug.LogWarning("none valid vertex！");
        }
    }
}
