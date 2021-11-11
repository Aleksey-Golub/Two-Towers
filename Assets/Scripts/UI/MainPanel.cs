using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    [SerializeField] private GameObject _actionPanel;
    [SerializeField] private GameObject _unitPanel;
    [SerializeField] private Button _buyCastleWeaponPlaceBtn;
    [SerializeField] private GameObject _addCastleWeaponPanel;
    [SerializeField] private GameObject _removeCastleWeapontPanel;
    [SerializeField] private Button _backBtn;
    [SerializeField] private TMP_Text _moneyText;

    [SerializeField] private Castle _myCastle;
    
    private void OnEnable()
    {
        //Debug.Log(_myCastle == null);
        //Debug.Log(_myCastle.Wallet == null);
        _myCastle.Wallet.MoneyChanged += OnMoneyChanged;
    }

    private void OnDisable()
    {
        _myCastle.Wallet.MoneyChanged -= OnMoneyChanged;
    }

    private void Start()
    {
        BackToActionPanel();
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
