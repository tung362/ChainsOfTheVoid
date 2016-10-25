using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MeshEdgePercentagePoint : MonoBehaviour
{
    public struct Edge
    {
        public Vector3 VectorA;
        public Vector3 VectorB;
        public int Index;
        public Edge(Vector3 aVectorA, Vector3 aVectorB, int aIndex)
        {
            VectorA = aVectorA;
            VectorB = aVectorB;
            Index = aIndex;
        }
    }

    //Get the object thats selected in the editor
    static GameObject GetSelectedObject()
    {
        return Selection.activeGameObject;
    }

    //The editor
    [MenuItem("Tung's Tools/GenerateEdgePercentagePoints")]
    static void Create()
    {
        Mesh TheMesh = GetSelectedObject().GetComponent<MeshFilter>().sharedMesh;

        List<Edge> Segments = FindSegments(TheMesh);
        List<Edge> Edges = FindEdges(Segments);
        List<Edge> SortedEdges = SortEdges(Edges);
        List<Vector3> MidPoints = FindMidpoints(SortedEdges);

        GameObject pointObject = new GameObject("Point");

        foreach (Vector3 point in MidPoints)
        {
            GameObject newPointObject = (GameObject)Instantiate(pointObject, (GetSelectedObject().transform.position + new Vector3(point.x, point.y, -0.007f)), GetSelectedObject().transform.rotation);
            newPointObject.name = "SnapPoint";
            newPointObject.AddComponent<CircleCollider2D>();
            newPointObject.GetComponent<CircleCollider2D>().radius = 0.5f;
            newPointObject.AddComponent<MeshFilter>();
            newPointObject.AddComponent<MeshRenderer>();
            newPointObject.transform.parent = GetSelectedObject().transform;
            newPointObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
        DestroyImmediate(pointObject);
    }


    //Converts the triangles into segments
    static List<Edge> FindSegments(Mesh aMesh)
    {
        List<Edge> retval = new List<Edge>();
        for (int i = 0; i < aMesh.triangles.Length / 3; ++i)
        {
            retval.Add(new Edge(aMesh.vertices[aMesh.triangles[i * 3]], aMesh.vertices[aMesh.triangles[(i * 3 + 1)]], i));
            retval.Add(new Edge(aMesh.vertices[aMesh.triangles[(i * 3 + 1)]], aMesh.vertices[aMesh.triangles[(i * 3 + 2)]], i));
            retval.Add(new Edge(aMesh.vertices[aMesh.triangles[(i * 3 + 2)]], aMesh.vertices[aMesh.triangles[i * 3]], i));
        }
        return retval;
    }


    //Find only edges from the list of segments
    static List<Edge> FindEdges(List<Edge> SegmentsList)
    {
        List<Edge> retval = SegmentsList;

        //Checks for shared vertices and removes them
        for (int i = retval.Count - 1; i > 0; i--)
        {
            for (int j = i - 1; j >= 0; j--)
            {
                if (retval[i].VectorA == retval[j].VectorB && retval[i].VectorB == retval[j].VectorA)
                {
                    //If shared vertices remove both
                    retval.RemoveAt(i);
                    retval.RemoveAt(j);
                    i--;
                    break;
                }
            }
        }
        return retval;
    }


    //Sort the edges so its not random ordered
    static List<Edge> SortEdges(List<Edge> EdgesList)
    {
        //Sort new edge
        List<Edge> retval = new List<Edge>();

        Edge nextEdge = EdgesList[0];
        for (int i = 0; i < EdgesList.Count; ++i)
        {
            if (nextEdge.VectorB == EdgesList[i].VectorA)
            {
                retval.Add(new Edge(nextEdge.VectorA, EdgesList[i].VectorA, nextEdge.Index));
                nextEdge = EdgesList[i];
                i = 0;
            }

            //Gets the last edge
            if (i == EdgesList.Count - 1) retval.Add(new Edge(nextEdge.VectorA, EdgesList[0].VectorA, nextEdge.Index));
        }

        return retval;
    }


    static List<Vector3> FindMidpoints(List<Edge> SortedEdgesList)
    {
        List<Vector3> retval = new List<Vector3>();
        for (int i = 0; i < SortedEdgesList.Count; ++i) retval.Add(Vector3.Lerp(SortedEdgesList[i].VectorA, SortedEdgesList[i].VectorB, 0.5f));
        return retval;
    }
}
