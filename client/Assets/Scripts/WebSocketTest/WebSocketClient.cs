using UnityEngine;
using NativeWebSocket;
using System.Collections;
using System.Threading.Tasks;

public class WebSocketClient : MonoBehaviour
{
    private WebSocket _webSocket;
    [SerializeField] private string _url = "ws://localhost:12345/ws";

    public bool _isConnected { get; private set; } = false;
    private bool _isReconnecting = false;
    [SerializeField] private WebCamController _webCamController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async void Start()
    {
        await ConnectWebSocket();
    }

    private async Task ConnectWebSocket()
    {
        _webSocket = new WebSocket(_url);

        _webSocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
            _isConnected = true;
            _isReconnecting = false;

            if (_webCamController != null)
            {
                _webCamController.StartSending();
            }
        };

        _webSocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        _webSocket.OnClose += async (e) =>
        {
            Debug.Log("Connection closed!");
            _isConnected = false;

            if (_webCamController != null)
            {
                _webCamController.StopSending();
            }

            if (!_isReconnecting && Application.isPlaying)
            {
                _isReconnecting = true;
                await TryReconnect();
            }
        };

        _webSocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            if (message != "Image recieved") 
            { 
                Debug.Log("Received Message: " + message);
            }
            
        };

        await _webSocket.Connect();
    }

    private async Task TryReconnect() 
    {
        while(!_isConnected)
        {
            Debug.Log("Trying to reconnect...");
            await _webSocket.Connect();
            await Task.Delay(3000);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (_webSocket != null)
        {
            _webSocket.DispatchMessageQueue();
        }
    }

    public async void SendWebcamFrame(byte[] frame)
    {
        if (_webSocket.State == WebSocketState.Open)
        {
            await _webSocket.Send(frame);
            //await Task.Run(async () => await _webSocket.Send(frame));
        }
    }

    public async void SendMessage(string message)
    {
        if (_webSocket.State == WebSocketState.Open)
        {
            await _webSocket.SendText(message);
        }
    }

    private async void OnApplicationQuit()
    {
        if (_webSocket != null && _webSocket.State == WebSocketState.Open)
        {

            await _webSocket.Close();
        }
    }
}
