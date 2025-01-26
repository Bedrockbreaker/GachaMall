using System.Collections;
using System.Collections.Generic;
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
	private CutsceneHelper cutsceneHelper;
	[SerializeField]
	private float coinSpawnDelay = 15f;
	[SerializeField]
	private int maxCoinSpawns = 4;
	[SerializeField]
	public EnemySpawner EnemySpawner;


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
		StartGame();
	}

	public void StartGame()
	{
		StartCoroutine(SpawnCoin());
		gachaMachine.OnGachaCollected += OnGachaCollected;
		gui.OnGachaAnimationFinished += OnGachaAnimationFinished;
		gui.OnFinalCutsceneFullyFaded += OnFinalCutsceneFullyFaded;
		gui.OnFinalCutsceneFinished += () => StartCoroutine(OnFinalCutsceneFinished());
		cutsceneHelper.SayText();
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
		EnemySpawner.SpawnEnemy(drop.rarity);
	}

	private void OnGachaAnimationFinished(GachaBubble gachaBubble)
	{
		if (gachaBubble.Rarity == GachaRarities.Unique) return;
		Player.AllowInput = true;
	}

	public void PlayOneShot(AudioClip clip) => audioSource.PlayOneShot(clip);

	public void OnFinalCutsceneFullyFaded()
	{
		Destroy(Player.Pawn.gameObject);
		// TODO: kill all enemy pawns
	}

	public IEnumerator OnFinalCutsceneFinished()
	{
		mainCamera.target = cutsceneHelper.transform;
		cutsceneHelper.SpawnPlayer2();
		cutsceneHelper.SayText();

		yield return new WaitForSeconds(5f);

		Application.Quit();
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}
}
