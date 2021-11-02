using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UnitViewer : MonoBehaviour
{
    private Animator _animator;

    private const string isDead = "isDead";
    private const string isWalk = "isWalk";
    private const string AttackTrigger = "AttackTrigger";

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Die()
    {
        _animator.SetBool(isDead, true);
    }

    public void Walk()
    {
        _animator.SetBool(isWalk, true);
    }

    public void Idle()
    {
        _animator.SetBool(isWalk, false);
    }

    public void Attack()
    {
        _animator.SetTrigger(AttackTrigger);
    }
}
