using UnityEngine;

public class WebCamController : Singleton<WebCamController>
{
    public WebCamTexture _webCamTexture { get; private set; }
    public Material _material;

    [SerializeField] private int deviceId = 0;
    [SerializeField] private int _width = 256;
    [SerializeField] private int _height = 256;
    [SerializeField] private int _fps = 60;

    [SerializeField] private FaceTrackingReceiver _faceTrackingReceiver;
    [SerializeField] private int _frameSkip = 3;

    private int _frameCount = 0;

    private bool _isSending = false;
    private Texture2D _texture;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (WebCamTexture.devices.Length == 0)
        {
            Debug.LogError("WebCam not found");
            return;
        }

        if (_faceTrackingReceiver == null)
        {
            Debug.LogError("FaceTrackingReceiver is null");
            return;
        }

        WebCamDevice webCamDevice = WebCamTexture.devices[deviceId];
        Debug.Log(webCamDevice.name);

        _webCamTexture = new WebCamTexture(webCamDevice.name, _width, _height, _fps);

        Debug.Log($"WebCamTexture size: {_webCamTexture.width}x{_webCamTexture.height}");

        if (_material != null)
        {
            _material.mainTexture = _webCamTexture;
        }
        else
        {
            Debug.LogError("Material is null");
        }

        _webCamTexture.Play();

        _texture = new Texture2D(_webCamTexture.width, _webCamTexture.height, TextureFormat.RGB24, false);
        Debug.Log($"Texture size: {_texture.width}x{_texture.height}");
    }

    private void OnDisable()
    {
        if (_webCamTexture != null && _webCamTexture.isPlaying)
        {
            _webCamTexture.Stop();
        }
    }


    // Update is called once per frame
    private void Update()
    {
        if (_isSending && _faceTrackingReceiver._isConnected && _webCamTexture.didUpdateThisFrame)
        {
            if (_frameCount % _frameSkip == 0)
            {
                SendImage();
                _frameCount = 0;
            }
            else
            {
                _frameCount++;
            }
            //Debug.Log($"WebCamTexture size: {_webCamTexture.width}x{_webCamTexture.height}");
        }
    }

    public void StartSending()
    {
        _isSending = true;
    }

    public void StopSending()
    {
        _isSending = false;
    }

    public void ToggleSending()
    {
        _isSending = !_isSending;
    }

    public void SendImage()
    {
        if (_webCamTexture != null && _webCamTexture.isPlaying)
        {
            _texture.SetPixels32(_webCamTexture.GetPixels32());
            _texture.Apply();
            byte[] bytes = _texture.EncodeToJPG();
            _faceTrackingReceiver.SendWebcamFrame(bytes);
        }
    }
}
