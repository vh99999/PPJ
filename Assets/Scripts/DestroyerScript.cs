//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DestroyerScript : MonoBehaviour {

//    public Canvas GameOver;

//    private void Start()
//    {
//        GameOver.gameObject.SetActive(false);
//        Time.timeScale = 1;
//    }
//
//    void OnTriggerEnter2D(Collider2D outro){
//
//        if (outro.gameObject.tag == "Player")
//        {
//            Time.timeScale = 0;
//            GameOver.gameObject.SetActive(true);
//            return;
//        }
//        else
//        {
//            if (outro.gameObject.transform.parent)
//                Destroy(outro.gameObject.transform.parent.gameObject);
//            else
//                Destroy(outro.gameObject);
//        }
//	}
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerScript : MonoBehaviour
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
        Debug.Log("🟡 Colidiu com: " + outro.gameObject.name + " | Tag: " + outro.gameObject.tag);

        if (outro.gameObject.tag == "Player")
        {
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
        else
        {
            Debug.Log("🟢 Destruindo objeto: " + outro.gameObject.name);

            if (outro.gameObject.transform.parent)
                Destroy(outro.gameObject.transform.parent.gameObject);
            else
                Destroy(outro.gameObject);
        }
    }
}

