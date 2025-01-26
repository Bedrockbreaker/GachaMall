using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
	public event Action<GachaBubble> OnGachaAnimationFinished;
	public event Action OnFinalCutsceneFullyFaded;
	public event Action OnFinalCutsceneFinished;

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
	private Image deathScreen;
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
	[SerializeField]
	private GameObject finalCutscene;

	[SerializeField]
	private Sprite deathScreenCommon;
	[SerializeField]
	private Sprite deathScreenUncommon;
	[SerializeField]
	private Sprite deathScreenRare;
	[SerializeField]
	private Sprite deathScreenEpic;
	[SerializeField]
	private Sprite deathScreenLegendary;

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
				moneyText.text = "";
				gachaCommon.color = new (0, 0, 0, 0);
				gachaUncommon.color = new (0, 0, 0, 0);
				gachaRare.color = new (0, 0, 0, 0);
				gachaEpic.color = new (0, 0, 0, 0);
				gachaLegendary.color = new (0, 0, 0, 0);
				finalCutscene.SetActive(true);
				break;
			default:
				throw new Exception("Invalid rarity");
		}

		OnGachaAnimationFinished?.Invoke(gachaBubble);
	}

	public void FinalCutsceneFullyFaded_Internal()
	{
		OnFinalCutsceneFullyFaded?.Invoke();
	}

	// Referenced by timeline signal
	public void FinalCutsceneFinished_Internal()
	{
		finalCutscene.SetActive(false);
		OnFinalCutsceneFinished?.Invoke();
	}

	public void setDeathScreen(GachaRarities rarity)
	{
		switch (rarity)
		{
			case GachaRarities.Common:
				deathScreen.sprite = deathScreenCommon;
				break;
			case GachaRarities.Uncommon:
				deathScreen.sprite = deathScreenUncommon;
				break;
			case GachaRarities.Rare:
				deathScreen.sprite = deathScreenRare;
				break;
			case GachaRarities.Epic:
				deathScreen.sprite = deathScreenEpic;
				break;
			case GachaRarities.Legendary:
				deathScreen.sprite = deathScreenLegendary;
				break;
			case GachaRarities.Unique:
				break;
			default:
				throw new Exception("Invalid rarity");
		}
		deathScreen.enabled=true;
		Invoke("clearDeathScreen",3);
	}
	public void clearDeathScreen(){
		deathScreen.enabled=false;
	}
}