using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
	public Transform target;
	public float smoothSpeed = 0.25f;

	private void LateUpdate()
	{
		Vector2 desiredPosition = target.position;

		Vector2 smoothedPosition = Vector2.Lerp(
			transform.position,
			desiredPosition,
			1 - Mathf.Pow(smoothSpeed, Time.deltaTime)
		);

		transform.position = new(
			smoothedPosition.x,
			smoothedPosition.y,
			transform.position.z
		);
	}
}
