using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private LayoutElement tooltipPanel;
    [SerializeField] private TMPro.TextMeshProUGUI headerText;
    [SerializeField] private TMPro.TextMeshProUGUI contentText;
    [SerializeField] private int characterLimit = 60;

    private void Update()
    {
        int headerLength = headerText.text.Length;
        int contentLength = contentText.text.Length;

        tooltipPanel.enabled = (headerLength > characterLimit || contentLength > characterLimit) ? true : false;
            
        
    }

}
