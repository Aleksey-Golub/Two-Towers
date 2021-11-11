using UnityEngine;
using UnityEngine.Events;

public class Castle : MonoBehaviour, IDamageAble
{
    [SerializeField] private int _maxHealth = 250;
    [SerializeField] private int _currentHealth;
    [SerializeField] private ParticleSystem _hit_effect;

    public PlayerWallet Wallet => _wallet;
    public CastleTargetPoint TargetPoint;
    public event UnityAction<int, int> HealthChanged;

    private PlayerWallet _wallet = new PlayerWallet(100);

    private void Start()
    {
        Wallet.AddMoney(0);
        _currentHealth = _maxHealth;
        HealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _hit_effect.Play();
        HealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth <= 0)
        {
            // to do
            Debug.Log("GAME OVER");
        }
    }
}
