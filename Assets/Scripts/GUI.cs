using TMPro;
using UnityEngine;

public class GUI : MonoBehaviour
{
	[field: SerializeField]
	public TextMeshProUGUI MoneyText { get; protected set; }

	public void SetMoney(int coinsAmount)
	{
		// Always print with at least 2 decimal digits
		MoneyText.text = $"${(coinsAmount / 4.0f).ToString("F2")}";
	}
}