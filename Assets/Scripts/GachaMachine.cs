using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GachaMachine : MonoBehaviour
{
    
    private bool canInteract = true;
    public event Action OnGachaCollected;

    [Header("Component References")]
    [SerializeField]
    private CanvasGroup canvasGroup;
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

    void Start()
    {
        canvasGroup.alpha = startAlpha;
        moneyText.text = $"${(coinsCost / 4.0f).ToString("F2")}";
    }

    private void Interact()
    {
        canInteract = false; // No spamming during the cutscene
        nearbyPlayer.RemoveCoins(coinsCost);

        // HACK: causes the text to turn red if they don't have enough after the interaction
        Collider2D playerCollider = nearbyPlayer.Pawn.gameObject.GetComponent<Collider2D>();
        OnTriggerExit2D(playerCollider);
        OnTriggerEnter2D(playerCollider);

        // TODO: check if player has acquired all gachas

        GachaDrops drop = choose_gacha();
        GachaBubble gachaBubble = Instantiate(
            gachaBubblePrefab,
            transform.position,
            Quaternion.identity
        );

        gachaBubble.BottomHalf.color = drop.color;

        // TODO: play sound
        // TODO: wait for animation

        OnGachaCollected?.Invoke();
        canInteract = true;
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

        throw new System.Exception("No gacha available, this should never happen during gameplay");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<PawnHuman>(out var human)) return;
        ControllerPlayer player = human.Controller as ControllerPlayer;
        if (player == null) return;

        if (player.Coins < coinsCost)
        {
            moneyText.color = insufficentFundsColor;
        }
        else
        {
            moneyText.color = Color.white;
            nearbyPlayer = player;
            if (canInteract) human.OnInteract += Interact;
        }

        StartCoroutine(FadeCanvas(1f));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (gameObject.activeSelf == false) return;
        if (!other.TryGetComponent<PawnHuman>(out var human)) return;
        nearbyPlayer = null;
        human.OnInteract -= Interact;
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
}

[System.Serializable]
public class GachaDrops{
    public string name;
    public int drop_weight;
    public Color color;

}