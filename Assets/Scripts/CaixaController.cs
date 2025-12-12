using UnityEngine;
using System.Collections;

public class CaixaController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bola"))
        {
            // Mostra os elementos de finalização antes de mudar de cena
            GameManager.gm.FinalizarFase();
            GameManager.gm.MostrarFinalizacao();

            // Inicia a corrotina para carregar a próxima fase com atraso
            StartCoroutine(CarregarProximaFaseComDelay());
        }
    }

    IEnumerator CarregarProximaFaseComDelay()
    {
        yield return new WaitForSeconds(4f); // espera 4 segundos

        GameManager.gm.CarregarProximaFase();
    }
}