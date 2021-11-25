using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public delegate bool MyDelegate<in T>(T obj);

public class UnitButton : MonoBehaviour
{
    [SerializeField] private Image _unitImage;
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _progressBarRoot;
    [SerializeField] private Image _progressBarImage;
    [SerializeField] private TMP_Text _unitsInQueueText;

    private int _myUnitIndex = -1;
    private UnitSpawner _spawner;
    private int _unitsInQueue;

    public void Init(Sprite unitSprite, int unitCost, int unitIndex, UnitSpawner spawner)//MyDelegate<int> action)
    {
        _unitImage.sprite = unitSprite;
        _costText.text = unitCost.ToString();
        _myUnitIndex = unitIndex;
        _spawner = spawner;
        _spawner.SpawnProgressUpdated += OnSpawnProgressUpdated;
        //_button.onClick.AddListener(() => action?.Invoke(unitIndex));
        _button.onClick.AddListener(OnBtnClicked);
    }

    private void OnSpawnProgressUpdated(int unitIndex, float progress)
    {
        if (unitIndex != _myUnitIndex)
            return;

        _progressBarImage.fillAmount = progress;

        if (progress == 1)
        {
            _unitsInQueue--;
            UpdateUnitsInQueueText();
            _progressBarImage.fillAmount = 0;

            if (_unitsInQueue == 0)
                _progressBarRoot.SetActive(false);
        }
    }

    private void OnBtnClicked()
    {
        bool success = _spawner.TryBuyUnit(_myUnitIndex);

        if (success)
        {
            _progressBarRoot.SetActive(true);

            _unitsInQueue++;
            UpdateUnitsInQueueText();
        }
    }

    private void UpdateUnitsInQueueText()
    {
        _unitsInQueueText.text = _unitsInQueue.ToString();
    }

    private void OnDestroy()
    {
        _spawner.SpawnProgressUpdated -= OnSpawnProgressUpdated;
    }
}
