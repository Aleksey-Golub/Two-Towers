using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UnitViewer : MonoBehaviour
{
    private Animator _animator;

    private readonly int isDead = Animator.StringToHash("isDead");
    private readonly int isWalk = Animator.StringToHash("isWalk");
    private readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");

    private void Start() =>_animator = GetComponent<Animator>();

    public void Die() => _animator.SetBool(isDead, true);
    
    public void Walk() => _animator.SetBool(isWalk, true);
    
    public void Idle() => _animator.SetBool(isWalk, false);
    
    public void Attack() => _animator.SetTrigger(AttackTrigger);
}
