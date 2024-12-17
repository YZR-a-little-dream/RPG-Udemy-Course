using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    private CharacterStats mystats;
    private RectTransform myTransform;
    private Slider slider;

    private void Start() {
        myTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        mystats = GetComponentInParent<CharacterStats>();

        entity.onFlipped += FlipUI;
        mystats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();                   //Initialize the blood bar
    }

    private void OnDisable() 
    {
        entity.onFlipped -= FlipUI;
        mystats.onHealthChanged -= UpdateHealthUI;
    }

    private void FlipUI() => myTransform.Rotate(0,180,0);
    
        private void UpdateHealthUI()
    {
        slider.maxValue = mystats.GetMaxHealthValue();
        slider.value = mystats.currentHealth;
    }
}
