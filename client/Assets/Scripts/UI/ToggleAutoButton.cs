using UnityEngine;
using UnityEngine.UI;

public class ToggleAutoButton : MonoBehaviour
{
    public Texture textureA;
    public Texture textureB;

    private RawImage rawImage;
    private bool isA = true;
    public GameSettings gameSettings;

    void Start()
    {
        rawImage = GetComponent<RawImage>();
        rawImage.texture = textureA;
        gameSettings.autoExpression = false;
    }

    public void ToggleTexture()
    {
        isA = !isA;
        rawImage.texture = isA ? textureA : textureB;
        gameSettings.autoExpression = !gameSettings.autoExpression;
    }
}