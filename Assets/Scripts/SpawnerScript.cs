using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlataformaComChance
{
    public GameObject prefab;
    [Range(0f, 1f)]
    public float chance;
}

public class SpawnerScript : MonoBehaviour
{
    [Header("Plataformas e Chances")]
    public PlataformaComChance[] plataformasComChance;

    [Header("Área de Spawn (relativa ao spawner)")]
    public Vector2 areaSpawnCenter = Vector2.zero;
    public Vector2 areaSpawnSize = new Vector2(12f, 10f); // Largura x Altura

    [Header("Configurações de Spawn")]
    public float spawnInterval = 0.2f;
    public int plataformasPorSpawn = 3;
    public float minSpacingY = 2.5f;
    public int maxTentativasPosicao = 30;

    private float timer = 0f;

    // Para debug visual
    private List<Vector2> posicoesExistentes = new List<Vector2>();
    private List<Vector2> posicoesSpawnadasAgora = new List<Vector2>();
    private List<Vector2> posicoesTentativasInvalidas = new List<Vector2>();

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnPlataformas();
        }
    }

    void SpawnPlataformas()
    {
        posicoesExistentes.Clear();
        posicoesSpawnadasAgora.Clear();
        posicoesTentativasInvalidas.Clear();

        // Detecta todas as plataformas dentro da área
        Collider2D[] plataformasNaArea = Physics2D.OverlapBoxAll(
            (Vector2)transform.position + areaSpawnCenter,
            areaSpawnSize,
            0f);

        foreach (Collider2D col in plataformasNaArea)
        {
            if (col.CompareTag("Plataforma"))
            {
                posicoesExistentes.Add(col.transform.position);
            }
        }

        for (int i = 0; i < plataformasPorSpawn; i++)
        {
            int tentativas = 0;
            bool encontrouPosicao = false;
            Vector2 posicaoParaSpawn = Vector2.zero;

            while (tentativas < maxTentativasPosicao && !encontrouPosicao)
            {
                float halfWidth = areaSpawnSize.x / 2f;

                // Só lado direito da área
                float xBase = transform.position.x + areaSpawnCenter.x;
                float x = xBase + Random.Range(0f, halfWidth);

                float y = transform.position.y + areaSpawnCenter.y +
                          Random.Range(-areaSpawnSize.y / 2f, areaSpawnSize.y / 2f);

                Vector2 tentativaPos = new Vector2(x, y);

                bool valido = true;

                // Verifica contra plataformas existentes
                foreach (Vector2 posExist in posicoesExistentes)
                {
                    if (Mathf.Abs(posExist.y - tentativaPos.y) < minSpacingY)
                    {
                        valido = false;
                        break;
                    }
                }

                // Verifica contra as da rodada atual
                if (valido)
                {
                    foreach (Vector2 posNova in posicoesSpawnadasAgora)
                    {
                        if (Mathf.Abs(posNova.y - tentativaPos.y) < minSpacingY)
                        {
                            valido = false;
                            break;
                        }
                    }
                }

                if (valido)
                {
                    encontrouPosicao = true;
                    posicaoParaSpawn = tentativaPos;
                }
                else
                {
                    posicoesTentativasInvalidas.Add(tentativaPos);
                }

                tentativas++;
            }

            if (encontrouPosicao)
            {
                GameObject prefab = EscolherPlataformaPorChance();
                if (prefab != null)
                {
                    Instantiate(prefab, posicaoParaSpawn, Quaternion.identity);
                    posicoesSpawnadasAgora.Add(posicaoParaSpawn);
                }
                else
                {
                    Debug.LogWarning("Nenhum prefab de plataforma configurado!");
                }
            }
        }
    }

    GameObject EscolherPlataformaPorChance()
    {
        float sorte = Random.value;
        float acumulado = 0f;

        foreach (PlataformaComChance p in plataformasComChance)
        {
            acumulado += p.chance;
            if (sorte <= acumulado)
                return p.prefab;
        }

        return plataformasComChance.Length > 0 ? plataformasComChance[0].prefab : null;
    }

    private void OnDrawGizmosSelected()
    {
        // Área total
        Vector3 center = (Vector3)areaSpawnCenter + transform.position;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, areaSpawnSize);

        // Metade direita (área de spawn)
        Gizmos.color = new Color(0f, 1f, 0f, 0.2f); // Verde claro
        Vector3 spawnAreaCenter = center + new Vector3(areaSpawnSize.x / 4f, 0f, 0f);
        Vector3 spawnAreaSize = new Vector3(areaSpawnSize.x / 2f, areaSpawnSize.y, 0f);
        Gizmos.DrawCube(spawnAreaCenter, spawnAreaSize);

        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            foreach (Vector2 pos in posicoesExistentes)
                Gizmos.DrawSphere(pos, 0.25f);

            Gizmos.color = Color.green;
            foreach (Vector2 pos in posicoesSpawnadasAgora)
                Gizmos.DrawSphere(pos, 0.35f);

            Gizmos.color = Color.red;
            foreach (Vector2 pos in posicoesTentativasInvalidas)
                Gizmos.DrawSphere(pos, 0.15f);
        }
    }
}
