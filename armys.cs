using UnityEngine;

public class ArmySpawner : MonoBehaviour
{
    public GameObject armyPrefab;
    public GameObject pippiPrefab;
    public GameObject bibiPrefab;

    public void SpawnArmy(Vector3 pos, int pippiCount, int bibiCount)
    {
        GameObject armyObj = Instantiate(armyPrefab, pos, Quaternion.identity);
        Army army = armyObj.GetComponent<Army>();

        for (int i = 0; i < pippiCount; i++)
        {
            Vector3 spawnPos = pos + Random.insideUnitSphere * 5f;
            spawnPos.y = 0;

            GameObject unit = Instantiate(pippiPrefab, spawnPos, Quaternion.identity);
            army.units.Add(unit.GetComponent<PippiController>());
        }

        for (int i = 0; i < bibiCount; i++)
        {
            Vector3 spawnPos = pos + Random.insideUnitSphere * 5f;
            spawnPos.y = 0;

            GameObject unit = Instantiate(bibiPrefab, spawnPos, Quaternion.identity);
            army.units.Add(unit.GetComponent<PippiController>());
        }
    }
}