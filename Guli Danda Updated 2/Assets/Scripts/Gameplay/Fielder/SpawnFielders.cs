using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFielders : MonoBehaviour
{
    public int fielderToSpawnCount;
    
    public List<GameObject> fielderSpawnPool;
    public List<int> spawnedFielderIndex;
    public List<GameObject> spawnedFielder;
    public List<GameObject> spawnArea;
    
    void Start()
    {
        CheckRandomNumber();
        spawnFielders();
        
        
    }

    public void spawnFielders()
    {
        GameObject fielder;
        Vector3 fielderPosition;

        for (int i = 0; i < fielderToSpawnCount; i++)
        {
           
                fielder = (GameObject)Instantiate(fielderSpawnPool[spawnedFielderIndex[i]], transform);
                spawnedFielder.Add(fielder);
                fielder.transform.parent = this.gameObject.transform;
                float maxX = spawnArea[i].transform.position.x + spawnArea[i].GetComponent<Collider>().bounds.size.x / 2;
                float minX = spawnArea[i].transform.position.x - spawnArea[i].GetComponent<Collider>().bounds.size.x / 2;
                float maxZ = spawnArea[i].transform.position.z + spawnArea[i].GetComponent<Collider>().bounds.size.z / 2;
                float minZ = spawnArea[i].transform.position.z - spawnArea[i].GetComponent<Collider>().bounds.size.z / 2;
                fielderPosition = new Vector3(Random.Range(minX, maxX), spawnArea[i].transform.position.y, Random.Range(minZ, maxZ));
                fielder.transform.position = fielderPosition;
                fielder.AddComponent<FielderAI>();
                fielder.GetComponent<FielderAI>().fielderIndex = i;
            
        }
    }

    private void Update()
    {
        /*if (gameplayManager.checkFielderDistance == true)
        {
            int tempFielderIndex = 0;
            for(int i = 0; i < 3; i++)
            {
                if (spawnedFielder[i].GetComponent<FielderAI>().distanceFromGuli < gameplayManager.lowestFielderDistance)
                {
                    gameplayManager.lowestFielderDistance = spawnedFielder[i].GetComponent<FielderAI>().distanceFromGuli;
                    tempFielderIndex = spawnedFielder[i].GetComponent<FielderAI>().fielderIndex;
                }
            }
            gameplayManager.lowestFielderDistanceIndex = tempFielderIndex;
        }*/
    }

    private void CheckRandomNumber()
    {
        List<int> listNumbers = new List<int>();
        int number;
        for (int i = 0; i < 3; i++)
        {
            do
            {
                number = Random.Range(0, fielderSpawnPool.Count);
                if(i==0 && number != 2)
                {
                    number = 2;
                }
                
            } while (listNumbers.Contains(number));
            
            listNumbers.Add(number);
            
        }
        spawnedFielderIndex = listNumbers;
    }
}
