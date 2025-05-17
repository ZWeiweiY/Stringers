using System.Collections;
using UnityEngine;

public class CharacterFlashEffect : MonoBehaviour
{
    private SkinnedMeshRenderer skinnedMesh;
    private Material material;
    private bool isFlashing = false;

    void Start()
    {
        skinnedMesh = GetComponent<SkinnedMeshRenderer>();
        if (skinnedMesh != null)
        {
            material = skinnedMesh.material;
        }
    }

    void Update()
    {
        // 按下空格键触发 Shader 效果
        if (Input.GetKeyDown(KeyCode.Space) && !isFlashing)
        {
            StartCoroutine(FlashRedEffect());
        }
    }

    IEnumerator FlashRedEffect()
    {
        isFlashing = true;
        float time = 0;
        while (time < 1.5f) // 持续1.5秒的闪烁
        {
            float flashAmount = Mathf.PingPong(Time.time * 4, 1); // 在 0 ~ 1 之间循环
            material.SetFloat("_FlashAmount", flashAmount);
            time += Time.deltaTime;
            yield return null;
        }

        material.SetFloat("_FlashAmount", 0); // 复原颜色
        isFlashing = false;
    }
}
