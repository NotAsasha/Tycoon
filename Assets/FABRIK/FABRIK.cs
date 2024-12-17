using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class FABRIK : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();
    public List<Vector3> lenghts = new List<Vector3>();
    public float sum;
    public Transform originPoint;
    public Transform goalPoint;
    private void Start()
    {
        for (int i = 0; i < points.Count - 1; i++)
        {
            lenghts.Add(points[i].position - points[i + 1].position);
            sum += (points[i].position - points[i + 1].position).magnitude;
        }

        //  CheckAccessibility();
    }
    void Update()
    {


        if (sum < (goalPoint.position - originPoint.position).magnitude) Debug.LogError("Too Long");
       // for (int i = 0; i < points.Count - 1; i++)
         //   points[i + 1].position = Vector3.Lerp(points[i + 1].position, points[i].po, 1f);

        Backwards();
        Forward();
    }
    void Backwards()
    {
        for (int i = points.Count - 1; i > 0; i--)
        {
            var currentPoint = points[i];
            if (i == points.Count - 1)
                currentPoint.position = goalPoint.position; //set it to target
            else
            {
                var nextPoint = points[i + 1];
                currentPoint.position = nextPoint.position + (currentPoint.position - nextPoint.position).normalized * lenghts[i].magnitude; //set in line on distance
            }
        }
    }
    void Forward()
    {
        for (int i = 1; i < points.Count - 1; i++)
        {
            var currentPoint = points[i];
            var previousPoint = points[i - 1];
            currentPoint.position = previousPoint.position + (currentPoint.position - previousPoint.position).normalized * lenghts[i - 1].magnitude; //set in line on distance

        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
#if UNITY_EDITOR
        for (int i = 0; i < points.Count - 1; i++)
            Gizmos.DrawLine(points[i].position, points[i + 1].position);
#endif
    }
}
