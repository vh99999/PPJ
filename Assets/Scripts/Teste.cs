using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teste : MonoBehaviour{

    public float x, y;
    float velocidadade;
    LineRenderer lr;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start(){
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();

        if (lr != null){
            Debug.Log("Adicionar LineRenderer");
        }
    }

    // Update is called once per frame
    void Update(){
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, new Vector3(transform.position.x + x, transform.position.y, transform.position.z + y));

        if (Input.GetButtonDown("Jump")){
            GetComponent<Rigidbody>().AddForce(new Vector3(2 * x, 0, 2 * y), ForceMode.Impulse);
            lr.enabled = false;
        }

        velocidadade = rb.velocity.magnitude;
        // Debug.Log("velocidade: " + velocidadade);

        if (velocidadade < 0.01f){
            lr.enabled = true;
        }
        else{
            lr.enabled = false;
        }  
    }

}