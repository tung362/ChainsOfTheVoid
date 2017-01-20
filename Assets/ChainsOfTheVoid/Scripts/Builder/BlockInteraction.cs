using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInteraction : MonoBehaviour
{
    /*Data*/
    public bool IsInteracting = false;
    //1 = hull, 2 = weapon
    public int SnapType = 1;
    public bool IsAttached = false;
    private GameObject ClosestJoint;
    private GameObject ClosestBlockJoint;
    private GameObject PreviousClosestJoint;
    private GameObject PreviousClosestBlockJoint;

    void Start ()
    {
		
	}
	
	void Update ()
    {

    }

    public void Interaction()
    {
        if (!IsInteracting) return;

        /*Object follow mouse*/
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] mouseHits = Physics.RaycastAll(mouseRay);

        foreach (RaycastHit hit in mouseHits)
        {
            if (hit.transform.tag == "MouseRay")
            {
                transform.position = new Vector3(hit.point.x, hit.point.y, transform.position.z);
                break;
            }
        }
    }

    public void AssignJoints(float SnapDistance)
    {
        /*Finding Closest joint of both blocks*/
        if (SnapType == 1)
        {
            //Find Closest Block
            GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
            GameObject closestBlock = null;
            float ClosestblockDistance = int.MaxValue;
            for (int i = 0; i < blocks.Length; ++i)
            {
                float distance = Vector3.Distance(transform.position, blocks[i].transform.position);
                if(distance < ClosestblockDistance && blocks[i] != gameObject && blocks[i].GetComponent<BlockInteraction>().SnapType == 1)
                {
                    ClosestblockDistance = distance;
                    closestBlock = blocks[i];
                }
            }

            //Find Closest joint of both blocks
            if(closestBlock != null)
            {
                ClosestJoint = null;
                ClosestBlockJoint = null;
                float ClosestJointDistance = int.MaxValue;
                foreach(Transform BlockJoint in closestBlock.transform)
                {
                    foreach (Transform joint in transform)
                    {
                        float distance = Vector3.Distance(joint.transform.position, BlockJoint.transform.position);
                        if(distance < ClosestJointDistance && joint.tag == "BuildJoint" && BlockJoint.tag == "BuildJoint")
                        {
                            ClosestJointDistance = distance;
                            ClosestJoint = joint.gameObject;
                            ClosestBlockJoint = BlockJoint.gameObject;
                        }
                    }
                }

                //Gives color to joins to mark where its going to snap to
                if(ClosestJoint != null && ClosestBlockJoint != null)
                {
                    if(PreviousClosestJoint != null)
                    {
                        if (PreviousClosestJoint != ClosestJoint) PreviousClosestJoint.GetComponent<Renderer>().material.color = Color.green;
                    }
                    if (PreviousClosestBlockJoint != null)
                    {
                        if (PreviousClosestBlockJoint != ClosestBlockJoint) PreviousClosestBlockJoint.GetComponent<Renderer>().material.color = Color.green;
                    }

                    foreach (Transform BlockJoint in closestBlock.transform)
                    {
                        if (BlockJoint.gameObject == ClosestBlockJoint && Vector3.Distance(ClosestJoint.transform.position, ClosestBlockJoint.transform.position) <= SnapDistance) ClosestBlockJoint.GetComponent<Renderer>().material.color = Color.yellow;
                        else
                        {
                            if(BlockJoint.tag == "BuildJoint") BlockJoint.GetComponent<Renderer>().material.color = Color.green;
                        }
                    }

                    foreach (Transform joint in transform)
                    {
                        if (joint.gameObject == ClosestJoint && Vector3.Distance(ClosestJoint.transform.position, ClosestBlockJoint.transform.position) <= SnapDistance) ClosestJoint.GetComponent<Renderer>().material.color = Color.yellow;
                        else
                        {
                            if (joint.tag == "BuildJoint") joint.GetComponent<Renderer>().material.color = Color.green;
                        }
                    }

                    PreviousClosestJoint = ClosestJoint;
                    PreviousClosestBlockJoint = ClosestBlockJoint;
                }
            }
        }
        if (SnapType == 2)
        {
            //Find Closest Block
            GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
            GameObject closestBlock = null;
            float ClosestblockDistance = int.MaxValue;
            for (int i = 0; i < blocks.Length; ++i)
            {
                float distance = Vector3.Distance(transform.position, blocks[i].transform.position);
                if (distance < ClosestblockDistance && blocks[i] != gameObject && blocks[i].GetComponent<BlockInteraction>().SnapType != 2)
                {
                    ClosestblockDistance = distance;
                    closestBlock = blocks[i];
                }
            }

            //Find Closest joint of both blocks
            if (closestBlock != null)
            {
                ClosestJoint = null;
                ClosestBlockJoint = null;
                float ClosestJointDistance = int.MaxValue;
                foreach (Transform BlockJoint in closestBlock.transform)
                {
                    foreach (Transform joint in transform)
                    {
                        float distance = Vector3.Distance(joint.transform.position, BlockJoint.transform.position);
                        if (distance < ClosestJointDistance && joint.tag == "WeaponJoint" && BlockJoint.tag == "WeaponJoint")
                        {
                            ClosestJointDistance = distance;
                            ClosestJoint = joint.gameObject;
                            ClosestBlockJoint = BlockJoint.gameObject;
                        }
                    }
                }

                //Gives color to joins to mark where its going to snap to
                if (ClosestJoint != null && ClosestBlockJoint != null)
                {
                    if (PreviousClosestJoint != null)
                    {
                        if (PreviousClosestJoint != ClosestJoint) PreviousClosestJoint.GetComponent<Renderer>().material.color = Color.red;
                    }
                    if (PreviousClosestBlockJoint != null)
                    {
                        if (PreviousClosestBlockJoint != ClosestBlockJoint) PreviousClosestBlockJoint.GetComponent<Renderer>().material.color = Color.red;
                    }

                    foreach (Transform BlockJoint in closestBlock.transform)
                    {
                        if (BlockJoint.gameObject == ClosestBlockJoint && Vector3.Distance(ClosestJoint.transform.position, ClosestBlockJoint.transform.position) <= SnapDistance) ClosestBlockJoint.GetComponent<Renderer>().material.color = Color.yellow;
                        else
                        {
                            if (BlockJoint.tag == "WeaponJoint") BlockJoint.GetComponent<Renderer>().material.color = Color.red;
                        }
                    }

                    foreach (Transform joint in transform)
                    {
                        if (joint.gameObject == ClosestJoint && Vector3.Distance(ClosestJoint.transform.position, ClosestBlockJoint.transform.position) <= SnapDistance) ClosestJoint.GetComponent<Renderer>().material.color = Color.yellow;
                        else
                        {
                            if (joint.tag == "WeaponJoint") joint.GetComponent<Renderer>().material.color = Color.red;
                        }
                    }

                    PreviousClosestJoint = ClosestJoint;
                    PreviousClosestBlockJoint = ClosestBlockJoint;
                }
            }
        }
    }

    public void Snap(float SnapDistance)
    {
        /*Snapping blocks together depending on the type*/
        if (ClosestJoint == null || ClosestBlockJoint == null) return;
        if (Vector3.Distance(ClosestJoint.transform.position, ClosestBlockJoint.transform.position) > SnapDistance) return;

        Vector3 BlockJointDirection = (ClosestBlockJoint.transform.position - ClosestBlockJoint.transform.root.transform.position).normalized;
        transform.position = new Vector3(ClosestBlockJoint.transform.position.x + (BlockJointDirection.x * 0.54f), ClosestBlockJoint.transform.position.y + (BlockJointDirection.y * 0.54f), ClosestBlockJoint.transform.position.z);

        Vector3 JointDirection = (new Vector3(ClosestJoint.transform.position.x, ClosestJoint.transform.position.y, 0) - new Vector3(transform.position.x, transform.position.y, 0)).normalized;

        float angle = Vector3.Angle(-BlockJointDirection, JointDirection);
        Debug.Log(angle);
        transform.eulerAngles += new Vector3(0, 0, angle);

        //Quaternion Augoo = Quaternion.LookRotation(Vector3.forward, );
        //transform.rotation = Augoo;

        if (ClosestJoint.transform.root.GetComponent<Rigidbody>() != null) Destroy(ClosestJoint.transform.root.GetComponent<Rigidbody>());
        if (ClosestBlockJoint.transform.root.GetComponent<Rigidbody>() != null) Destroy(ClosestBlockJoint.transform.root.GetComponent<Rigidbody>());
    }

    public void ResetColor()
    {
        if(ClosestJoint != null)
        {
            if(ClosestJoint.tag == "BuildJoint") ClosestJoint.GetComponent<Renderer>().material.color = Color.green;
            else if (ClosestJoint.tag == "WeaponJoint") ClosestJoint.GetComponent<Renderer>().material.color = Color.red;
        }

        if (ClosestBlockJoint != null)
        {
            if (ClosestBlockJoint.tag == "BuildJoint") ClosestBlockJoint.GetComponent<Renderer>().material.color = Color.green;
            else if (ClosestBlockJoint.tag == "WeaponJoint") ClosestBlockJoint.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
