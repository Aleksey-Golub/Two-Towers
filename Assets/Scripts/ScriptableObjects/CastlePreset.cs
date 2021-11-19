using UnityEngine;

[CreateAssetMenu(fileName = "New CastlePreset", menuName = "CastlePreset/Create new CastlePreset")]
public class CastlePreset : ScriptableObject
{
    [SerializeField] private Sprite _unDestroyedFront;
    [SerializeField] private Sprite _unDestroyedBack;
    [SerializeField] private Sprite _destroyed;

    public Sprite UnDestroyedFront => _unDestroyedFront;
    public Sprite UnDestroyedBack => _unDestroyedBack;
    public Sprite Destroyed => _destroyed;
}
