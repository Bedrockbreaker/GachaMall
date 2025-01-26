using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
	public event Action OnGachaAnimationFinished;

	[SerializeField]
	private Canvas canvas;
	[SerializeField]
	private TextMeshProUGUI moneyText;
	[SerializeField]
	private Image gachaCommon;
	[SerializeField]
	private Image gachaUncommon;
	[SerializeField]
	private Image gachaRare;
	[SerializeField]
	private Image gachaEpic;
	[SerializeField]
	private Image gachaLegendary;
	[SerializeField]
	private Sprite spriteCommon;
	[SerializeField]
	private Sprite spriteUncommon;
	[SerializeField]
	private Sprite spriteRare;
	[SerializeField]
	private Sprite spriteEpic;
	[SerializeField]
	private Sprite spriteLegendary;
	[SerializeField]
	private GachaBubble gachaBubblePrefab;

	public void SetMoney(int coinsAmount)
	{
		// Always print with at least 2 decimal digits
		moneyText.text = $"${(coinsAmount / 4.0f).ToString("F2")}";
	}

	public void RevealGacha(GachaRarities rarity)
	{
		GachaBubble gachaBubble = Instantiate(gachaBubblePrefab, canvas.transform);
		gachaBubble.SetRarity(rarity);
		gachaBubble.OnOpen += OnGachaPop;
		gachaBubble.OnFinished += OnGachaFinished;
	}

	public void OnGachaPop(GachaBubble gachaBubble)
	{
		
	}

	public void OnGachaFinished(GachaBubble gachaBubble)
	{
		switch (gachaBubble.Rarity)
		{
			case GachaRarities.Common:
				gachaCommon.sprite = spriteCommon;
				break;
			case GachaRarities.Uncommon:
				gachaUncommon.sprite = spriteUncommon;
				break;
			case GachaRarities.Rare:
				gachaRare.sprite = spriteRare;
				break;
			case GachaRarities.Epic:
				gachaEpic.sprite = spriteEpic;
				break;
			case GachaRarities.Legendary:
				gachaLegendary.sprite = spriteLegendary;
				break;
			case GachaRarities.Unique:
				break;
			default:
				throw new Exception("Invalid rarity");
		}

		OnGachaAnimationFinished?.Invoke();
	}
}