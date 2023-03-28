using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSetup : MonoBehaviour
{
    [HideInInspector] public string MapName;
    public List<GameObject> MapPrefabs;


    public void SetupAMap() 
    {
        GameObject gameInformation = GameObject.Find("GameInformation");
        GameObject MapPrefab = GetSelectedMap();
        if (MapPrefab != null)
        {
            //coordinates
            var coordinatesGameObject = MapPrefab.transform.Find("Coordinates");
            var teamSettings = gameInformation.GetComponent<PlayerTeams>();
            teamSettings.allCharacterList.teams[1].coordinates.Clear();
            for (int i = 0; i < 3; i++) 
            {
                teamSettings.allCharacterList.teams[0].coordinates[i] = coordinatesGameObject.GetChild(0).GetChild(i);
            }
            for(int i = 0; i < coordinatesGameObject.GetChild(1).childCount; i++)
            {
                teamSettings.allCharacterList.teams[1].coordinates.Add(coordinatesGameObject.GetChild(1).GetChild(i));
            }
            //AI destinations
            var destinationsGameObject = MapPrefab.transform.Find("AIDestinations");
            var AIsettings = gameInformation.GetComponent<AIManager>();
            List<GameObject> destinations = new List<GameObject>();
            for (int i = 0; i < destinationsGameObject.childCount; i++) 
            {
                destinations.Add(destinationsGameObject.GetChild(i).gameObject);
            }
            AIsettings.AIDestinations = destinations;
        }
    }

    public GameObject GetSelectedMap()
    {
        GameObject selectedMap = null;
        if (MapName != null)
        {
            selectedMap = MapPrefabs.Find(x => x.name == MapName);
        }
        return selectedMap;
    }
}
