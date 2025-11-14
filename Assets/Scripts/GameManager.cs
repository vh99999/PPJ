using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public TMP_Text textTacadas;
    public TMP_Text textPar;
    public int tacadas;
    public int par;
    private int recorde;
    private int pontuacao;
    // Start is called before the first frame update
    void Start()
    {
        if(gm == null)
        {
            gm = this.gameObject.GetComponent<GameManager>();
            tacadas = 0;
            textTacadas.text = "Tacadas: 0";
            textPar.text = "Par: " + par;
            pontuacao = 0;

        }
    }

    public void tacada()
    {
        Debug.Log("Tacada++");
        tacadas++;
        textTacadas.text = "Tacadas: " + tacadas;
    }
    // albatross - 3, eagle, birdie, par = 0, bogie, double bogie, triple bogie + 3, infinite

    // Update is called once per frame
    void Update()
    {
        
    }
}
