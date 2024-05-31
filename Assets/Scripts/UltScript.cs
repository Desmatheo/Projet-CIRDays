using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltBar : MonoBehaviour
{
    
    public Slider slider;
    public void SetMaxUlt(float ult)
    {
        slider.maxValue = ult;
        slider.value = 0;
    }

    // Update is called once per frame
    public void SetUlt(float ult)
    {
        slider.value = ult;
    }
}
