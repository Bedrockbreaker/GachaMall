using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class ControllerEnemy : ControllerAbstract
{
    //public
    public float sight_range=25;
    public float chase_timeout = 5;

    public GameObject target;

    //private
    private bool target_seen=false;
    private float chase_timer=0;

    protected override void HandleInput()
	{

    }
	public override void Start()
	{

	}
    public override void Update()
    {
        if (target){
            Vector2 direction = target.transform.position - Pawn.transform.position;            

            Pawn.Move(direction.normalized);
            chase_timer+=Time.deltaTime;

            //give up
            if(chase_timeout<chase_timer){
                target=null;
                chase_timer=0;
            }
        }else{
            Vector2 forward_direction = Pawn.transform.up*sight_range;
            target=((PawnEnemy)Pawn).seen_target(forward_direction);
        }


    }

    public override void OnDestroy()
    {
    }
}
