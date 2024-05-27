using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using System;

public class DisplayPlayerData : MonoBehaviour
{
    [SerializeField] private TMP_Text playerDataText;

    private DatabaseReference reference;

    void Start()
    {
        // Obtener la referencia a la base de datos
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        // Obtener los datos del jugador actual
        ObtenerDatosJugador();
    }

    // Método para obtener los datos del jugador desde la base de datos
    private void ObtenerDatosJugador()
    {
        // Hacer la consulta a la base de datos para obtener los datos del jugador actual
        reference.Child("Players").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error al obtener los datos del jugador: " + task.Exception);
                return;
            }

            // Obtener los datos del jugador del resultado de la consulta
            DataSnapshot snapshot = task.Result;
            foreach (DataSnapshot jugadorSnapshot in snapshot.Children)
            {
                // Obtener el nombre del jugador
                string playerName = jugadorSnapshot.Child("Name").Value.ToString();

                // Si el nombre del jugador coincide con el jugador actual (puedes usar otro identificador si lo deseas)
                if (playerName == "NombreDelJugadorActual")
                {
                    // Obtener el puntaje y puntaje máximo del jugador
                    int score = Convert.ToInt32(jugadorSnapshot.Child("Score").Value);
                    int highScore = Convert.ToInt32(jugadorSnapshot.Child("HighScore").Value);

                    // Actualizar el texto en el canvas con los valores obtenidos
                    ActualizarTexto(score, highScore);
                    break;
                }
            }
        });
    }

    // Método para actualizar el texto en el canvas con los valores obtenidos de la base de datos
    private void ActualizarTexto(int score, int highScore)
    {
        playerDataText.text = "Score: " + score.ToString() + "\nYour HighScore: " + highScore.ToString();
    }
}
