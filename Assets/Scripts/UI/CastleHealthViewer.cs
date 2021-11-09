using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CastleHealthViewer : MonoBehaviour
{
    [SerializeField] private Castle _castle;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private Slider _healthSlider;

    private void OnEnable()
    {
        _castle.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        _castle.HealthChanged -= OnHealthChanged;
    }
    
    private void OnHealthChanged(int currentHealth, int maxHealth)
    {
        _healthText.text = currentHealth.ToString();

        _healthSlider.value = (float)currentHealth / maxHealth;
    }
}
