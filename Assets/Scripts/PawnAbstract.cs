using System;
using UnityEngine;

public abstract class PawnAbstract : MonoBehaviour
{

	private bool isUpdatingInternal = false;

	[field: SerializeField]
	public ControllerAbstract Controller { get; protected set; }
	[field: SerializeField]
	public MoverAbstract Mover { get; protected set; }

	public event Action<ControllerAbstract> OnControllerBound;
	public event Action OnControllerUnbound;

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
		Mover.Move(directionNormalized);
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
