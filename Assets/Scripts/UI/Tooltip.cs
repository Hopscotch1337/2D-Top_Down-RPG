using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private LayoutElement tooltipPanel;
    [SerializeField] private TMPro.TextMeshProUGUI headerText;
    [SerializeField] private TMPro.TextMeshProUGUI contentText;
    [SerializeField] private int characterLimit = 40;
    [SerializeField] Vector2 padding = new Vector2(20f, 20f);
    private RectTransform rectTransform;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string header, string content)
    {
        headerText.text = header;
        contentText.text = content;

        int headerLength = headerText.text.Length;
        int contentLength = contentText.text.Length;

        tooltipPanel.enabled = (headerLength > characterLimit || contentLength > characterLimit) ? true : false;
    }

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        float halfWidth = Screen.width / 2;
        float halfHeight = Screen.height / 2;
        float offsetY = (mousePos.y < halfHeight) ? padding.y : -padding.y;
        float offsetX = (mousePos.x < halfWidth) ? padding.x : -padding.x;

        Vector2 offset = new Vector2(offsetX, offsetY); // Offset to position the tooltip away from the mouse cursor

        float pivotX = mousePos.x / Screen.width; // Calculate pivot based on mouse position relative to screen width
        float pivotY = (offsetY > 0f) ? 0f : 1f;
        


        
        rectTransform.pivot = new Vector2(pivotX, pivotY); // Set the pivot based on the offset direction
        transform.position = mousePos + offset;
        

    }
    

}
