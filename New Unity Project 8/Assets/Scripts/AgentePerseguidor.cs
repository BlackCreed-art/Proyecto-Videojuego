using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentePerseguidor : MonoBehaviour
{
    public NavMeshAgent agente;
    public Transform objetivo;

    void Update()
    {
        agente.SetDestination(objetivo.position);
    }
    
}
