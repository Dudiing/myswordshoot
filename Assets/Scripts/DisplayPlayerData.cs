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
        // Initialize the Firebase database reference
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        // Get the current player's data
        ObtenerDatosJugador();
    }

    // Method to get the player's data from the database
    private void ObtenerDatosJugador()
    {
        // Check if player name is set
        if (string.IsNullOrEmpty(PlayerInfo.PlayerName))
        {
            Debug.LogError("Player name is not set.");
            return;
        }

        // Query the database to get the current player's data
        reference.Child("Players").OrderByChild("Name").EqualTo(PlayerInfo.PlayerName).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error getting player data: " + task.Exception);
                return;
            }

            // Get the player data from the query result
            DataSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                foreach (DataSnapshot playerSnapshot in snapshot.Children)
                {
                    // Get the player's score
                    int score = Convert.ToInt32(playerSnapshot.Child("Score").Value);

                    // Get the player's high score
                    int highScore = Convert.ToInt32(playerSnapshot.Child("HighScore").Value);

                    // Update the text on the canvas with the obtained values
                    ActualizarTexto(PlayerInfo.PlayerName, score, highScore);
                    break;
                }
            }
        });
    }

    public void ActualizarTexto(string playerName, int score, int highScore = 0)
    {
        Debug.Log("Text updated");
        // Update the text on the canvas with the obtained values
        playerDataText.text = playerName + "\nScore: " + score.ToString() + "\nYour HighScore: " + highScore.ToString();
    }
}
