using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Courtesy of World Of Zero
/// Draws a Bezier Curve using a lineRenderer
/// https://www.youtube.com/watch?v=tgCFzoG_BJM
/// </summary>
[ExecuteInEditMode]
public class ARX_Script_BezierCurve : MonoBehaviour
{

    public Transform point1, point2, point3;
    public LineRenderer mo_lineRenderer;
    public int vertexCount = 12;

    public bool mb_move = false, mb_showNet = false;
    public GameObject mo_object;
    public int movementsOnLine = 0;


    public Transform[] bufPoints = new Transform[0];

    // Update is called once per frame
    void Update()
    {
        if (bufPoints.Length > 0)
        {
            MultiPointDraw();
        }
        else
        {
            CurveDraw();
        }


    }

    void CurveDraw()
    {
        if (movementsOnLine < 0)
            movementsOnLine = 0;
        if (movementsOnLine > vertexCount + 1)
            movementsOnLine = vertexCount + 1;

        if (mb_move && mo_object != null)
        {
            MoveAlongLine(movementsOnLine);
        }

        var pointList = new List<Vector3>();

        for (float ration = 0; ration <= 1; ration += 1.0f / vertexCount)
        {
            var tangentLineVertex1 = Vector3.Lerp(point1.position, point2.position, ration);
            var tangentLineVertex2 = Vector3.Lerp(point2.position, point3.position, ration);
            var bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ration);
            pointList.Add(bezierPoint);
        }
        mo_lineRenderer.positionCount = pointList.Count;
        mo_lineRenderer.SetPositions(pointList.ToArray());
    }

    void MultiPointDraw()
    {
        if (movementsOnLine < 0)
            movementsOnLine = 0;
        if (movementsOnLine > vertexCount + 1)
            movementsOnLine = vertexCount + 1;

        if (mb_move && mo_object != null)
        {
            MoveAlongLine(movementsOnLine);
        }

        var pointList = new List<Vector3>();

        int i = 0;
        for (float ration = 0; ration <= 1; ration += 1.0f / vertexCount, i++)
        {
            if (i < bufPoints.Length - 2)
            {
                Transform p1 = bufPoints[i], p2 = bufPoints[i + 1], p3 = bufPoints[i + 2];
                var tangentLineVertex1 = Vector3.Lerp(p1.position, p2.position, ration);
                var tangentLineVertex2 = Vector3.Lerp(p2.position, p3.position, ration);
                var bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ration);
                pointList.Add(bezierPoint);
            }
            else if(i < bufPoints.Length)
            {
                var bezierPoint = bufPoints[i].position;
                pointList.Add(bezierPoint);
            }
            

            
        }
        mo_lineRenderer.positionCount = pointList.Count;
        mo_lineRenderer.SetPositions(pointList.ToArray());
    }

    Vector3 MoveAlongLine(int nIncrements)
    {
        int i = 0;
        Vector3 bezierPoint = point1.position;
        for (float ration = 0; ration <= 1 && i < nIncrements; ration += 1.0f / vertexCount)
        {
            var tangentLineVertex1 = Vector3.Lerp(point1.position, point2.position, ration);
            var tangentLineVertex2 = Vector3.Lerp(point2.position, point3.position, ration);
            bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ration);
            i++;
        }
        

        mo_object.transform.position = bezierPoint;
        return bezierPoint;
    }


    private void OnDrawGizmos()
    {
        if (mb_showNet)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(point1.position, point2.position);

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(point2.position, point3.position);

            Gizmos.color = Color.red;
        }
        float nfDistance = 0;

        for (float ratio = 0.5F / vertexCount; ratio < 1; ratio += 1.0F / vertexCount)
        {
            Vector3 firstLerp = Vector3.Lerp(point1.position, point2.position, ratio);
            Vector3 secondLerp = Vector3.Lerp(point2.position, point3.position, ratio);

            if (mb_showNet)
                Gizmos.DrawLine(firstLerp, secondLerp);

            nfDistance += Vector3.Distance(firstLerp, secondLerp);
        }

    }
}
