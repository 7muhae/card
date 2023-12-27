using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioData : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(this.gameObject, 0.5f);
    }
}
