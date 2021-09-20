using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ConmutadorPersonajes2 : MonoBehaviour
{
    
    public GameObject[] personajes;
    public GameObject dinosaurio;
    public CinemachineFreeLook camara;
    private int indicePersonajeActivo = -1;

    private Vector3 posicionRelativaDinosaurio;

    void Start()
    {
        CambiarPersonaje(0);
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            CambiarPersonaje(0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            CambiarPersonaje(1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            CambiarPersonaje(2);
        }                
    }

    void DesactivarPersonajes(){
        foreach(GameObject personaje in personajes){
            personaje.SetActive(false);
        }
    }

    void CambiarPersonaje(int nuevoIndice){
        if(nuevoIndice != indicePersonajeActivo && nuevoIndice < personajes.Length)
        {
            //Copiar ubicación del personaje activo al personaje que se va a activar
            //Activar o desactivar dinosaurio
            //Re ubiucar dinosaurio cuando se reactiva
            if(indicePersonajeActivo >= 0){
                //Copio la posición y rotación del personaje activo al personaje que se va a activar
                personajes[nuevoIndice].transform.position = personajes[indicePersonajeActivo].transform.position;
                personajes[nuevoIndice].transform.rotation = personajes[indicePersonajeActivo].transform.rotation;

                //Activar o desactivar dinosaurio
                if(nuevoIndice == 0){
                    //Si se activa la chica, el dinosaurio la persigue
                    dinosaurio.SetActive(true);
                    //Le asigno la posición al dinosaurio
                    dinosaurio.transform.position = personajes[indicePersonajeActivo].transform.position - posicionRelativaDinosaurio;
                }else{
                    dinosaurio.SetActive(false);
                    //Guardo la posición del dinosaurio, relativa a ls posición de la chica
                    posicionRelativaDinosaurio = personajes[indicePersonajeActivo].transform.position - dinosaurio.transform.position;
                }
            }                    

            DesactivarPersonajes();
            indicePersonajeActivo = nuevoIndice;
            personajes[indicePersonajeActivo].SetActive(true);            

            camara.Follow = personajes[indicePersonajeActivo].transform;
            camara.LookAt = personajes[indicePersonajeActivo].transform;
            
        }        
    }
}
