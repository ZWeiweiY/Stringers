using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public RawImage countDownImage;
    public Texture[] countDownTextures;
    public Texture countDown1;
    public Texture countDown2;
    public Texture countDown3;
    public Texture countDown4;
    public Texture countDown5;

    public IEnumerator CountDownTwoSec()
    {
        countDownImage.texture = countDown2;
        yield return new WaitForSecondsRealtime(1f);
        countDownImage.texture = countDown1;
        yield return new WaitForSecondsRealtime(1f);
        gameObject.SetActive(false);
    }

    public IEnumerator ShowCountDown(int seconds)
    {
        if (seconds > 5)
        {
            Debug.Log(seconds + "s is too long");
            yield break;
        }
        for (int i = seconds; i>0; i--)
        {
            countDownImage.texture = countDownTextures[i - 1];
            yield return new WaitForSeconds(1f);
        }
        gameObject.SetActive(false);
    }


}
