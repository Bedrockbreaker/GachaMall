using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<Transform> Spawners= new List<Transform>();
    public NavMeshAgent CommonSpawn;
    public NavMeshAgent UncommonSpawn;
    public GameObject RareSpawn;
    public NavMeshAgent EpicSpawn;
    public GameObject LegendarySpawn;
    //wendigo, kappa, krasu, mari, mothman
    private  List<bool> SpawnedRarity = new List<bool>{false, false, false, false, false};

    public void Start(){
        for (int i = 0; i<transform.childCount; i++){
           Spawners.Add(transform.GetChild(i));
        }
        
    }
    public void SpawnEnemy(GachaRarities rarity){
        int index = UnityEngine.Random.Range(0, Spawners.Count);
        Transform Spawner = Spawners[index];
 
        switch (rarity){
			case GachaRarities.Common:
				 if (!SpawnedRarity[0]){
                    CommonSpawn.Warp(Spawner.position);
                    CommonSpawn.transform.position=Spawner.position;
                    SpawnedRarity[0]=true;
                }
				break;
			case GachaRarities.Uncommon:
				 if (!SpawnedRarity[1]){
                    UncommonSpawn.Warp(Spawner.position);
                    UncommonSpawn.transform.position=Spawner.position;
                    SpawnedRarity[1]=true;
                }
                break;
			case GachaRarities.Rare:
				 if (!SpawnedRarity[2]){
                    RareSpawn.transform.position=Spawner.position;
                    SpawnedRarity[2]=true;
                }
                break;
			case GachaRarities.Epic:
                if (!SpawnedRarity[3]){
                    EpicSpawn.Warp(Spawner.position);
                    EpicSpawn.transform.position=Spawner.position;
                    SpawnedRarity[3]=true;
                }
                break;
			case GachaRarities.Legendary:
                if (!SpawnedRarity[4]){
                    LegendarySpawn.transform.position=Spawner.position;
                    SpawnedRarity[4]=true;
                }
				break;
            case GachaRarities.Unique:
                break;
			default:
				throw new Exception("Invalid rarity");
		}
    }


}
