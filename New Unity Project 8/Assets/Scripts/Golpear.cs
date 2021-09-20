using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golpear : MonoBehaviour
{
    public Animator animator;
    public KeyCode teclaGolpear;

    public float danio = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(teclaGolpear)){
            animator.SetTrigger("golpear");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("ontriggerenter "+other);
        other.SendMessage("RecibirDanio", danio, SendMessageOptions.DontRequireReceiver);
    }
}
