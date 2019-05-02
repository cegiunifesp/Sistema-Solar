using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Planeta : MonoBehaviour, Sensor {

    //Game settings
    GameObject Sol;
    float velAng; // velocidade de giro
    bool jogando;
    bool movendo;

    // Constante
    private Vector3 init_pos;
    private Vector3 planet_pos;

    // Custom data
    public float raio;
    public bool eh_planeta;
    public float max_velAng;

    public static Planeta planeta_atual = null;

	// Use this for initialization
	void Start () {
		
        Sol = GameObject.FindGameObjectWithTag("Sol");
        velAng = 1;
        jogando = false;
        movendo = false;
        init_pos = transform.position;
        planet_pos = Vector3.zero;

        FaseAdm.getAdm().AddSensor(this);

	}
	
	// Update is called once per frame
	void Update () {
		

        // Girar em torno no centro
        if (jogando)
        {

            if (velAng < max_velAng)
            {
                velAng = max_velAng;
                velAng += velAng * Time.deltaTime;
                if (velAng > max_velAng)
                    velAng = max_velAng;
            }

            if (eh_planeta)
                transform.position = RodaSobrePonto(transform.position, Sol.transform.position, Quaternion.Euler(0,0, velAng * Time.deltaTime * FaseAdm.getTempo()));
            return;
        }


        if (movendo)
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        else if (planet_pos == Vector3.zero && transform.position != init_pos && eh_planeta)
            transform.position = Vector3.MoveTowards(transform.position, init_pos, 100 * Time.deltaTime);
        else if (planet_pos != Vector3.zero && transform.position != planet_pos && eh_planeta)
            transform.position = Vector3.MoveTowards(transform.position, planet_pos, 100 * Time.deltaTime);

        ChecarClick();
	}

    void ChecarClick(){

        if (jogando)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            // Posicao Relatica do Mouse
            Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Calcula distancia entre Mouse e Virus 
            double dist = Mathf.Sqrt(Mathf.Pow((MousePos.x - transform.position.x), 2) +
                Mathf.Pow((MousePos.y - transform.position.y), 2));

            // Se sofreu click 
            if (dist < raio)
            {
                movendo = true;

                mudaFilhos("Luz", true);

                mudaFilhos("Texto", true);

                planeta_atual = this;
            }
        }

        if (Input.GetMouseButtonUp(0) && movendo)
        {
            movendo = false;

            mudaFilhos("Luz", false);

            planeta_atual = null;

            if (planet_pos != Vector3.zero)
            {
                mudaFilhos("Texto", false);
                FaseAdm.toca(0);
            }
            else
            {
                mudaFilhos("Texto", true);
                FaseAdm.toca(1);
            }
            
        }

    }


    void mudaFilhos(string filho, bool visivel)
    {
        foreach (Transform child in transform)
        {
            if (child.name.Contains(filho))
            {
                child.gameObject.SetActive(visivel);
            }
        } 
    }

    void OnMouseEnter()
    {
        if (!movendo && planet_pos != Vector3.zero && eh_planeta && !jogando)
        {
            mudaFilhos("Texto", true);
        }
    }

    void OnMouseExit()
    {
        if (!movendo && planet_pos != Vector3.zero && eh_planeta && !jogando)
        {
            mudaFilhos("Texto", false);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, raio);
    }

    static Vector3 RodaSobrePonto (Vector3 pono, Vector3 centro, Quaternion ang)
    {
        return ang * (pono - centro) + centro;
    }

    public bool acertou(Vector2 pos)
    {
        if (movendo)
        {
            FaseAdm.getAdm().numAcertos(1);
            planet_pos = new Vector3(pos.x, pos.y, transform.position.z);
            return true;
        }

        return false;
    }

    public bool errou(Vector2 pos)
    {
        if ((Vector2)planet_pos == pos)
        {
            planet_pos = Vector3.zero;
            FaseAdm.getAdm().numAcertos(-1);
            return true;
        }

        return false;
    }


    public void jogando_mudou(bool jog)
    {
        jogando = jog;

        mudaFilhos("Texto", true);

        if (!jog)
        {
            velAng = 1;
            planet_pos = Vector3.zero;
            movendo = false;
        }
    }

    public void set_init(Vector3 p)
    {
        init_pos = p;

    }

    public Vector3 get_init()
    {

        return init_pos;
    }
}
