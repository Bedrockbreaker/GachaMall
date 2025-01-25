using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class ControllerEnemy : MonoBehaviour
{
    //public
    public PawnEnemy Pawn;
    public GameObject target;
    
    //private
	public void Start()
	{

	}
    public void Update()
    {
        if (target){
            Pawn.target_state();
        }else{
            Pawn.default_state();
        }
        Pawn.debug_sight();
    }
}
