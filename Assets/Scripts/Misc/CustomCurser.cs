using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CustomCurser : MonoBehaviour
{
    [SerializeField] private Image cursorImage;

    private void Start()
    {
        if (cursorImage == null)
        {
            Debug.LogError("Cursor Image is not assigned in the inspector.");
            return;
        }
        Cursor.visible = false;
    }
    private void Update()
    {
        // Update the cursor position to follow the mouse
        Vector2 cursorPosition = Input.mousePosition;
        cursorImage.transform.position = cursorPosition;

        // Optionally, you can hide the default cursor
        Cursor.visible = false;
    }
}
