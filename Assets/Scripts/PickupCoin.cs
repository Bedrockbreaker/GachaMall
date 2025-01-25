using UnityEngine;

public class PickupCoin : MonoBehaviour
{
	public int coinValue = 1;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.TryGetComponent<PawnHuman>(out var human)) return;
		ControllerPlayer player = human.Controller as ControllerPlayer;
		if (player == null) return;
		player.AddCoins(coinValue);
		// TODO: play sound
		Destroy(gameObject);
	}
}
