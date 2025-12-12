using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BolaController : MonoBehaviour
{
    public float velocidade = 5.0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Evita giros estranhos da bola
    }

    void Update()
    {
        // Somente mover a bola se ela estiver parada
        if (rb.velocity.sqrMagnitude < 0.0025f)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 direcao = new Vector3(horizontal, 0, vertical);

            if (direcao.magnitude > 0.01f)
            {
                rb.AddForce(direcao.normalized * velocidade, ForceMode.Impulse);
                if (GameManager.gm)
                    GameManager.gm.Tacada(); // Registrar a tacada
            }
        }
    }
}