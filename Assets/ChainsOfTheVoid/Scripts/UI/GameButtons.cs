using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameButtons : MonoBehaviour
{
    private LevelTracker Tracker;

    private Selection TheSelection;

    void Start()
    {
        Tracker = FindObjectOfType<LevelTracker>();
        TheSelection = FindObjectOfType<Selection>();
    }

    public void SpawnObjectAtMouse(GameObject APrefab)
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] mouseHits = Physics.RaycastAll(mouseRay);

        foreach (RaycastHit hit in mouseHits)
        {
            if (hit.collider.transform.tag == "MouseRay")
            {
                Debug.Log(hit.point);
                GameObject spawnedObj = Instantiate(APrefab, new Vector3(hit.point.x, hit.point.y, APrefab.transform.position.z), APrefab.transform.rotation) as GameObject;
                spawnedObj.GetComponent<BlockInteraction>().Detatch();
                spawnedObj.GetComponent<BlockInteraction>().IsInteracting = true;
                TheSelection.SelectedObject = spawnedObj;
                break;
            }
        }
    }
}
