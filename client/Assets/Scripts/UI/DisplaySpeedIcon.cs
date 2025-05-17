using System;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySpeedIcon : MonoBehaviour
{
    public Texture2D[] icons;
    public RawImage rawImage;
    public GameSettings gameSettings;

    public void DisplayIcon(int i)
    {
        rawImage.texture = icons[i];
    }

    public void DisplayCurrentIcon()
    {
        int i = LevelManager.Instance.GetCurrentLevel();
        switch (i)
        {
            case 4: rawImage.texture = icons[gameSettings.index[0]];
                break;
            case 6: rawImage.texture = icons[gameSettings.index[1]];
                break;
            case 8: rawImage.texture = icons[gameSettings.index[2]];
                break;
            default:
                break;
        }
    }

    private void OnEnable()
    {
        DisplayCurrentIcon();
    }
}
