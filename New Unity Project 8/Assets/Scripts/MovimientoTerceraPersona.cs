using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoTerceraPersona : MonoBehaviour
{
    public CharacterController controlador;
    public Transform camara;
    public float rapidez = 6f;
    public float rapidezCorriendo = 12f;
    public float tiempoSuavizarGiro = 0.1f;
    float velocidadSuavizadoGiro; 
    public Transform controlSuelo;
    public float distanciaSuelo = 0.4f;
    public LayerMask mascaraSuelo;
    public float altoDeSalto = 3f;
    Vector3 velocidad;
    public float gravedad = -9.81f;
    bool tocandoSuelo;

    void Start(){
        //Ocultar y bloquear puntero del mouse
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Verificar si está tocando el suelo
        tocandoSuelo = Physics.CheckSphere(controlSuelo.position, distanciaSuelo, mascaraSuelo);
        
        if(tocandoSuelo && velocidad.y < 0){
            //Si está tocando el suelo, aplicamos una velocidad negativa en y para que el personaje se quede pegado al suelo
            velocidad.y = -2f;
        }
        //Recolectar entradas
        float horizontal = Input.GetAxisRaw("Horizontal"); //-1 si se presiona "a" o la flecha izquierda | 1 si se presiona "d" o la flecha derecha | 0 si no se preciona ninguna tecla
        float vertical = Input.GetAxisRaw("Vertical"); //-1 si se presiona "s" o la flecha abajo | 1 si se presiona "w" o la flecha arriba | 0 si no se preciona ninguna tecla
        //Dirección del movimiento, en base a las entradas
        Vector3 direccion = new Vector3(horizontal, 0f, vertical).normalized; //Normalizar para evitar que se mueva más rápido si se presionan dos teclas al mismo tiempo
        //Verificar si se está moviendo en alguna dirección
        if(direccion.magnitude >= 0.1f){
            //Atan2 angulo entre el punto 0,0 y el punto que se pasa como parámetro
            //devuelve radianes
            //Agregamos la rotación de la cámara, para que avance hacia donde mira la cámara
            float anguloObjetivo = Mathf.Atan2(direccion.x, direccion.z) * Mathf.Rad2Deg + camara.eulerAngles.y;
            //Suavizar el giro, para que no sea instantáneo
            float angulo = Mathf.SmoothDampAngle(transform.eulerAngles.y, anguloObjetivo, ref velocidadSuavizadoGiro, tiempoSuavizarGiro );
            //Rotar al personaje para que mire hacia donde avanza
            transform.rotation = Quaternion.Euler(0f, angulo, 0f);
            //Rotación a vector
            Vector3 direccionMovimiento = Quaternion.Euler(0f, anguloObjetivo, 0f) * Vector3.forward;
            //Aplicar el movimiento en x y z
            if(Input.GetKey(KeyCode.LeftShift) && tocandoSuelo){
                controlador.Move(direccionMovimiento.normalized * rapidezCorriendo * Time.deltaTime);   
            }else{
                controlador.Move(direccionMovimiento.normalized * rapidez * Time.deltaTime);                
            }            
        }

        if(Input.GetButtonDown("Jump") && tocandoSuelo){
            //Aplicar una fuerza en y para que salte
            velocidad.y = Mathf.Sqrt(altoDeSalto * -2f * gravedad);    
        }

        //Simular gravedad
        velocidad.y += gravedad * Time.deltaTime;
        //Aplicar movimiento en y
        controlador.Move(velocidad * Time.deltaTime);

    }
}
