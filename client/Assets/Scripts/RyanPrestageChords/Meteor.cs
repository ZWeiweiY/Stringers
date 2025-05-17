using UnityEngine;

public class Meteor : Generated
{
    public float moveSpeed = 1f;
    public Transform spawnLeft;
    public Transform spawnRight;
    public float scaleMin = 5f;
    public float scaleMax = 10f;

    void Update()
    {
        transform.Translate(Vector3.back * (moveSpeed * Time.deltaTime), Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            other.gameObject.GetComponent<STGShipController>().Hit();
        }
    }

    public override GameObject Spawn()
    {
        float m = Random.Range(0f, 1f);
        Vector3 spawnLine = Vector3.Lerp(spawnLeft.position, spawnRight.position, m);
        float s = Random.Range(scaleMin, scaleMax);
        gameObject.transform.localScale = new Vector3(s, s, s);
        return Instantiate(gameObject, spawnLine, Quaternion.identity);
    }

    public override void Despawn()
    {
        Destroy(gameObject);
    }
}
