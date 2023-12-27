using UnityEngine;

public class AudioData : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(this.gameObject, 0.5f);
    }
}
