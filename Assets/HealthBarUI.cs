using Assets.Components;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider Slider;
    [SerializeField] private TMP_Text HealthText;
    public float MaxHealth;
    public float Health;




    private void Start()
    {
        Slider.maxValue = MaxHealth;
        Slider.minValue = 0;
        Slider.value = Health;
    }

    public void UpdateHealth()
    {
        Slider.value = Health;
        HealthText.text = ((int)math.round(Health)).ToString();
    }

    private void Update()
    {
        
    }
}
