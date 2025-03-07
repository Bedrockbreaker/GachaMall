using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GachaMachine : MonoBehaviour
{
    public event Action<GachaDrops> OnGachaCollected;

    [Header("Component References")]
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Image controlsImage;
    [SerializeField]
    private TextMeshProUGUI moneyText;
    private ControllerPlayer nearbyPlayer;
    [SerializeField]
    private GachaBubble gachaBubblePrefab;

    [Header("Gacha")]
    public int coinsCost = 4;
    [SerializeField] private GachaDrops[] drops;

    [Header("Animations")]
    // Reddish salmon
    public Color insufficentFundsColor = new (0.9716981f, 0.3895959f, 0.3895959f, 1.0f);
    public float interactionTransitionTime = 0.5f;
    public float startAlpha = 0f;
    [SerializeField]
    private Sprite interactE;
    [SerializeField]
    private Sprite interactFaceButtonSouth;

    [Header("Sound")]
    [SerializeField]
    private AudioClip interactSound;
    [SerializeField]
    private AudioClip insufficientFundsSound;
    [SerializeField]
    private AudioClip sufficientFundsSound;
    [SerializeField]
    private AudioClip gachaBuildUpSound;

    void Start()
    {
        canvasGroup.alpha = startAlpha;
        moneyText.text = $"${(coinsCost / 4.0f).ToString("F2")}";
        InputSystem.actions.FindAction("Move").performed += UpdateControlsImage;
    }

    private void Interact()
    {
        nearbyPlayer.RemoveCoins(coinsCost);

        GachaDrops drop;

        if (nearbyPlayer.CollectedRarities.Count == 5)
        {
            drop = new GachaDrops {
                drop_weight = 0,
                rarity = GachaRarities.Unique
            };
        }
        else {
            drop = choose_gacha();
        }
        
        OnGachaCollected?.Invoke(drop);

        GameManager.Instance.PlayOneShot(interactSound);
        GameManager.Instance.PlayOneShot(sufficientFundsSound);
        GameManager.Instance.PlayOneShot(gachaBuildUpSound);

        if (nearbyPlayer.Coins < coinsCost)
        {
            moneyText.color = insufficentFundsColor;
            nearbyPlayer.Pawn.OnInteract -= Interact;
            nearbyPlayer.Pawn.OnInteract += BadInteract;
            nearbyPlayer = null;
        }
    }

    private void BadInteract()
    {
        GameManager.Instance.PlayOneShot(insufficientFundsSound);
    }

    private GachaDrops choose_gacha() {
        int total_chance = 0;
        foreach (GachaDrops drop in drops){
            total_chance += drop.drop_weight;
        }

        int random_gacha = UnityEngine.Random.Range(0, total_chance);

        foreach (GachaDrops drop in drops){
            random_gacha -= drop.drop_weight;

            //select gacha
            if (random_gacha < 0){

                //reduce weight
                if (drop.drop_weight>1){
                    drop.drop_weight-=1;
                }

                //exit 
                return drop;
            }
        }

        throw new Exception("No gacha available, this should never happen during gameplay");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<PawnHuman>(out var human)) return;
        ControllerPlayer player = human.Controller as ControllerPlayer;
        if (player == null) return;

        if (player.Coins < coinsCost)
        {
            moneyText.color = insufficentFundsColor;
            human.OnInteract += BadInteract;
        }
        else
        {
            moneyText.color = Color.white;
            if (nearbyPlayer == null) human.OnInteract += Interact;
            nearbyPlayer = player;
        }

        StartCoroutine(FadeCanvas(1f));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (gameObject.activeSelf == false) return;
        if (!other.TryGetComponent<PawnHuman>(out var human)) return;
        nearbyPlayer = null;
        human.OnInteract -= Interact;
        human.OnInteract -= BadInteract;
        ControllerPlayer player = human.Controller as ControllerPlayer;
        if (player == null) return;

        StartCoroutine(FadeCanvas(0f));
    }

    private IEnumerator FadeCanvas(float targetAlpha)
    {
        float elapsedTime = 0f;
        while (elapsedTime < interactionTransitionTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.SmoothStep(
                startAlpha,
                targetAlpha,
                elapsedTime / interactionTransitionTime
            );

            canvasGroup.alpha = newAlpha;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        startAlpha = targetAlpha;
    }

    private void UpdateControlsImage(InputAction.CallbackContext context)
    {
        bool isGamepad = context.control.device is Gamepad;
        if (isGamepad)
        {
            controlsImage.sprite = interactFaceButtonSouth;
        }
        else
        {
            controlsImage.sprite = interactE;
        }
    }
}

[Serializable]
public class GachaDrops{
    public GachaRarities rarity;
    public int drop_weight;
}