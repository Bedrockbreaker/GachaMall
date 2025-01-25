using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class ControllerPlayer : ControllerAbstract
{

	protected InputAction inputMove;
	protected InputAction inputInteract;

	protected override void HandleInput()
	{
		if (Pawn == null) return;
		Vector2 rawDirection = inputMove.ReadValue<Vector2>();
		Pawn.Move(rawDirection.normalized);
	}

	protected virtual void HandleInteract(InputAction.CallbackContext context)
	{
		if (Pawn == null) return;
		// TODO: Pawn.Interact();
		Debug.Log("Interact");
	}

	public virtual int AddCoins(int count)
    {
        Assert.IsTrue(count > 0);
        Coins += count;
        return Coins;
    }

    public virtual int RemoveCoins(int count)
    {
        Assert.IsTrue(count > 0);
        // TODO: kill the player if not enough?
        Coins -= count;
        return Coins;
    }

	public override void Start()
	{
		base.Start();

		inputMove = InputSystem.actions.FindAction("Move");
		inputInteract = InputSystem.actions.FindAction("Interact");
		inputInteract.performed += HandleInteract;
	}

    public override void OnDestroy()
    {
        base.OnDestroy();

		inputInteract.performed -= HandleInteract;
    }
}
