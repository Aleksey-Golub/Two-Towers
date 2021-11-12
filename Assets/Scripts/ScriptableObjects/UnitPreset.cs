using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UnitPreset", menuName = "UnitPreset/Create new UnitPreset")]
public class UnitPreset : ScriptableObject
{
    [SerializeField] private Unit _prefab;
    [SerializeField] private Sprite _sprite;

    public Unit Prefab => _prefab;
    public Sprite Sprite => _sprite;
}
