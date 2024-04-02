using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States {Iddle, Atack}
public class PokachuBehaviur : MonoBehaviour
{
    [Header("Opciones")]
    public States actualState;
    public int numberProbability = 30;
    public float velocidad = 1.0f;

    [Header("Puntos de referencia")]
    public Vector3 puntoA;
    public Vector3 puntoB;

    private float tiempoInicio;
    private float distanciaAB;
    private BoxCollider myColider;
    int numberChance;
    private void Start()
    {
        actualState = States.Iddle;
        tiempoInicio = Time.time;
        distanciaAB = Vector3.Distance(puntoA, puntoB);
        myColider = GetComponent<BoxCollider>();
        numberChance = Random.Range(1, 101);
    }
    private void Update()
    {
        if(actualState == States.Iddle)
        {
            myColider.enabled = true;
            if (numberChance >= numberProbability)
            {
                Debug.Log("Quieto");
            }
            else
            {
                Debug.Log("Se mueve");
                float tiempoPasado = (Time.time - tiempoInicio) * velocidad;
                float distanciaRecorrida = tiempoPasado / distanciaAB;
                transform.position = Vector3.Lerp(puntoA, puntoB, distanciaRecorrida);
                if (distanciaRecorrida >= 1.0f)
                {
                    tiempoInicio = Time.time;
                }
            }
        }
        else if(actualState == States.Iddle)
        {
            myColider.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("choque con la pokebola");
    }
}
