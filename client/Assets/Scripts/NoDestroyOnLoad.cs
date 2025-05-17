using UnityEngine;

public class NoDestroyOnLoad : MonoBehaviour
{
    public static NoDestroyOnLoad io;

    public void Awake()
    {
        if (io == null)
        {
            io = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
