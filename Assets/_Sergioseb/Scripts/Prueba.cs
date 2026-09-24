using System;
using UnityEngine;

public class Prueba : MonoBehaviour
{
    [SerializeField] private float velocidad = 15f;
    [SerializeField] private Transform transform2;
    [SerializeField] private string nombre = "Mi Nombre";
    
    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    private void LateUpdate()
    {
        
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable");
    }

    private void OnDestroy()
    {
        Debug.Log("OnEnable");
    }
}
