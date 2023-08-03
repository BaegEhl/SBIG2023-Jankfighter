using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] private GameObject[] waves;
    [SerializeField] private Vector2[] spawnpoints;
    [SerializeField] private float[] spawnTimes;
    [SerializeField] private List<GameObject> aliveEnemies;
    [SerializeField] private GameObject[] bossAssets;
    [SerializeField] private float timer;
    [SerializeField] private GameObject bossCutscene;
    // Start is called before the first frame update
    public void startWaves(){
        StartCoroutine(spawnManage());
    }
    IEnumerator spawnManage(){
        for(int i = 0; i < waves.Length; i++){
            timer = spawnTimes[i];
            while(timer > 0){yield return new WaitForEndOfFrame();}
            GameObject enemy = Instantiate(waves[i],spawnpoints[i],transform.rotation, enemyContainer.transform);
            aliveEnemies.Add(enemy.GetComponentInChildren<EnemyAI>().gameObject);
        }
        foreach(GameObject obj in bossAssets){
            obj.SetActive(true);
        }
        aliveEnemies.Add(bossAssets[0].GetComponentInChildren<EnemyAI>().gameObject);
        while(aliveEnemies.Count > 0){yield return new WaitForEndOfFrame();}
        Destroy(enemyContainer);
        bossCutscene.SetActive(true);
    }
    void Update(){
        timer -= Time.deltaTime;
        for(int i = 0; i < aliveEnemies.Count; i++){
            if(aliveEnemies[i] == null){
                aliveEnemies.RemoveAt(i);
            }
        }
        if(timer > 5 && aliveEnemies.Count == 0){
            timer = 5;
        }
    }
}
