using UnityEngine;

public class Pawn : PawnAbstract
{
	public override void Move(Vector2 normalizedDirection)
	{
		Debug.Log(normalizedDirection);
	}
}
