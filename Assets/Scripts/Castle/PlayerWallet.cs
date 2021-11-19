using System;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    private MoneyDispenser _moneyDispenser;

    public int Money { get; private set; }
    public event Action<int> MoneyChanged;

    private void Awake()
    {
        _moneyDispenser = FindObjectOfType<MoneyDispenser>();
    }

    private void OnEnable()
    {
        _moneyDispenser.MoneyDispensed += AddMoney;
    }
    
    private void OnDisable()
    {
        _moneyDispenser.MoneyDispensed -= AddMoney;
    }

    public void AddMoney(int addingMoney)
    {
        if (addingMoney >= 0)
        {
            Money += addingMoney;
            MoneyChanged?.Invoke(Money);
        }
        else
        {
            throw new InvalidOperationException(); 
        }
    }

    public void SpendMoney(int cost)
    {
        if (cost >= 0)
        {
            Money -= cost;
            MoneyChanged?.Invoke(Money);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
}
