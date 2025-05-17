using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDissolve : MonoBehaviour
{
    private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");

    private SkinnedMeshRenderer[] meshRenderers;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;
    public SkinnedMeshRenderer bodyRenderer;
    public Material outlineMaterial;

    void Awake()
    {
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DissolveCo());
        }*/
    }

    public IEnumerator DissolveCo()
    {
        if (meshRenderers.Length > 0)
        {

            float counter = 0;
            //GameManager.Instance.character.GetComponent<RBMoveController>().resetting = true;
            Material[] newMaterials = new Material[1];

            // Copy only the first material
            newMaterials[0] = bodyRenderer.materials[0];

            // Assign the new material array
            bodyRenderer.materials = newMaterials;
            while (counter < 1)
            {
                counter += dissolveRate;
                foreach (SkinnedMeshRenderer renderer in meshRenderers)
                {
                    renderer.material.SetFloat(DissolveAmount, counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
            //StartCoroutine(GameManager.Instance.RestartGame());
            /*float counter = 0;
            
            while (skinnedMaterials[0].GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;
                for (int i = 0; i < skinnedMaterials.Count; i++)
                {
                    skinnedMaterials[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }

            StartCoroutine(GameManager.Instance.RestartGame());*/
        }
    }
    
    public IEnumerator ReEmerge()
    {
        if (meshRenderers.Length > 0)
        {
            float counter = 1;

            while (counter > 0)
            {
                counter -= dissolveRate;
                foreach (SkinnedMeshRenderer renderer in meshRenderers)
                {
                    renderer.material.SetFloat(DissolveAmount, counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
            Material[] newMaterials = new Material[2];

            // Copy only the first material
            newMaterials[0] = bodyRenderer.materials[0];
            newMaterials[1] = outlineMaterial;
            // Assign the new material array
            bodyRenderer.materials = newMaterials;
            
            /*while (skinnedMaterials[0].GetFloat("_DissolveAmount") > 0)
            {
                counter -= dissolveRate;
                for (int i = 0; i < skinnedMaterials.Count; i++)
                {
                    skinnedMaterials[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
            GameManager.Instance.character.GetComponent<RBMoveController>().resetting = false;*/
        }
    }

}