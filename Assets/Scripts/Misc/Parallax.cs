using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Range(-1, 0)]
    [SerializeField] private float parallaxAmount = -0.15f;

    private Camera cam;
    private Vector2 startPos;
    private Vector2 Travel => (Vector2)cam.transform.position - startPos;
    
    private void Awake()
    {
        cam = Camera.main;
    }
    private void Start() {
        startPos = cam.transform.position;
    }

    private void FixedUpdate()
    {
        transform.position = startPos + Travel * parallaxAmount;
    }
}
