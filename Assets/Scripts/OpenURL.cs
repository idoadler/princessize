using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    public string url;

    public void Action()
    {
        Application.OpenURL(url);
    }
}
