using System;
using UnityEngine;

/// <summary>
/// AI спанвит случайного юнита из заданного диапазона при наличии у него денег по кулдауну.
/// </summary>
public class SimpleAI : MonoBehaviour
{
    [SerializeField] private UnitSpawner _spawner;
    [Tooltip("AI спавнит юнитов по индексу в диапазоне от 0 до MaxUnitIndex из массива юнитов в Спавнере")]
    [SerializeField] private int _maxUnitIndex = 2;

    private void Update()
    {
        if (Time.frameCount % 5 != 0)
            return;

        //TryBuyRandonUnit();
        _spawner.TryBuyUnit(2);
    }

    private void OnValidate()
    {
        ValidateMaxUnitIndex();
    }

    public void ValidateMaxUnitIndex()
    {
        int maxIndex = _spawner.UnitsPresets.Length - 1;

        _maxUnitIndex = Mathf.Clamp(_maxUnitIndex, 0, maxIndex);

        if (maxIndex < 0)
        {
            Debug.LogError("Массив юнитов в Спавнере пуст");
            return;
        }
    }

    private bool TryBuyRandonUnit()
    {
        int index = UnityEngine.Random.Range(0, _maxUnitIndex + 1);

        return _spawner.TryBuyUnit(index);
    }
}
