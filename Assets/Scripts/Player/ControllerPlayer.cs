using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class ControllerPlayer : ControllerAbstract
{

	private bool allowInput = true;
	public bool AllowInput {
		get => allowInput;
		set {
			allowInput = value;
			Pawn.Move(Vector2.zero); // stop movement animation
		}
	}

	protected InputAction inputMove;
	protected InputAction inputInteract;
#if UNITY_EDITOR
	protected InputAction inputDebug;
#endif

	[SerializeField]
	protected GUI gui;
	[SerializeField]
	protected GameObject RespawnPoint;
	[field: SerializeField]
	public List<GachaRarities> CollectedRarities { get; } = new();

	[SerializeField]
	private AudioClip newGachaSound;

	protected override void HandleInput()
	{
		if (!AllowInput) return;
		if (Pawn == null) return;
		Vector2 rawDirection = inputMove.ReadValue<Vector2>();
		if (rawDirection.sqrMagnitude > 1f) rawDirection.Normalize();
		Pawn.Move(rawDirection);
	}

	protected virtual void HandleInteract(InputAction.CallbackContext context)
	{
		if (!AllowInput) return;
		if (Pawn == null) return;
		Pawn.Interact();
	}

#if UNITY_EDITOR
	protected virtual void HandleDebug(InputAction.CallbackContext context)
	{
		AddCoins(100);
	}
#endif

	public virtual int AddCoins(int count)
    {
        Assert.IsTrue(count > 0);
        Coins += count;
		gui.SetMoney(Coins);
        return Coins;
    }

    public virtual int RemoveCoins(int count)
    {
        Assert.IsTrue(count >= 0);
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

	public virtual void CollectRarity(GachaRarities rarity)
	{
		if (CollectedRarities.Contains(rarity)) return;
		CollectedRarities.Add(rarity);
		GameManager.Instance.PlayOneShot(newGachaSound);
	}

	public override void Start()
	{
		base.Start();

		inputMove = InputSystem.actions.FindAction("Move");
		inputInteract = InputSystem.actions.FindAction("Interact");
		inputInteract.performed += HandleInteract;
#if UNITY_EDITOR
		inputDebug = InputSystem.actions.FindAction("Jump");
		inputDebug.performed += HandleDebug;
#endif
		gui.SetMoney(Coins);
	}

    public override void OnDestroy()
    {
        base.OnDestroy();

		inputInteract.performed -= HandleInteract;
    }
}
