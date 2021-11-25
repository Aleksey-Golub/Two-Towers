using System;
using UnityEngine;

/// <summary>
/// AI ������� ���������� ����� �� ��������� ��������� ��� ������� � ���� ����� �� ��������.
/// </summary>
public class SimpleAI : MonoBehaviour
{
    [SerializeField] private UnitSpawner _spawner;
    [Tooltip("AI ������� ������ �� ������� � ��������� �� 0 �� MaxUnitIndex �� ������� ������ � ��������")]
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
            Debug.LogError("������ ������ � �������� ����");
            return;
        }
    }

    private bool TryBuyRandonUnit()
    {
        int index = UnityEngine.Random.Range(0, _maxUnitIndex + 1);

        return _spawner.TryBuyUnit(index);
    }
}
