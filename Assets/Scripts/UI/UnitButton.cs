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

    //private int _myUnitIndex = -1;
    private UnitSpawner _spawner;

    public void Init(Sprite unitSprite, int unitCost, int unitIndex, MyDelegate<int> action)
    {
        _unitImage.sprite = unitSprite;
        _costText.text = unitCost.ToString();
        //_myUnitIndex = unitIndex;
        _button.onClick.AddListener(() => action?.Invoke(unitIndex));
    }
}
