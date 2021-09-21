using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrullaPersecucion : MonoBehaviour
{
    public NavMeshAgent agente;
    public Transform[] puntosPatrulla;
    public TipoPatrulla tipoPatrulla;
    private int indiceDestinoActual;

    private SentidoPatrulla sentidoPatrulla;

    public MovimientoTerceraPersona[] personajes;

    private GameObject personajeElegido;

    public float distanciaVision;
    public float anguloVision;

    public float tiempoPausa;

    private Estado estado;

    public Animator animator;

    public float distanciaGolpe;
    public bool debug;
        
    void Start()
    {
        sentidoPatrulla = SentidoPatrulla.Ida;
        indiceDestinoActual = -1;
        ObtenerProximoDestino();
        estado = Estado.Patrullando;
    }

    void Update()
    {
        if(estado == Estado.Patrullando){
            if(agente.remainingDistance <= agente.stoppingDistance){
                //Llego al punto
                ObtenerProximoDestino();
            }
        }
        if(estado == Estado.Persiguiendo){
            agente.SetDestination(personajeElegido.transform.position);
            //Si está cerca, golpea
            if(agente.remainingDistance <= distanciaGolpe && animator != null){
                animator.SetTrigger("golpear");
            }
        }
        
        //Verificar si ve a algún personaje
        bool personajeVisible = VerPersonaje();

        //Si perdió de vista al personaje, queda pausado
        if(!personajeVisible && estado == Estado.Persiguiendo){
            estado = Estado.Pausado;
            //Detenemos el enemigo
            agente.ResetPath();
            StartCoroutine("EsperarYReanudarPatrulla");
        }
        if(debug){
            //Vector hacia donde mira el enemigo
            Debug.DrawRay(transform.position, transform.forward * distanciaVision, Color.blue);            
        }        
    }

    bool VerPersonaje(){
        //Perseguir al personaje activo más cercano
        bool personajeVisible = false;
        //Distancia del personaje más cercano
        float distanciaPersonaje = Mathf.Infinity;
        //Recorrer la lista de personajes
        foreach(MovimientoTerceraPersona personaje in personajes){
            //Para dibujar el rayo de otro color si el personaje está en el cono de visión
            bool estePersonajeEnConoVision = false;            
            //Para dibujar el rayo de otro color si el enemigo ve al personaje (cerca y sin objetos en el medio)
            bool estePersonajeEsVisible = false;            
            //Vector del enemigo al personaje
            Vector3 direccionPersonaje = personaje.transform.position - transform.position;
            direccionPersonaje = direccionPersonaje.normalized;
            //Controlar si el personaje está en el cono de vista del enemigo            
            float anguloPersonajeEnemigo = Vector3.Angle(direccionPersonaje, transform.forward);            
            if(anguloPersonajeEnemigo < anguloVision){
                estePersonajeEnConoVision = true;
                if(debug){
                    Debug.Log("Personaje en cono de visión "+ personaje.gameObject.name);
                }
                //Controlar si hay algun objeto entre el enemigo y el personaje
                //Controlar si el personaje está a una distancia de vista                                          
                RaycastHit hit;  
                if (Physics.Raycast(transform.position, direccionPersonaje, out hit, distanciaVision))
                {                    
                    //Si el rayo chocó, me aseguro que sea el peronsaje y que este activo
                    if(hit.transform.gameObject == personaje.gameObject && personaje.enabled){
                        estePersonajeEsVisible = true;                                                   
                        //Si es el personaje que está más cerca
                        if(hit.distance < distanciaPersonaje){
                            //Perseguirlo
                            personajeElegido = personaje.gameObject;
                            estado = Estado.Persiguiendo;
                            personajeVisible = true;
                            //Evitar que vuelva a la patrulla
                            StopCoroutine("EsperarYReanudarPatrulla");
                        }
                    }
                        
                }
            }   
            if(debug){  
                Debug.Log("Personaje visible "+ personaje.gameObject.name);      
                //Vector al personaje
                if(estePersonajeEsVisible){
                    Debug.DrawRay(transform.position, direccionPersonaje * distanciaVision, Color.red);
                }else{
                    if(estePersonajeEnConoVision){
                        Debug.DrawRay(transform.position, direccionPersonaje * distanciaVision, Color.yellow);
                    }else{
                        Debug.DrawRay(transform.position, direccionPersonaje * distanciaVision, Color.green);
                    }
                    
                }
                                 
            }             
        }
        return personajeVisible;
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
        DefinirDestino(puntosPatrulla[indiceDestinoActual].position);
    }

    void DefinirDestino(Vector3 destino){
        agente.SetDestination(destino);
    }

    IEnumerator EsperarYReanudarPatrulla(){
        //Esperar un tiempo, pero mientras sigue viendo
        yield return new WaitForSeconds(tiempoPausa);
        //Reanudar la patrulla
        estado = Estado.Patrullando;
        DefinirProximoDestino();
        personajeElegido = null;
    }
}
