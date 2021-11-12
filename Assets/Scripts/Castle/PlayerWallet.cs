using System;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    public int Money { get; private set; }
    public event Action<int> MoneyChanged;

    private void OnEnable()
    {
        MoneyDispenser.Instance.MoneyDispensed += AddMoney;
    }

    private void OnDisable()
    {
        MoneyDispenser.Instance.MoneyDispensed -= AddMoney;
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
