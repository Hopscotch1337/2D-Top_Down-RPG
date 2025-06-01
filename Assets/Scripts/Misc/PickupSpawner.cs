using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goldCoin, healthGlobe, staminaGlobe;
    [SerializeField] private float dropChance = 0.5f; // 50% chance to drop an item
    [SerializeField] private int maxGoldCoinsToDrop = 3; // Maximum number of items to drop




    public void DropItems()
    {
        int itemType = Random.Range(0, 3);
        if (Random.value > dropChance)
        {
            switch (itemType)
            {
                case 0:
                    int itemsToDrop = Random.Range(1, maxGoldCoinsToDrop + 1);
                    for (int i = 0; i < itemsToDrop; i++)
                    {
                        Instantiate(goldCoin, transform.position, Quaternion.identity);
                    }
                    break;
                case 1:
                    Instantiate(healthGlobe, transform.position, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(staminaGlobe, transform.position, Quaternion.identity);
                    break;
                default:
                    break;
            }
        }
    }
}
