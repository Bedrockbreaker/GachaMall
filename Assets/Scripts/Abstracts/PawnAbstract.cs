using System;
using UnityEngine;

public abstract class PawnAbstract : MonoBehaviour
{

	private bool isUpdatingInternal = false;

	[field: SerializeField]
	public ControllerAbstract Controller { get; protected set; }
	[field: SerializeField]
	public MoverAbstract Mover { get; protected set; }
	[field: SerializeField]
	public Transform CameraLookTarget { get; protected set; }
	public float CameraLookaheadDistance = 2.0f;
	
	public event Action<ControllerAbstract> OnControllerBound;
	public event Action OnControllerUnbound;
	public event Action OnInteract;

	public virtual bool BindController(ControllerAbstract controller)
	{
		if (isUpdatingInternal) return false;
		isUpdatingInternal = true;

		if (Controller != null) {
			OnControllerUnbound?.Invoke();
			Controller.Unpossess();
		}

		Controller = controller;

		if (Controller != null) {
			Controller.Possess(this);
			OnControllerBound?.Invoke(Controller);
		}

		isUpdatingInternal = false;
		return true;
	}

	public virtual bool UnbindController() => BindController(null);

	public virtual void Move(Vector2 directionNormalized)
	{
		if (Mover == null) return;

		CameraLookTarget.position = transform.position
			+ (Vector3)(directionNormalized * CameraLookaheadDistance);

		Mover.Move(directionNormalized);
	}

	public virtual void Interact()
	{
		OnInteract?.Invoke();
	}

	public virtual void Start()
	{
		GameManager.Instance.RegisterPawn(this);

	}

	public virtual void OnDestroy()
	{
		UnbindController();
		GameManager.Instance.UnregisterPawn(this);
	}
}
