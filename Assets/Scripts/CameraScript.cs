using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraScript : MonoBehaviour
{
    public Transform jogador;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(jogador.position.x + 7, 0, -10);
    }
}
