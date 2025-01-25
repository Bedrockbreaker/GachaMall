using UnityEngine;

public class MoverBasic : MoverAbstract
{
	public float speed = 10.0f;

    public override void Move(Vector2 directionNormalized)
    {
        Vector2 delta = speed * Time.deltaTime * directionNormalized;
		rigidbody.MovePosition(rigidbody.position + delta);
    }
}
