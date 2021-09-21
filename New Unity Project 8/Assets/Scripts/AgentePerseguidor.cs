using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentePerseguidor : MonoBehaviour
{
    public NavMeshAgent agente;
    public Transform objetivo;
    private bool active = false;
    
    public void Activar(){
        active = true;
    }

    void Update()
    {
        if(active){
            agente.SetDestination(objetivo.position);
        }
    }
    
}
