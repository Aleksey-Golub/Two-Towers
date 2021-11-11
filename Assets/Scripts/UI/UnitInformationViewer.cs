using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitInformationViewer : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TMP_Text _gradeText;
    [SerializeField] private GameObject _oneStar;
    [SerializeField] private GameObject _twoStars;

    private void OnEnable()
    {
        _unit.HealthChanged += OnHealthChanged;
        _unit.GradeChanged += OnGradeChanged;
    }

    private void OnDisable()
    {
        _unit.HealthChanged -= OnHealthChanged;
        _unit.GradeChanged -= OnGradeChanged;
    }

    private void OnHealthChanged(int currentHealth, int maxHealth)
    {
        _healthSlider.value = (float)currentHealth / maxHealth;
    }

    private void OnGradeChanged(int newGrade)
    {
        _gradeText.text = newGrade.ToString();
        SwitchStars(newGrade);
    }

    private void SwitchStars(int newGrade)
    {
        switch (newGrade)
        {
            case 1:
                _oneStar.SetActive(false);
                _twoStars.SetActive(false);
                break;
            case 2:
                _oneStar.SetActive(true);
                _twoStars.SetActive(false);
                break;
            case 3:
                _oneStar.SetActive(false);
                _twoStars.SetActive(true);
                break;
        }
    }
}
