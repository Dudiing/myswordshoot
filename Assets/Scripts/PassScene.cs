using UnityEngine;
using TMPro;
using Firebase.Database;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class PassScene : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private int sceneNumber;

    private bool cubeDestroyed = false;

    void Update()
    {
        // Verificar si se ha ingresado un nombre de 5 letras
        if (inputField.text.Length == 5)
        {
            cube.SetActive(true);
            CrearNuevoJugador(inputField.text);

            // Cambiar a la escena especificada
            SceneManager.LoadScene(sceneNumber);
        }
        else
        {
            cube.SetActive(false);
        }

        // Verificar si el cubo ha sido destruido
        //if (cube == null)
        //{
            // Crear un nuevo jugador en la base de datos
            //CrearNuevoJugador(inputField.text);

            // Cambiar a la escena especificada
            //SceneManager.LoadScene(sceneNumber);
        //}
    }

    // Método que se llama cuando el cubo es destruido
    public void CubeDestroyed()
    {
        cubeDestroyed = true;
    }

    // Método para crear un nuevo jugador en la base de datos
    private void CrearNuevoJugador(string playerName)
    {
        // Obtener la referencia a la base de datos
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        // Crear un nuevo jugador con los valores especificados
        Jugador nuevoJugador = new Jugador
        {
            Name = playerName,
            HighScore = 0,
            Score = 0
        };

        // Convertir el nuevo jugador a un diccionario
        var jugadorDict = nuevoJugador.ToDictionary();

        // Crear el nuevo jugador en la base de datos
        reference.Child("Players").Push().SetValueAsync(jugadorDict).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Nuevo jugador creado en la base de datos.");
            }
            else
            {
                Debug.LogError("Error al crear el nuevo jugador en la base de datos: " + task.Exception);
            }
        });
    }

    // Clase para representar un jugador
    public class Jugador
    {
        public string Name;
        public int HighScore;
        public int Score;

        // Convertir el jugador a un diccionario
        public Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Name"] = Name;
            result["HighScore"] = HighScore;
            result["Score"] = Score;
            return result;
        }
    }
}
