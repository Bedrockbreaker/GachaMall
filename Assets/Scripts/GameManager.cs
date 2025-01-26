using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	private List<PawnAbstract> Pawns { get; } = new();
	private List<ControllerAbstract> Controllers { get; } = new();
	private List<CoinSpawner> CoinSpawners { get; } = new();
	private List<CoinSpawner> ValidCoinSpawners { get; } = new();
	public ControllerPlayer Player { get; private set; }
	[SerializeField]
	private AudioSource audioSource;
	[SerializeField]
	private SmoothCamera mainCamera;
	[SerializeField]
	private GUI gui;
	[SerializeField]
	private GachaMachine gachaMachine;
	[SerializeField]
	private float coinSpawnDelay = 15f;
	[SerializeField]
	private int maxCoinSpawns = 4;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	private void Start()
	{
		// TODO: only start game after play button is pressed
		StartGame();
	}

	public void StartGame()
	{
		StartCoroutine(SpawnCoin());
		gachaMachine.OnGachaCollected += OnGachaCollected;
		gui.OnGachaAnimationFinished += OnGachaAnimationFinished;
	}

	public void RegisterController(ControllerAbstract controller)
	{
		if (!Controllers.Contains(controller)) Controllers.Add(controller);

		if (controller is not ControllerPlayer player) return;

		Player = player;

		PawnAbstract playerPawn = player.Pawn;
		if (playerPawn == null) return;

		mainCamera.target = playerPawn.CameraLookTarget;
	}

	public void UnregisterController(ControllerAbstract controller)
	{
		Controllers.Remove(controller);
	}

	public void RegisterPawn(PawnAbstract pawn)
	{
		if (!Pawns.Contains(pawn)) Pawns.Add(pawn);
	}

	public void UnregisterPawn(PawnAbstract pawn)
	{
		Pawns.Remove(pawn);
	}

	public void SpawnPawn(
		PawnAbstract pawnPrefab,
		ControllerAbstract controller,
        Vector2 spawnPoint,
		bool controllerIsPrefab
	) {
#if UNITY_EDITOR
		if (spawnPoint == null)
		{
			SceneView sceneView = SceneView.lastActiveSceneView;
			spawnPoint = sceneView.pivot
				- sceneView.rotation * Vector3.forward * sceneView.cameraDistance;
		}
#endif

		PawnAbstract pawn = Instantiate(
			pawnPrefab,
			new Vector3(spawnPoint.x, spawnPoint.y, 0),
			Quaternion.identity
		);

		if (controllerIsPrefab)
		{
			controller = Instantiate(
				controller,
				new Vector3(spawnPoint.x, spawnPoint.y, 0),
				Quaternion.identity
			);
		}

		controller.Possess(pawn);
	}

	public void RegisterCoinSpawner(CoinSpawner coinSpawner)
	{
		if (!CoinSpawners.Contains(coinSpawner)) CoinSpawners.Add(coinSpawner);
		if (!ValidCoinSpawners.Contains(coinSpawner) && coinSpawner.CanSpawnCoin)
		{
			ValidCoinSpawners.Add(coinSpawner);
		}
	}

	public void UnregisterCoinSpawner(CoinSpawner coinSpawner)
	{
		CoinSpawners.Remove(coinSpawner);
		ValidCoinSpawners.Remove(coinSpawner);
	}

	private IEnumerator SpawnCoin()
	{
		yield return new WaitForSeconds(coinSpawnDelay);

		if (ValidCoinSpawners.Count != 0 && maxCoinSpawns > 0)
		{
			CoinSpawner spawner = ValidCoinSpawners[Random.Range(0, ValidCoinSpawners.Count)];
			spawner.SpawnCoin();
			maxCoinSpawns--;

			ValidCoinSpawners.Remove(spawner);
		}

		StartCoroutine(SpawnCoin());
	}

	private void OnGachaCollected(GachaDrops drop)
	{
		Player.AllowInput = false;

		foreach (CoinSpawner coinSpawner in CoinSpawners)
		{
			if (coinSpawner.ResetSpawner())
			{
				maxCoinSpawns++;
				ValidCoinSpawners.Add(coinSpawner);
			}
		}

		gui.RevealGacha(drop.rarity);
	}

	private void OnGachaAnimationFinished() => Player.AllowInput = true;

	public void PlayOneShot(AudioClip clip) => audioSource.PlayOneShot(clip);
}
