using UnityEngine;

public abstract class MoverAbstract : MonoBehaviour
{
	[SerializeField]
	new protected Rigidbody2D rigidbody;

	public abstract void Move(Vector2 directionNormalized);

	public abstract void MoveTo(Vector2 position);
}
