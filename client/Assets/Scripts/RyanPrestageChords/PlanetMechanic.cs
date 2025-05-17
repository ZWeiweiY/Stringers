using UnityEngine;
using System.Collections;

public class PlanetMechanic : MonoBehaviour
{
    [Header("Devour Settings")]
    public float moveSpeed = 5f;

    public Vector3 targetOffset = Vector3.zero;

    private bool playerInRange = false;   
    private Transform playerTransform;    
    private bool isMoving = false;        
    private Vector3 initialScale;         

    void Start()
    {
        //initialScale = transform.localScale;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerTransform = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerTransform = null;
        }
    }

    void Update()
    {
        if (playerInRange && !isMoving && Input.GetKeyDown(KeyCode.Space))
        {
            if (playerTransform != null)
            {
                // Set as Player's child
                transform.SetParent(playerTransform, true);
                StartCoroutine(MoveToPlayer());
            }
        }
    }

    IEnumerator MoveToPlayer()
    {
        isMoving = true;
        initialScale = transform.localScale;
        // 当物体成为玩家子物体后，获取其在玩家局部坐标系中的当前位置
        Vector3 startLocalPos = transform.localPosition;
        // 目标位置：玩家局部坐标系中的指定偏移位置
        Vector3 targetLocalPos = targetOffset;
        // 计算局部坐标下的直线距离
        float journeyLength = Vector3.Distance(startLocalPos, targetLocalPos);
        float t = 0f;  // 插值因子（0 到 1）

        // 使用插值逐渐更新局部位置和缩放
        while (t < 1f)
        {
            // 按照速度更新 t 值（确保 t 的增长与整个路程相关）
            t += (moveSpeed * Time.deltaTime) / journeyLength;
            t = Mathf.Clamp01(t);

            // 更新局部位置（物体相对于玩家的坐标）
            transform.localPosition = Vector3.Lerp(startLocalPos, targetLocalPos, t);
            // 缩放从初始尺寸逐渐缩小到初始尺寸的??
            transform.localScale = Vector3.Lerp(initialScale, initialScale / 8f, t);

            yield return null;
        }

        Destroy(gameObject);
    }
}
