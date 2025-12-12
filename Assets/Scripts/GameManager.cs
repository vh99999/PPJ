using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    [Header("Referências de UI (HUD)")]
    public TMP_Text textTacadas;
    public TMP_Text textPar;
    public TMP_Text textRecorde;   

    [Header("Configuração da Fase")]
    [Tooltip("Número ideal de tacadas para completar o buraco")]
    public int par = 3;
    public string idFase = "fase01"; 

    [Header("Configuração do Circuito (6 fases)")]
    [Tooltip("Marque TRUE na primeira fase do circuito")]
    public bool primeiraFaseDoCircuito = false;

    [Tooltip("Marque TRUE na última fase do circuito (opcional, ver fallback abaixo)")]
    public bool ultimaFaseDoCircuito = false;

    [Header("Painel de Finalização (da fase)")]
    public GameObject painelFinalizacao;
    [Tooltip("Texto grande central que mostra BIRDIE / PAR / BOGEY...")]
    public TMP_Text textClassificacaoFinal;

    [Header("Avanço de Fase")]
    [Tooltip("Nome da próxima cena para carregar (se vazio, considera que esta é a última fase do circuito)")]
    public string nomeProximaFase = "nomeDaCena";

    private int tacadas;

    private int pontuacaoTotalAtual;  
    private int recordeGeral;         
    private Vector3 ultimaPosicaoValida;

    void Awake()
    {
        if (gm == null)
            gm = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        tacadas = 0;

        if (primeiraFaseDoCircuito)
        {
            PlayerPrefs.SetInt("PontuacaoAtual", 0);
            Debug.Log("[GM] Primeira fase: zerando PontuacaoAtual");
        }

        pontuacaoTotalAtual = PlayerPrefs.GetInt("PontuacaoAtual", 0);
        recordeGeral = PlayerPrefs.GetInt("RecordeGeral", int.MaxValue);

        Debug.Log($"[GM] Start - PontuacaoAtual = {pontuacaoTotalAtual}, RecordeGeral = {(recordeGeral == int.MaxValue ? "nenhum" : recordeGeral.ToString())}");

        GameObject bola = GameObject.FindWithTag("Bola");
        if (bola != null)
            ultimaPosicaoValida = bola.transform.position;

        AtualizarUI();

        if (painelFinalizacao != null)
            painelFinalizacao.SetActive(false);
    }

    void Update()
    {
        MonitorarBola();
    }

    public void Tacada()
    {
        if (!PodeJogar()) return;

        tacadas++;
        AtualizarUI();
    }

    void AtualizarUI()
    {
        if (textTacadas != null)
            textTacadas.text = "Tacadas: " + tacadas;

        if (textPar != null)
            textPar.text = "Par: " + par;

        if (textRecorde != null)
        {
            string txtRec = (recordeGeral == int.MaxValue) ? "--" : recordeGeral.ToString();
            textRecorde.text = "Recorde: " + txtRec;
        }
    }

    public void FinalizarFase()
    {
        int pontuacaoBuraco = tacadas - par;

        pontuacaoTotalAtual = PlayerPrefs.GetInt("PontuacaoAtual", 0);
        pontuacaoTotalAtual += pontuacaoBuraco;
        PlayerPrefs.SetInt("PontuacaoAtual", pontuacaoTotalAtual);

        Debug.Log($"[GM] FinalizarFase - par={par}, tacadas={tacadas}, pontuacaoBuraco={pontuacaoBuraco}, pontuacaoTotalAtual={pontuacaoTotalAtual}");

        bool ehUltimaFase = ultimaFaseDoCircuito || string.IsNullOrEmpty(nomeProximaFase);

        if (ehUltimaFase)
        {
            Debug.Log("[GM] Esta é a ÚLTIMA fase do circuito. Verificando recorde geral...");
            VerificarRecordeGeral();
        }

        AtualizarUI();
    }

    public void MostrarFinalizacao()
    {
        if (painelFinalizacao != null)
        {
            painelFinalizacao.SetActive(true);

            if (textClassificacaoFinal != null)
                textClassificacaoFinal.text = NomePontuacao().ToUpper();
        }
    }

    void MonitorarBola()
    {
        GameObject bola = GameObject.FindWithTag("Bola");
        if (bola != null)
        {
            Rigidbody rb = bola.GetComponent<Rigidbody>();

            if (rb.velocity.magnitude < 0.05f)
                ultimaPosicaoValida = bola.transform.position;

            if (bola.transform.position.y < -2f)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                bola.transform.position = ultimaPosicaoValida;
            }
        }
    }
    void VerificarRecordeGeral()
    {
        Debug.Log($"[GM] VerificarRecordeGeral - pontuacaoTotalAtual={pontuacaoTotalAtual}, recordeGeralAntes={(recordeGeral == int.MaxValue ? "nenhum" : recordeGeral.ToString())}");

        if (pontuacaoTotalAtual < recordeGeral)
        {
            recordeGeral = pontuacaoTotalAtual;
            PlayerPrefs.SetInt("RecordeGeral", recordeGeral);
            PlayerPrefs.Save(); 

            Debug.Log($"[GM] NOVO RECORDE GERAL = {recordeGeral}");
        }
        else
        {
            Debug.Log("[GM] Não bateu o recorde geral.");
        }
    }

    string NomePontuacao()
    {
        int diff = tacadas - par;

        if (diff <= -3) return "Albatross";
        if (diff == -2) return "Eagle";
        if (diff == -1) return "Birdie";
        if (diff == 0) return "Par";
        if (diff == 1) return "Bogey";
        return "Double Bogey+";
    }

    bool PodeJogar()
    {
        GameObject bola = GameObject.FindWithTag("Bola");
        if (bola == null) return false;

        Rigidbody rb = bola.GetComponent<Rigidbody>();
        return rb != null && rb.velocity.sqrMagnitude < 0.0025f;
    }

    public void ReiniciarFase()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CarregarProximaFase()
    {
        if (!string.IsNullOrEmpty(nomeProximaFase))
            SceneManager.LoadScene(nomeProximaFase);
    }

    public void IrParaCena(string nomeDaCena)
    {
        SceneManager.LoadScene(nomeDaCena);
    }
}