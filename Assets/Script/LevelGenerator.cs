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

    //private void GenaratePlatform()
    //{
    //    while (Vector2.Distance(player.transform.position, nextPartPosition) < distanceToSpawn)
    //    {
    //        Transform part = GetRandomPlatform();

    //        Vector2 newPosition = new Vector2(nextPartPosition.x - part.Find("StartPoint").position.x, nextPartPosition.y);

    //        float randomGap = Random.Range(1f, 2f);
    //        float randomHeight = Random.Range(-8f, 8f);

    //        newPosition.x += randomGap;
    //        newPosition.y += randomHeight;

    //        Transform newPart = Instantiate(part, newPosition, transform.rotation, transform);

    //        nextPartPosition = newPart.Find("EndPoint").position;
    //    }
    //}

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
        int randomIndex = Random.Range(0, levelPart.Length); 
        return levelPart[0];
    }


}
