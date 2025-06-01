using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class Stamina : Singelton<Stamina>
{
    [SerializeField] private Sprite emptyStaminaSprite, fullStaminaSprite;
    [SerializeField] private int maxStamina = 3;
    [SerializeField] private float staminaRecoveryTime = 5f;

    private Transform staminaContainer;
    const string STAMINA_BAR = "StaminaContainer";
    public int currentStamina;

    protected override void Awake()
    {
        base.Awake();

        currentStamina = maxStamina;
    }
    private void Start()
    {
        staminaContainer = GameObject.Find(STAMINA_BAR).transform;
    }

    public void UseStamina()
    {
        currentStamina--;
        if (currentStamina < 0)
        {
            currentStamina = 0;
        }
        UpdateStaminaBar();

        StopAllCoroutines();
        StartCoroutine(RestoreStaminaOverTime());
    }

    public void RestoreStamina(int value)
    {
        currentStamina += value;
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
        UpdateStaminaBar();
    }
    private IEnumerator RestoreStaminaOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(staminaRecoveryTime);
            RestoreStamina(1);
        }
    }

    private void UpdateStaminaBar()
    {
        for (int i = 0; i < maxStamina; i++)
        {
            Transform child = staminaContainer.GetChild(i);
            Image image = child.GetComponent<Image>();
            if (i <= currentStamina - 1)
            {
                image.sprite = fullStaminaSprite;
            }
            else
            {
                image.sprite = emptyStaminaSprite;
            }
        }
    }
    public void ResetStaminaOnDeath()
    {
        currentStamina = maxStamina;
        UpdateStaminaBar();
    }
}
