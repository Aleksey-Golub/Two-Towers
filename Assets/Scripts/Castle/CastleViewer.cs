using UnityEngine;

public class CastleViewer : MonoBehaviour
{
    [SerializeField] private CastlePreset _preset;
    [SerializeField] private SpriteRenderer _rendererFront;
    [SerializeField] private SpriteRenderer _rendererBack;

    private void Start()
    {
        _rendererFront.sprite = _preset.UnDestroyedFront;
        _rendererBack.sprite = _preset.UnDestroyedBack;
    }

    public void Die()
    {
        _rendererFront.sprite = _preset.Destroyed;
        _rendererBack.enabled = false;
    }
}
