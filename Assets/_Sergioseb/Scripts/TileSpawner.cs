using System.Collections.Generic;
using UnityEngine;
 
public class TileSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] tilePrefabs;     // Array con los tipos de tramos
    [SerializeField] private Transform playerTransform;    // Referencia al jugador
    [SerializeField] private int initialTiles = 5;         // Tramos iniciales en pantalla
    [SerializeField] private float tileLength = 30f;       // Longitud fija de cada tramo
    [SerializeField] private float cleanupBuffer = 5f;      // Distancia extra antes de borrar un tramo
 
    private float spawnZ = 0f;                             // Posición Z del próximo spawn
    private List<GameObject> activeTiles = new List<GameObject>(); // Lista para control de memoria
 
    private void Start()
    {
        // Generamos los tramos iniciales al arrancar el juego
        for (int i = 0; i < initialTiles; i++)
        {
            // El primer tramo siempre es seguro (índice 0), los demás aleatorios
            SpawnTile(i == 0 ? 0 : Random.Range(0, tilePrefabs.Length));
        }
    }
 
    private void Update()
    {
        // Borramos un tramo solo cuando el jugador ya ha pasado su final.
        while (activeTiles.Count > 0 &&
               playerTransform.position.z > activeTiles[0].transform.position.z + tileLength + cleanupBuffer)
        {
            SpawnTile(Random.Range(0, tilePrefabs.Length));
            DeleteOldestTile();
        }
    }
 
    private void SpawnTile(int prefabIndex)
    {
        // Instancia el prefab seleccionado en la posición Z actual
        GameObject nextTile = Instantiate(tilePrefabs[prefabIndex], Vector3.forward * spawnZ, Quaternion.identity);
        activeTiles.Add(nextTile);
        spawnZ += tileLength; // Desplazamos el punto de spawn para el siguiente tramo
    }
 
    private void DeleteOldestTile()
    {
        Destroy(activeTiles[0]); // Destruye el GameObject de la memoria de Unity
        activeTiles.RemoveAt(0); // Elimina la referencia de la lista
    }
}
