using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

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


    void Update()
    {

       
    }
}
