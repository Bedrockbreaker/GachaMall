using TMPro;
using UnityEngine;

public class DialogueCanvas : MonoBehaviour
{
	public TextMeshProUGUI dialogue;

	public float timer = 5f;

	public void Update()
	{
		dialogue.color = new (1, 1, 1, timer / 5f);
		transform.position += 0.25f * Time.deltaTime * Vector3.up;
		timer -= Time.deltaTime;

		if (timer < 0)
		{
			Destroy(gameObject);
		}
	}
}