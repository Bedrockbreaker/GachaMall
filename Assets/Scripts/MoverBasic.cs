using UnityEngine;

public class MoverBasic : MoverAbstract
{
	public float speed = 2.0f;

    public override void Move(Vector2 directionNormalized)
    {
        Vector2 delta = speed * directionNormalized;
        rigidbody.linearVelocity = delta;
    }

    public override void MoveTo(Vector2 position)
    {
        throw new System.Exception("Not implemented. Use MoverBasic#Move instead.");
    }
}
