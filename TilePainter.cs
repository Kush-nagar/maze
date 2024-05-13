using System;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePainter : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile tileSliced;

    void Start()
    {
        Place();
    }
    public void Place() 
    {
        // Specify the path to your text file
        string filePath = "C:/Users/kushn/Downloads/test.txt";

        // Check if the file exists
        if (File.Exists(filePath))
        {
            try
            {
                // Read all lines from the file
                string[] lines = File.ReadAllLines(filePath);

                // Loop through each line
                for (int i = 0; i < lines.Length; i++)
                {
                    // Split the line into an array of numbers
                    string[] numbersAsString = lines[i].Split(' ');

                    // Loop through each number
                    for (int j = 0; j < numbersAsString.Length; j++)
                    {
                        // Parse the number
                        if (int.TryParse(numbersAsString[j], out int tileValue))
                        {
                            // Determine if the tile should be null or set to the specified tile
                            Vector3Int tilePosition = new Vector3Int(j, lines.Length - 1 - i, 0);
                            tilemap.SetTile(tilePosition, (tileValue == 1) ? tileSliced : null);
                        }
                        else
                        {
                            Debug.LogError($"Error parsing number at line {i + 1}, position {j + 1}");
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Debug.LogError("An error occurred while reading the file: " + e.Message);
            }
            catch (Exception e)
            {
                Debug.LogError("An unexpected error occurred: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }
}


