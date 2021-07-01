using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    
    public void SetMaxFill(int fill)
    {
        slider.maxValue = fill;
        slider.value = fill;
    }

    public void SetFillBar(float fill)
    {
        slider.value = fill;
    }
}
