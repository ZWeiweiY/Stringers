using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PowerUpTest: MonoBehaviour
{
    //public Animator anim;
    public VisualEffect levelUp;

    private bool levelingUp;

    private void Update()
    {
        if (Input.GetKeyDown("Space") && !levelingUp)
        {
            //anim.SetTrigger("PowerUp");

            if (levelUp != null)
                levelUp.Play();

            levelingUp = true;
            StartCoroutine(ResetBool(levelingUp, 0.5f));
        }
        //if (anim != null)
        //{
            
        //}
    }

    IEnumerator ResetBool (bool boolToReset, float delay = 0.1f)
    {
        yield return new WaitForSeconds (delay); 
        levelingUp = !levelingUp;
    }

}
