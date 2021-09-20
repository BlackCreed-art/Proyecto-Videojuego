using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image barra; //Imagen del UI
    public float rapidezRelleno; //Que tan rapido se cambia el relleno

    private float vidaActual = 1f; //El nivel de vida actual
    

    // Update is called once per frame
    void Update()
    {
        float rellenoActual = barra.fillAmount;
        if(rellenoActual != vidaActual){
            if(rellenoActual < vidaActual){
                rellenoActual += rapidezRelleno * Time.deltaTime ;
                barra.fillAmount = Mathf.Clamp(rellenoActual,0, vidaActual);    
            }else{
                if(rellenoActual > vidaActual){
                    rellenoActual -= rapidezRelleno * Time.deltaTime;
                    barra.fillAmount = Mathf.Clamp(rellenoActual,vidaActual, 1);
                }
            }
            
        }        
    }

    public void RecibirDanio(float danio)
    {
        vidaActual -= danio;
        Mathf.Clamp(vidaActual,0, 1);
    }

    public void Recuperar()
    {
        vidaActual = 1;
    }

    public void RecibirRecuperacion(float recuperacion){
        vidaActual += recuperacion;
        Mathf.Clamp(vidaActual,0, 1);
    }
}
