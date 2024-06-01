// Importante: Asegúrate de tener una referencia al componente DisplayPlayerData en tu escena.
// Puedes hacerlo agregando este script a un GameObject en la escena y asignándolo en el editor.

using UnityEngine;
using Firebase;
using Firebase.Database;
using System;

public class BulletCollisionDetector : MonoBehaviour
{
    private DatabaseReference reference;

    void Start()
    {
        // Initialize the Firebase database reference
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("CollisionEntered test");

        // Initialize score increment variable
        int scoreIncrement = 0;

        // Check the tag of the collided object and set the score increment
        if (collision.gameObject.CompareTag("Diana_White"))
        {
            Debug.Log("Collision with Diana_White");
            scoreIncrement = 1;
        }
        else if (collision.gameObject.CompareTag("Diana_Black"))
        {
            Debug.Log("Collision with Diana_Black");
            scoreIncrement = 3;
        }
        else if (collision.gameObject.CompareTag("Diana_Blue"))
        {
            Debug.Log("Collision with Diana_Blue");
            scoreIncrement = 5;
        }
        else if (collision.gameObject.CompareTag("Diana_Red"))
        {
            Debug.Log("Collision with Diana_Red");
            scoreIncrement = 10;
        }
        else if (collision.gameObject.CompareTag("Diana_Yellow"))
        {
            Debug.Log("Collision with Diana_Yellow");
            scoreIncrement = 20;
        }

        // If scoreIncrement is greater than 0, update the player's score in Firebase
        if (scoreIncrement > 0)
        {
            UpdatePlayerScore(scoreIncrement);
            // Destroy the parent of the collided object
            Destroy(collision.transform.parent.gameObject);
        }

        // Destroy the bullet
        Destroy(gameObject);
    }

    private void UpdatePlayerScore(int scoreIncrement)
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
                    // Get the player's key
                    string playerKey = playerSnapshot.Key;

                    // Get the current score
                    int currentScore = Convert.ToInt32(playerSnapshot.Child("Score").Value);

                    // Update the score
                    reference.Child("Players").Child(playerKey).Child("Score").SetValueAsync(currentScore + scoreIncrement).ContinueWith(updateTask =>
                    {
                        if (updateTask.IsFaulted)
                        {
                            Debug.LogError("Error updating score: " + updateTask.Exception);
                        }
                        else
                        {
                            Debug.Log("Score updated successfully");
                            // Llama al método ActualizarTexto en DisplayPlayerData
                            FindObjectOfType<DisplayPlayerData>().ActualizarTexto(PlayerInfo.PlayerName, currentScore + scoreIncrement);
                        }
                    });

                    break;
                }
            }
            else
            {
                Debug.LogError("Player not found.");
            }
        });
    }
}
