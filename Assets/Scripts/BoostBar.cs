using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start() 
    {
        //slider = GetComponent<Slider>();
    }
    
    public void SetMaxBoost(int boost)
    {
        slider.maxValue = boost;
        slider.value = boost;
    }

    public void SetBoostBar(float boost)
    {
        slider.value = boost;
    }
}
