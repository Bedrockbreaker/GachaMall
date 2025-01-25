using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	private List<PawnAbstract> Pawns { get; } = new();
	private List<ControllerAbstract> Controllers { get; } = new();

	public GameManager()
	{
		if (Instance != null) return;
		Instance = this;
	}

	private void Awake()
	{
		if (Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		else
		{
			DontDestroyOnLoad(gameObject);
		}
	}

	public void StartGame()
	{
		// TODO: stub, start game
		Debug.Log("Game started");
	}

	public void RegisterController(ControllerAbstract controller)
	{
		if (!Controllers.Contains(controller)) Controllers.Add(controller);

		// TODO: player controller specific stuff?
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
}