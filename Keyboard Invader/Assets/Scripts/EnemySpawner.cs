﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    public List<GameObject> enemyPrefabs = new List<GameObject>();
    public float spawnRate; //스폰 주기
    [SerializeField]
    private int spawnCount; //스폰 수
    

    public float levelUpCycle; //레벨업 주기

    private static float levelTimer = 0f; //레벨업 타이머


    public float distance = 20; //적 스폰 거리

    public Transform player;

    private static void Spawn()
    {
        //플레이어 위치에서 무작위방향으로 n만큼 떨어진 거리에 생성
        Vector2 pos = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))
            .normalized  * instance.distance + (Vector2)instance.player.position;

        //프리팹중에서 무작위로 선택
        int _i = Random.Range(0, instance.enemyPrefabs.Count);
        var newObj = Instantiate(instance.enemyPrefabs[_i],pos,Quaternion.identity);
        //ObjectPooler.SpawnFromPool(ObjectPooler.PoolingType.Enemy, pos);
    }

    private static IEnumerator spawnCycle;
    private static IEnumerator SpawnCycle()
    {
        while (true)
        {
            //주기적으로 적 스폰
            yield return new WaitForSeconds(instance.spawnRate);
            if (GameState.current == GameStateType.Playing)
            {
                for (int i = 0; i < instance.spawnCount; i++)
                {
                    Spawn();
                }
                //레벨업 타이머
                levelTimer += instance.spawnRate;
                if (levelTimer > instance.levelUpCycle)
                {
                    instance.spawnCount++;
                    levelTimer -= instance.spawnRate;
                }
            }
        }
    }

    public static void StartSpawn()
    {
        spawnCycle = SpawnCycle();
        instance.StartCoroutine(spawnCycle);

    }
    public static void StopSpawn()
    {
        if (spawnCycle !=null)
        {
            instance.StopCoroutine(spawnCycle);
        }
    }

    //레벨 초기화
    public static void ResetSpawner()
    {
        instance.spawnCount = 1;

        levelTimer = 0f;
       
    }
    private void Awake()
    {
        instance = this;
        if (player == null)
        {
            player = GameObject.Find("Player").transform;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //StartSpawn();
    }
    
}