using UnityEngine;

public class PawnHuman : PawnAbstract
{

	[field: SerializeField]
	public SpriteRenderer SpriteRenderer { get; protected set; }
	[field: SerializeField]
	public Animator Animator { get; protected set; }

    public override void Move(Vector2 directionNormalized)
    {
        base.Move(directionNormalized);

		Animator.SetBool("isMoving", directionNormalized.sqrMagnitude > 0.01f);
		if (directionNormalized.x != 0)
		{
			SpriteRenderer.flipX = directionNormalized.x < 0;
		}
    }
}
