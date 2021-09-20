using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Patrulla : MonoBehaviour
{
    public NavMeshAgent agente;
    public Transform[] puntosPatrulla;
    public TipoPatrulla tipoPatrulla;
    private int indiceDestinoActual;

    private SentidoPatrulla sentidoPatrulla;
        
    void Start()
    {
        sentidoPatrulla = SentidoPatrulla.Ida;
        indiceDestinoActual = -1;
        ObtenerProximoDestino();
    }

    void Update()
    {
        if(agente.remainingDistance <= agente.stoppingDistance){
            //Llego al punto
            ObtenerProximoDestino();
        }
    }

    void ObtenerProximoDestino(){
        //Actualizar el valor de indiceDestinoActual
        switch(tipoPatrulla){
            case TipoPatrulla.Aleatorio:{
                indiceDestinoActual = Random.Range(0, puntosPatrulla.Length);
                break;
            }
            case TipoPatrulla.Circular:{
                indiceDestinoActual += 1;
                if(indiceDestinoActual >= puntosPatrulla.Length){
                    indiceDestinoActual = 0;
                }
                break;
            }
            case TipoPatrulla.IdaVuelta:{
                switch(sentidoPatrulla){
                    case SentidoPatrulla.Ida:{                        
                        if(indiceDestinoActual < puntosPatrulla.Length - 1){
                            indiceDestinoActual +=1;                            
                        }else{
                            indiceDestinoActual -= 1;
                            sentidoPatrulla = SentidoPatrulla.Vuelta;
                        }
                        break;
                    }
                    case SentidoPatrulla.Vuelta:{
                        if(indiceDestinoActual > 0){
                            indiceDestinoActual -= 1;
                        }else{
                            sentidoPatrulla = SentidoPatrulla.Ida;
                            indiceDestinoActual += 1;
                        }
                        break;
                    }
                    default:break;
                }
                break;
            }            
            default:break;            
        }
        //Definir el siguiente destino
        DefinirProximoDestino();
    }

    void DefinirProximoDestino(){
        agente.SetDestination(puntosPatrulla[indiceDestinoActual].position);
    }
}
