using UnityEngine;
using UnityEngine.Rendering;

public class SmoothCamera : MonoBehaviour
{
	[SerializeField]
	private Camera thisCamera;
	[SerializeField]
	private Volume postProcssing;
	[SerializeField]
	private Collider2D bounds;

	public Transform target;
	public float smoothSpeed = 0.25f;

	private void Start()
	{
		postProcssing.enabled = true;
	}

	private void LateUpdate()
	{
		float cameraHeight = thisCamera.orthographicSize * 2;
		float cameraWidth = cameraHeight * thisCamera.aspect;

		Vector2 desiredPosition = new(
			Mathf.Clamp(
				target.position.x,
				bounds.bounds.min.x + cameraWidth / 2,
				bounds.bounds.max.x - cameraWidth / 2
			),
			Mathf.Clamp(
				target.position.y,
				bounds.bounds.min.y + cameraHeight / 2,
				bounds.bounds.max.y - cameraHeight / 2
			)
		);

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
