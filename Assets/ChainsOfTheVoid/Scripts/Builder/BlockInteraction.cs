using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInteraction : MonoBehaviour
{
    /*Data*/
    public List<GameObject> ChildJoints = new List<GameObject>();
    public List<GameObject> ConnectedJoints = new List<GameObject>();
    public GameObject WeaponJoint;
    public GameObject ConnectedWeaponJoint;
    public bool IsCommandBlock = false;
    public bool IsInteracting = false;
    //1 = hull, 2 = weapon
    public int SnapType = 1;
    public bool IsAttached = false;
    private GameObject ClosestJoint;
    private GameObject ClosestBlockJoint;
    private GameObject PreviousClosestJoint;
    private GameObject PreviousClosestBlockJoint;

    void Start()
    {
        for(int i = 0; i < ChildJoints.Count; ++i) ConnectedJoints.Add(null);
    }

    void Update()
    {
        for (int i = 0; i < ConnectedJoints.Count; ++i)
        {
            if(ConnectedJoints[i] != null)
            {
                if (ChildJoints[i].GetComponent<Joint>().IsUsed == false) ChildJoints[i].GetComponent<Joint>().IsUsed = true;
            }
            else
            {
                if (ChildJoints[i].GetComponent<Joint>().IsUsed == true) ChildJoints[i].GetComponent<Joint>().IsUsed = false;
            }
        }

        if (ConnectedWeaponJoint != null)
        {
            if (WeaponJoint.GetComponent<Joint>().IsUsed == false) WeaponJoint.GetComponent<Joint>().IsUsed = true;
        }
        else
        {
            if (WeaponJoint.GetComponent<Joint>().IsUsed == true) WeaponJoint.GetComponent<Joint>().IsUsed = false;
        }
    }

    public void Interaction()
    {
        if (IsCommandBlock) return;
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

    public void Detatch()
    {
        if (IsCommandBlock) return;
        IsAttached = false;
        for (int i = 0; i < ConnectedJoints.Count; ++i) ConnectedJoints[i] = null;
        ConnectedWeaponJoint = null;
        foreach(Transform child in transform.root)
        {
            if(child.GetComponent<BlockInteraction>() != null) child.GetComponent<BlockInteraction>().RemoteRemoveConnectedJoint(gameObject);
        }
        transform.root.GetComponent<BlockInteraction>().RemoteRemoveConnectedJoint(gameObject);
        transform.parent = null;
        transform.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void AssignJoints(float SnapDistance)
    {
        if (IsCommandBlock) return;
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
                if(distance < ClosestblockDistance && blocks[i] != gameObject && blocks[i].GetComponent<BlockInteraction>().SnapType == 1 && blocks[i].GetComponent<BlockInteraction>().IsAttached == true)
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
                        if(distance < ClosestJointDistance && joint.tag == "BuildJoint" && BlockJoint.tag == "BuildJoint" && joint.GetComponent<Joint>().IsUsed == false && BlockJoint.GetComponent<Joint>().IsUsed == false)
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
                if (distance < ClosestblockDistance && blocks[i] != gameObject && blocks[i].GetComponent<BlockInteraction>().SnapType != 2 && blocks[i].GetComponent<BlockInteraction>().IsAttached == true)
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
                        if (distance < ClosestJointDistance && joint.tag == "WeaponJoint" && BlockJoint.tag == "WeaponJoint" && joint.GetComponent<Joint>().IsUsed == false && BlockJoint.GetComponent<Joint>().IsUsed == false)
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
        //Gets rid of physics
        transform.GetComponent<Rigidbody>().isKinematic = true;

        if (IsCommandBlock) return;
        /*Snapping blocks together depending on the type*/
        if (ClosestJoint == null || ClosestBlockJoint == null) return;
        if (Vector3.Distance(ClosestJoint.transform.position, ClosestBlockJoint.transform.position) > SnapDistance) return;

        if (SnapType == 1)
        {
            //Rotation
            Vector3 other = (new Vector3(ClosestBlockJoint.transform.parent.position.x, ClosestBlockJoint.transform.parent.position.y, 0) - new Vector3(ClosestBlockJoint.transform.position.x, ClosestBlockJoint.transform.position.y, 0)).normalized;
            Vector3 current = (new Vector3(ClosestJoint.transform.position.x, ClosestJoint.transform.position.y, 0) - new Vector3(transform.position.x, transform.position.y, 0)).normalized;
            float angle = Vector3.Angle(-other, -current);
            Vector3 cross = Vector3.Cross(-other, -current);
            if (cross.z > 0) angle = -angle;
            transform.eulerAngles += new Vector3(0, 0, angle);

            //Position
            float Offset = Vector3.Distance(ClosestJoint.transform.position, transform.position);
            Vector3 BlockJointDirection = (new Vector3(ClosestBlockJoint.transform.position.x, ClosestBlockJoint.transform.position.y, 0) - new Vector3(ClosestBlockJoint.transform.parent.position.x, ClosestBlockJoint.transform.parent.position.y, 0)).normalized;
            transform.position = new Vector3(ClosestBlockJoint.transform.position.x + (BlockJointDirection.x * Offset), ClosestBlockJoint.transform.position.y + (BlockJointDirection.y * Offset), ClosestBlockJoint.transform.position.z);

            for (int i = 0; i < ChildJoints.Count; ++i)
            {
                if (ChildJoints[i] == ClosestJoint)
                {
                    ConnectedJoints[i] = ClosestBlockJoint;
                    ClosestBlockJoint.transform.parent.GetComponent<BlockInteraction>().RemoteAddConnectedJoint(ClosestJoint, ClosestBlockJoint, 1);
                    break;
                }
            }
        }
        else if (SnapType == 2)
        {
            //Position
            transform.position = new Vector3(ClosestBlockJoint.transform.position.x, ClosestBlockJoint.transform.position.y, transform.position.z);

            //Rotation
            transform.eulerAngles = new Vector3(0, 0, 0);

            if (WeaponJoint == ClosestJoint)
            {
                ConnectedWeaponJoint = ClosestBlockJoint;
                ClosestBlockJoint.transform.parent.GetComponent<BlockInteraction>().RemoteAddConnectedJoint(ClosestJoint, ClosestBlockJoint, 2);
            }
        }

        transform.parent = ClosestBlockJoint.transform.root;
        IsAttached = true;
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

    //Call Whenever connecting with another joint (ensures sync)
    public void RemoteAddConnectedJoint(GameObject newConnectedJoint, GameObject Joint, int AddType)
    {
        if(AddType == 1)
        {
            for (int i = 0; i < ChildJoints.Count; ++i)
            {
                if (ChildJoints[i] == Joint)
                {
                    ConnectedJoints[i] = newConnectedJoint;
                    break;
                }
            }
        }
        else if(AddType == 2)
        {
            if (WeaponJoint == Joint) ConnectedWeaponJoint = newConnectedJoint;
        }
    }

    public void RemoteRemoveConnectedJoint(GameObject RemovedJoint)
    {
        for (int i = 0; i < ConnectedJoints.Count; ++i)
        {
            if(ConnectedJoints[i] != null)
            {
                if (ConnectedJoints[i].transform.parent.gameObject == RemovedJoint)
                {
                    ConnectedJoints[i] = null;
                    break;
                }
            }
        }
        if (ConnectedWeaponJoint != null)
        {
            if (ConnectedWeaponJoint.transform.parent.gameObject == RemovedJoint) ConnectedWeaponJoint = null;
        }
    }
}
