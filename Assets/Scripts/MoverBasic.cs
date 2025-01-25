using UnityEngine;

public class MoverBasic : MoverAbstract
{
	public float speed = 2.0f;

    public override void Move(Vector2 directionNormalized)
    {
        Vector2 delta = speed * directionNormalized;
        rigidbody.linearVelocity = delta;
    }
}
