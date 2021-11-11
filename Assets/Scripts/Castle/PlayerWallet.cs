using System;

public class PlayerWallet
{
    public int Money { get; private set; }
    public event Action<int> MoneyChanged;

    public PlayerWallet(int startMoney)
    {
        Money = startMoney;
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
