using UnityEngine;
using UnityEngine.AI;

public class MoverNavMesh : MoverAbstract
{
	[SerializeField]
	protected NavMeshAgent agent;

    public override void Move(Vector2 directionNormalized)
    {
        throw new System.Exception("Not implemented. Use MoverNavMesh#MoveTo instead.");
    }

	public override void MoveTo(Vector2 position)
	{
		agent.SetDestination(position);
	}
}
