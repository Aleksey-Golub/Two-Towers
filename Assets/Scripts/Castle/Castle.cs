using UnityEngine;
using UnityEngine.Events;

public class Castle : MonoBehaviour, IDamageAble
{
    [SerializeField] private int _maxHealth = 250;
    [SerializeField] private int _currentHealth;
    [SerializeField] private ParticleSystem _hit_effect;

    [Header("References")]
    [SerializeField] private UnitSpawner _spawner;
    [SerializeField] private PlayerWallet _wallet;
    [SerializeField] private CastleViewer _viewer;

    public PlayerWallet Wallet => _wallet;
    public UnitSpawner Spawner => _spawner;
    public CastleTargetPoint TargetPoint;
    public event UnityAction<int, int> HealthChanged;
    public event UnityAction<Castle> GameOver;

    private void Start()
    {
        GameManager.Instance.RegisterCastle(this);    

        Wallet.AddMoney(500);
        _currentHealth = _maxHealth;
        HealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _hit_effect.Play();
        _currentHealth = Mathf.Clamp(_currentHealth, 0, int.MaxValue);
        HealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth <= 0)
        {
            Die();
            Debug.Log("GAME OVER");
        }
    }

    private void Die()
    {
        GameOver?.Invoke(_spawner.Enemy_castle);
        enabled = false;
        _spawner.enabled = false;

        _viewer.Die();
    }
}
