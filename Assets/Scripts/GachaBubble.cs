using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaBubble : MonoBehaviour
{
	public event Action<GachaBubble> OnOpen;
	public event Action<GachaBubble> OnFinished;

	[field: SerializeField]
	public GachaRarities Rarity { get; private set; } = GachaRarities.Common;
	[SerializeField]
	private Image topHalf;
	[SerializeField]
	private RectTransform topHalfRect;
	[SerializeField]
	private Image bottomHalf;
	[SerializeField]
	private RectTransform bottomHalfRect;
	[SerializeField]
	private Image plush;
	[SerializeField]
	private TextMeshProUGUI collectedText;
	[SerializeField]
	private TextMeshProUGUI finalText;
	[SerializeField]
	private Sprite spriteBottomOpen;
	[SerializeField]
	private Sprite spriteCommonTop;
	[SerializeField]
	private Sprite spriteCommonOpen;
	[SerializeField]
	private Sprite spriteCommonBottom;
	[SerializeField]
	private Sprite spriteCommonPlush;
	[SerializeField]
	private Sprite spriteUncommonTop;
	[SerializeField]
	private Sprite spriteUncommonOpen;
	[SerializeField]
	private Sprite spriteUncommonBottom;
	[SerializeField]
	private Sprite spriteUncommonPlush;
	[SerializeField]
	private Sprite spriteRareTop;
	[SerializeField]
	private Sprite spriteRareOpen;
	[SerializeField]
	private Sprite spriteRareBottom;
	[SerializeField]
	private Sprite spriteRarePlush;
	[SerializeField]
	private Sprite spriteEpicTop;
	[SerializeField]
	private Sprite spriteEpicOpen;
	[SerializeField]
	private Sprite spriteEpicBottom;
	[SerializeField]
	private Sprite spriteEpicPlush;
	[SerializeField]
	private Sprite spriteLegendaryTop;
	[SerializeField]
	private Sprite spriteLegendaryOpen;
	[SerializeField]
	private Sprite spriteLegendaryBottom;
	[SerializeField]
	private Sprite spriteLegendaryPlush;
	[SerializeField]
	private Sprite spriteUniqueTop;
	[SerializeField]
	private Sprite spriteUniqueOpen;
	[SerializeField]
	private Sprite spriteUniqueBottom;
	[SerializeField]
	private Sprite spriteUniquePlush;
	[SerializeField]
	private AudioClip gachaOpen;
	[SerializeField]
	private AudioClip gachaFanfare;

	public void Start()
	{
		SetRarity(Rarity);
		finalText.enabled = 
			GameManager.Instance.Player.CollectedRarities.Count == 4
			&& !GameManager.Instance.Player.CollectedRarities.Contains(Rarity);
	}

	// Referenced by timeline signal
	public void SetRarity_Internal()
	{
		SetRarity(Rarity);
	}

	// Referenced by timeline signal
	public void Open_Internal()
	{
		OnOpen?.Invoke(this);

		bottomHalf.sprite = spriteBottomOpen;
		bottomHalfRect.rect.Set(
			bottomHalfRect.rect.x,
			bottomHalfRect.rect.y,
			bottomHalfRect.rect.width,
			bottomHalfRect.rect.height * 2
		);
        topHalf.sprite = Rarity switch
        {
            GachaRarities.Common => spriteCommonOpen,
            GachaRarities.Uncommon => spriteUncommonOpen,
            GachaRarities.Rare => spriteRareOpen,
            GachaRarities.Epic => spriteEpicOpen,
            GachaRarities.Legendary => spriteLegendaryOpen,
            GachaRarities.Unique => spriteUniqueOpen,
            _ => throw new Exception("Invalid rarity")
        };
		topHalfRect.rect.Set(
			topHalfRect.rect.x,
			topHalfRect.rect.y,
			topHalfRect.rect.width,
			topHalfRect.rect.height * 2
		);

		plush.sprite = Rarity switch
		{
			GachaRarities.Common => spriteCommonPlush,
			GachaRarities.Uncommon => spriteUncommonPlush,
			GachaRarities.Rare => spriteRarePlush,
			GachaRarities.Epic => spriteEpicPlush,
			GachaRarities.Legendary => spriteLegendaryPlush,
			GachaRarities.Unique => spriteUniquePlush,
			_ => throw new Exception("Invalid rarity")
		};

		GameManager.Instance.PlayOneShot(gachaOpen);
		GameManager.Instance.PlayOneShot(gachaFanfare);
		GameManager.Instance.Player.CollectRarity(Rarity);
    }

	// Referenced by timeline signal
	public void Finished_Internal()
	{
		OnFinished?.Invoke(this);
		Destroy(gameObject);
	}

	public void SetRarity(GachaRarities rarity)
	{
		Rarity = rarity;
		switch (rarity)
		{
			case GachaRarities.Common:
				topHalf.sprite = spriteCommonTop;
				bottomHalf.sprite = spriteCommonBottom;
				break;
			case GachaRarities.Uncommon:
				topHalf.sprite = spriteUncommonTop;
				bottomHalf.sprite = spriteUncommonBottom;
				break;
			case GachaRarities.Rare:
				topHalf.sprite = spriteRareTop;
				bottomHalf.sprite = spriteRareBottom;
				break;
			case GachaRarities.Epic:
				topHalf.sprite = spriteEpicTop;
				bottomHalf.sprite = spriteEpicBottom;
				break;
			case GachaRarities.Legendary:
				topHalf.sprite = spriteLegendaryTop;
				bottomHalf.sprite = spriteLegendaryBottom;
				break;
			case GachaRarities.Unique:
				topHalf.sprite = spriteUniqueTop;
				bottomHalf.sprite = spriteUniqueBottom;
				break;
			default:
				throw new Exception("Invalid rarity");
		}
	}
}