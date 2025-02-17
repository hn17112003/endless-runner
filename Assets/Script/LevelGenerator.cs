using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform[] levelPart;
    [SerializeField] private Vector3 nextPartPosition;

    [SerializeField] private float distanceToSpawn;
    [SerializeField] private float distanceToDelete;
    [SerializeField] private Transform player;

    void Start()
    {

    }

    void Update()
    {
        DeletePlatform();
        GenaratePlatform();
    }

    private void GenaratePlatform()
    {
        while (Vector2.Distance(player.transform.position, nextPartPosition) < distanceToSpawn)
        {

            Transform part = GetRandomPlatform();

            Vector2 newPosition = new Vector2(nextPartPosition.x - part.Find("StartPoint").position.x, 0);

            Transform newPart = Instantiate(part, newPosition, transform.rotation, transform);

            nextPartPosition = newPart.Find("EndPoint").position;
        }
    }

    private void DeletePlatform()
    {
        if (transform.childCount > 0)
        {
            Transform partToDelete = transform.GetChild(0);

            if (Vector2.Distance(player.transform.position, partToDelete.transform.position) > distanceToDelete)
            {
                Destroy(partToDelete.gameObject);
            }

        }
    }

    private Transform GetRandomPlatform()
    {
        int random = Random.Range(0, 100);
        if (random < 50)
        {
            return levelPart[0];
        }
        else if (random < 80) 
        {
            return levelPart[1];
        }
        else 
        {
            return levelPart[2];
        }
    }

}
