using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerScript1 : MonoBehaviour
{
    public Canvas GameOver;

    private void Start()
    {
        if (GameOver != null)
        {
            GameOver.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("⚠️ GameOver Canvas não atribuído no Inspector!");
        }

        Time.timeScale = 1;
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        // Só reage se for o Player
        if (!outro.CompareTag("Player")) return;

        Debug.Log("🔴 Player atingiu o trigger. Ativando tela de Game Over.");

        Time.timeScale = 0;

        if (GameOver != null)
        {
            GameOver.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("⚠️ GameOver Canvas ainda está nulo na hora de ativar.");
        }
    }
}
