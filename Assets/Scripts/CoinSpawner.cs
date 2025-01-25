using UnityEngine;

public class CoinSpawner : MonoBehaviour
{

	[field: SerializeField]
	public bool CanSpawnCoin { get; private set; } = true;

	[SerializeField]
	private PickupCoin coinPrefab;

	private PickupCoin spawnedCoin;

	public bool SpawnCoin()
	{
		if (!CanSpawnCoin) return false;
		CanSpawnCoin = false;

		spawnedCoin = Instantiate(
			coinPrefab,
			transform.position,
			Quaternion.identity
		);

		return true;
	}

	public bool ResetSpawner()
	{
		if (spawnedCoin == null && !CanSpawnCoin)
		{
			CanSpawnCoin = true;
			return true;
		}

		return false;
	}

	private void Start()
	{
		GameManager.Instance.RegisterCoinSpawner(this);
	}

	private void OnDestroy()
	{
		GameManager.Instance.UnregisterCoinSpawner(this);
	}
}