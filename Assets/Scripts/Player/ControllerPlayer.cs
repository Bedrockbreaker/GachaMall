using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class ControllerPlayer : ControllerAbstract
{

	protected InputAction inputMove;
	protected InputAction inputInteract;

	[SerializeField]
	protected GUI gui;
	[SerializeField]
	protected GameObject RespawnPoint;

	protected override void HandleInput()
	{
		if (Pawn == null) return;
		Vector2 rawDirection = inputMove.ReadValue<Vector2>();
		if (rawDirection.sqrMagnitude > 1f) rawDirection.Normalize();
		Pawn.Move(rawDirection);
	}

	protected virtual void HandleInteract(InputAction.CallbackContext context)
	{
		if (Pawn == null) return;
		Pawn.Interact();
	}

	public virtual int AddCoins(int count)
    {
        Assert.IsTrue(count > 0);
        Coins += count;
		gui.SetMoney(Coins);
        return Coins;
    }

    public virtual int RemoveCoins(int count)
    {
        Assert.IsTrue(count > 0);
        // TODO: kill the player if not enough?
        Coins -= count;
		gui.SetMoney(Coins);
        return Coins;
    }

	public virtual void Die(){
		if (Coins>0){
			RemoveCoins(Coins);
		}
		Pawn.transform.position = RespawnPoint.transform.position;
	}

	public override void Start()
	{
		base.Start();

		inputMove = InputSystem.actions.FindAction("Move");
		inputInteract = InputSystem.actions.FindAction("Interact");
		inputInteract.performed += HandleInteract;
		gui.SetMoney(Coins);
	}

    public override void OnDestroy()
    {
        base.OnDestroy();

		inputInteract.performed -= HandleInteract;
    }
}
