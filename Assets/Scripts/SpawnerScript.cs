using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject[] vetor;
    public float spawnMin;
    public float spawnMax;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        Instantiate(vetor[Random.Range(0, vetor.Length)], 
            transform.position, Quaternion.identity);
        Invoke("Spawn", Random.Range(spawnMin, spawnMax));
    }
}
