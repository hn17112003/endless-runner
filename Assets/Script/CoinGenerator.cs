using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    [SerializeField] private int amountOfCoins;
    [SerializeField] private GameObject coinPrefab;
    void Start()
    {
        int additonalOffset = amountOfCoins / 2;
        for (int i = 0; i < amountOfCoins; i++)
        {
            Vector3 offset = new Vector2(i - additonalOffset, 0);
            Instantiate(coinPrefab, transform.position + offset, Quaternion.identity, transform);

        }
    }
}
