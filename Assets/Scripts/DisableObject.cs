using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObject : MonoBehaviour
{
    public GameObject target;

    public void Action()
    {
        target.SetActive(false);
    }
}
