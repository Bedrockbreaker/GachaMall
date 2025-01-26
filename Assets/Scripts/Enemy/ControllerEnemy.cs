using UnityEngine;
using UnityEngine.AI;

public class ControllerEnemy : MonoBehaviour
{
    //public
    public PawnEnemy Pawn;
    public GameObject target;

    public NavMeshAgent agent;
    public GachaRarities rarity;
    
    //private
	public void Start()
	{
        if (agent == null) return;
        agent.updateRotation=false;
        agent.updateUpAxis=false;

        if(agent){
            ((MoverBasic)Pawn.Mover).speed=agent.speed;
        }
	}
    public void Update()
    {     
        if (target){
            Pawn.target_state();
        }else{
            Pawn.default_state();
        }
    }
}
