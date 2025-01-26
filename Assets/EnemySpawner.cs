using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private List<Transform> Spawners= new List<Transform>();
    public GameObject CommonSpawn;
    public GameObject UncommonSpawn;
    public GameObject RareSpawn;
    public GameObject EpicSpawn;
    public GameObject LegendarySpawn;
    //wendigo, kappa, krasu, mari, mothman
    private  List<bool> SpawnedRarity = new List<bool>{false, false, false, false, false};

    public void Start(){
        for (int i = 0; i<transform.childCount; i++){
           Spawners.Add(transform.GetChild(i));
        }
        
    }
    public void SpawnEnemy(GachaRarities rarity){
        GameObject Spawned;
        int index = UnityEngine.Random.Range(0, Spawners.Count-1);
        print(Spawners.Count);
        print(index);
        print("create enemy");
        Transform Spawner = Spawners[index];
 
        switch (rarity){
			case GachaRarities.Common:
				 if (!SpawnedRarity[0]){
                    Spawned = Instantiate(
                        CommonSpawn,
                        Spawner.position,
                        Spawner.rotation
                    );
                    SpawnedRarity[0]=true;
                }
				break;
			case GachaRarities.Uncommon:
				 if (!SpawnedRarity[1]){
                    Spawned = Instantiate(
                        UncommonSpawn,
                        Spawner.position,
                        Spawner.rotation
                    );
                    SpawnedRarity[1]=true;
                }
                break;
			case GachaRarities.Rare:
				 if (!SpawnedRarity[2]){
                    Spawned = Instantiate(
                        CommonSpawn,
                        Spawner.position,
                        Spawner.rotation
                    );
                    SpawnedRarity[2]=true;
                }
                break;
			case GachaRarities.Epic:
                if (!SpawnedRarity[3]){
                    Spawned = Instantiate(
                        EpicSpawn,
                        Spawner.position,
                        Spawner.rotation
                    );
                    SpawnedRarity[3]=true;
                }
                break;
			case GachaRarities.Legendary:
                if (!SpawnedRarity[4]){
                    Spawned = Instantiate(
                        LegendarySpawn,
                        Spawner.position,
                        Spawner.rotation
                    );
                    SpawnedRarity[4]=true;
                }
				break;
			default:
				throw new Exception("Invalid rarity");
		}
    }


}
