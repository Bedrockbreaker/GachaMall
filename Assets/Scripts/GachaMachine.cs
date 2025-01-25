using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GachaMachine : MonoBehaviour
{
    [Header("Gacha Drops")]
    [SerializeField] private SpriteRenderer gacha_bubble;
    [SerializeField] private GachaDrops[] drops;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        choose_gatcha();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void choose_gatcha(){
        int total_chance = 0;
        foreach (GachaDrops drop in drops){
            total_chance += drop.drop_weight;
        }

        int random_gacha = Random.Range(0, total_chance);
        int cumlative_chance = 0;

        foreach (GachaDrops drop in drops){
            cumlative_chance += drop.drop_weight;

            //select gacha
            if (random_gacha <= cumlative_chance){
                gacha_bubble.color = drop.color;
                
                //reduce weight
                if (drop.drop_weight>1){
                    drop.drop_weight-=1;
                }

                //exit 
                return;
            }
        }
    }
}

[System.Serializable]
public class GachaDrops{
    public string name;
    public int drop_weight;
    public Color color;

}