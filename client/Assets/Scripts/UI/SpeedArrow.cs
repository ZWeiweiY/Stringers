using UnityEngine;

public class SpeedArrow : MonoBehaviour
{
    public GameObject[] speeds;
    public GameObject arrowL;
    public GameObject arrowR;
    [SerializeField] private int curr = 0;

    public void IncreaseSpeed()
    {
        curr++;
        DisplaySpeedIcon();
    }

    public void DecreaseSpeed()
    {
        curr--;
        DisplaySpeedIcon();
    }

    public void DisplaySpeedIcon()
    {
        for (int i = 0; i < speeds.Length; i++)
        {
            speeds[i].SetActive(i == curr);
        }
        arrowL.SetActive(curr != 0);
        arrowR.SetActive(curr != speeds.Length - 1);
    }
}
