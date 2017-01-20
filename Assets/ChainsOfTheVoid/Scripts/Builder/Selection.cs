﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    /*Settings*/
    public float SnapDistance = 2;


    [HideInInspector]
    public GameObject SelectedObject;

    void Update()
    {
        UpdateInput();
    }

    void UpdateInput()
    {
        //Start
        if (Input.GetMouseButtonDown(0))
        {
            Select();
        }

        //Held
        if (Input.GetMouseButton(0))
        {
            StableizeSelectedObject();
        }

        //Reset
        if (Input.GetMouseButtonUp(0))
        {
            ResetSelection();
        }
    }

    void Select()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] mouseHits = Physics.RaycastAll(mouseRay);

        foreach (RaycastHit hit in mouseHits)
        {
            if (hit.transform.tag == "Block")
            {
                SelectedObject = hit.transform.gameObject;
                hit.transform.GetComponent<BlockInteraction>().IsInteracting = true;
                break;
            }
        }
    }

    void StableizeSelectedObject()
    {
        if (SelectedObject != null)
        {
            SelectedObject.transform.GetComponent<BlockInteraction>().Interaction();
            if(SelectedObject.transform.GetComponent<Rigidbody>() != null) SelectedObject.transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            SelectedObject.transform.GetComponent<BlockInteraction>().AssignJoints(SnapDistance);
        }
    }

    void ResetSelection()
    {
        if(SelectedObject != null)
        {
            SelectedObject.transform.GetComponent<BlockInteraction>().IsInteracting = false;
            SelectedObject.transform.GetComponent<BlockInteraction>().Snap(SnapDistance);
            SelectedObject.transform.GetComponent<BlockInteraction>().ResetColor();
            if (SelectedObject.transform.GetComponent<Rigidbody>() != null) SelectedObject.transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            SelectedObject = null;
        }
    }
}
