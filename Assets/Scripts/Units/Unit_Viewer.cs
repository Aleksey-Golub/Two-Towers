using UnityEngine;

public class Unit_Viewer : MonoBehaviour
{
    [SerializeField] private Animator _anim_control;
    [SerializeField] private Unit _unit;
    string[]
        _animS_type = new string[] { "Fight", "Walk", "Stay", "Death" };
    int
        _anim_old_index = 2; // 2- индекс "Stay", состояние по умолчанию в Аниматоре

    /// <summary>
    /// 0 - Fight | 1 - Walk | 2 - Stay | 3 - Death
    /// </summary>
    /// <param name="number"></param>
    public void AnimatorChanger(int number)
    {
        if (number < 0 || number > _animS_type.Length - 1)
            return;

        _anim_control.SetBool(_animS_type[_anim_old_index], false);
        _anim_control.SetBool(_animS_type[number], true);

        _anim_old_index = number;
    }

    public void OnAttackAnimationFinishedEvent()
    {
        _unit.OffAttacking();
    }

    public void OnAttackAnimationEvent()
    {
        _unit.DamageTarget();
    }
}
