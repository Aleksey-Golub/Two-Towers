using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New UnitPreset", menuName = "UnitPreset/Create new UnitPreset")]
public class UnitPreset : ScriptableObject
{
    [SerializeField] private Unit _prefab;
    [FormerlySerializedAs("_sprite")]
    [SerializeField] private Sprite _spriteUIBtn;

    public Unit Prefab => _prefab;
    public Sprite Sprite => _spriteUIBtn;
}
