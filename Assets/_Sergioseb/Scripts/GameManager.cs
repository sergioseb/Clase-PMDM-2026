using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Instancia estática accesible globalmente desde cualquier script
    public static GameManager Instance { get; private set; }
 
    // Definición de los estados posibles del juego
    public enum GameState { Starting, Playing, GameOver }
    public GameState CurrentState { get; private set; }
    
    private int lives = 3; // Número de vidas iniciales del jugador
 
    // Variables de estado del jugador protegidas contra modificación externa directa
    public int Coins { get; private set; }
    public float Distance { get; private set; }
    public int Lives
    {
        get => lives;
    }
 
    private void Awake()
    {
        
        GetComponent<Renderer>().material.color = Color.red;
        GetComponent<Renderer>().material.color = new Color(
            GetComponent<Renderer>().material.color.r, GetComponent<Renderer>().material.color.g, 
            GetComponent<Renderer>().material.color.b, 0.5f);
        CurrentState = GameState.Starting;
        // Patrón Singleton: Comprobación de duplicados
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Si ya existe uno, destruimos este duplicado
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Descomentar si hay múltiples escenas
        CurrentState = GameState.Playing;
    }

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame()
    {
        Debug.Log("El juego empieza en 3 segundos...");
        yield return new WaitForSeconds(1f);
        Debug.Log("El juego empieza en 2 segundos...");
        yield return new WaitForSeconds(1f);
        Debug.Log("El juego empieza en 1 segundos...");
        yield return new WaitForSeconds(1f);
        Debug.Log("Juego Iniciado. Estado actual: Playing");
        CurrentState = GameState.Playing;
        SceneManager.LoadScene("ActionPhase");
    }

    public void AddCoin(int amount)
    {
        Coins += amount;
    }
 
    public void EndGame()
    {
        CurrentState = GameState.GameOver;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Juego Finalizado. Estado actual: Game Over");
    }
}