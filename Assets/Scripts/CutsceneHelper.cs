using UnityEngine;

public class CutsceneHelper : MonoBehaviour
{
	public Canvas canvasPrefab;
	public GameObject player2Prefab;

	public void SayText()
	{
		Instantiate(
			canvasPrefab,
			transform.position + Vector3.up * 1.2f,
			Quaternion.identity
		);
	}

	public void SpawnPlayer2()
	{
		Instantiate(player2Prefab, transform.position, Quaternion.identity);
	}
}
