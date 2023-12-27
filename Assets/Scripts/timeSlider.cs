using UnityEngine;
using UnityEngine.UI;

public class timeSlider : MonoBehaviour
{
    private Slider tSlider;
    public Text timeText;
    // Start is called before the first frame update
    void Start()
    {
        tSlider = GetComponent<Slider>();
        if (Time.deltaTime >= 0f)
        {
            tSlider.value += Time.deltaTime;
        }

    }
}
