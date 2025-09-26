using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleCamera : MonoBehaviour
{
    public Transform jogador;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(jogador.transform.position.x + 6, 0, -10);
    }
}
