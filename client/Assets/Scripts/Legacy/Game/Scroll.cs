using System;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    private Vector3 startPosition;
    [SerializeField] private bool scrolling;
    
    private void Awake()
    {
        startPosition = transform.position;
        scrollSpeed = SecondStage.instance.bpm;
    }

    private void Update()
    {
        if (scrolling)
        {
            transform.position = new Vector3(transform.position.x - scrollSpeed / 60 * SecondStage.instance.horizontalSpacing * Time.deltaTime, transform.position.y, transform.position.z);
        }
    }

    void ResetPosition()
    {
        transform.position = startPosition;
    }
}
