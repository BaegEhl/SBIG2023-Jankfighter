using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] private GameObject[][] waves;
    [SerializeField] private Vector2[][] spawnpoints;
    [SerializeField] private float[][] spawnTimes;
    [SerializeField] private float[] waveTimers;
    private List<GameObject> aliveEnemies;
    [SerializeField] private GameObject[] bossAssets;
    private float timer;
    private int spawnCount;
    [SerializeField] private GameObject bossCutscene;
    // Start is called before the first frame update
    public void startWaves(){
        StartCoroutine(spawnManage());
    }
    IEnumerator spawnManage(){
        int wave = 0;
        while(wave < waves.Length){
            spawnCount = waves[wave].Length;
            for(int i = 0; i < waves[wave].Length; i++){
                yield return new WaitForSeconds(spawnTimes[wave][i]);
                GameObject enemy = Instantiate(waves[wave][i],spawnpoints[wave][i],transform.rotation, enemyContainer.transform);
                aliveEnemies.Add(enemy.GetComponentInChildren<EnemyAI>().gameObject);
                spawnCount--;
            }
            wave++;
            timer = waveTimers[wave];
            while(timer > 0){yield return new WaitForEndOfFrame();}
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
        if(spawnCount == 0 && timer > 5 && aliveEnemies.Count == 0){
            timer = 5;
        }
    }
}
