using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    public virtual void enter_state(){}
    public virtual void exit_state(){}
    public virtual void update_state(){}
}
