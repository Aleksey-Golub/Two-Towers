using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _actionPanel;
    [SerializeField] private GameObject _unitPanel;
    [SerializeField] private Button _buyCastleWeaponPlaceBtn;
    [SerializeField] private GameObject _addCastleWeaponPanel;
    [SerializeField] private GameObject _removeCastleWeapontPanel;
    [SerializeField] private Button _backBtn;
    [SerializeField] private TMP_Text _moneyText;

    [SerializeField] private Castle _myCastle;

    [Header("Templates")]
    [SerializeField] private UnitButton _unitBtnPrefab;

    private void OnEnable()
    {
        _myCastle.Wallet.MoneyChanged += OnMoneyChanged;
    }

    private void OnDisable()
    {
        _myCastle.Wallet.MoneyChanged -= OnMoneyChanged;
    }

    private void Start()
    {
        BackToActionPanel();
        InitUnitPanel();
    }

    private void InitUnitPanel()
    {
        UnitPreset unitPreset;

        for (int i = 0; i < _myCastle.Spawner.UnitsPresets.Length; i++)
        {
            unitPreset = _myCastle.Spawner.UnitsPresets[i];
            var unitBtn = Instantiate(_unitBtnPrefab, _unitPanel.transform) as UnitButton;
            unitBtn.Init(unitPreset.Sprite, unitPreset.Prefab.Cost, i, _myCastle.Spawner.TryBuyUnit);
        }
    }

    public void BackToActionPanel()
    {
        _unitPanel.SetActive(false);
        _addCastleWeaponPanel.SetActive(false);
        _removeCastleWeapontPanel.SetActive(false);

        _actionPanel.SetActive(true);
        _backBtn.interactable = false;
    }

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
        _actionPanel.SetActive(false);
        _backBtn.interactable = true;
    }

    private void OnMoneyChanged(int money)
    {
        _moneyText.text = money.ToString();
    }
}
