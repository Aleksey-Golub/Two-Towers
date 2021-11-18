using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPanel : MonoBehaviour
{
    [SerializeField] private Transform _unitsBtnContainer;
    [SerializeField] private UnitButton _unitBtnPrefab;
    [SerializeField, Min(1)] private int _amountOfSimultaneouslyDisplayedButtons = 5;
    [SerializeField] private Button _leftScrollBtn;
    [SerializeField] private Button _rightScrollBtn;

    private List<UnitButton> _unitBtns;
    private int _firstDisplayedBtnIndex = 0;

    public void InitUnitPanel(Castle castle)
    {
        int length = castle.Spawner.UnitsPresets.Length;

        UnitPreset unitPreset;
        _unitBtns = new List<UnitButton>(length);

        for (int i = 0; i < length; i++)
        {
            unitPreset = castle.Spawner.UnitsPresets[i];
            var unitBtn = Instantiate(_unitBtnPrefab, _unitsBtnContainer) as UnitButton;
            unitBtn.Init(unitPreset.Sprite, unitPreset.Prefab.Cost, i, castle.Spawner);

            _unitBtns.Add(unitBtn);
        }

        SwitchBtns(_firstDisplayedBtnIndex);
    }

    private void SwitchBtns(int index)
    {
        for (int i = 0; i < _unitBtns.Count; i++)
        {
            if (i >= index && i < index + _amountOfSimultaneouslyDisplayedButtons)
                _unitBtns[i].gameObject.SetActive(true);
            else
                _unitBtns[i].gameObject.SetActive(false);
        }

        _leftScrollBtn.interactable = (index == 0) ? false : true;
        _rightScrollBtn.interactable = (index + _amountOfSimultaneouslyDisplayedButtons >= _unitBtns.Count) ? false : true;
    }

    public void ScrollLeft()
    {
        _firstDisplayedBtnIndex -= _amountOfSimultaneouslyDisplayedButtons;

        SwitchBtns(_firstDisplayedBtnIndex);
    }

    public void ScrollRight()
    {
        _firstDisplayedBtnIndex += _amountOfSimultaneouslyDisplayedButtons;

        SwitchBtns(_firstDisplayedBtnIndex);
    }
}
