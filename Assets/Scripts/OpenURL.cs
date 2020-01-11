#if UNITY_WEBGL
using System.Runtime.InteropServices;
#endif    
using UnityEngine;

public class OpenURL : MonoBehaviour
{
#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void openPage(string url);
#endif    
    public string url;

    public void Action()
    {
#if !UNITY_WEBGL
        Application.OpenURL(url);
#else
        openPage(url);
#endif
    }
}
