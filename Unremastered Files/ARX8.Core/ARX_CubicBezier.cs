using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Draws a Cubic Bezier Curve 
/// </summary>
public class ARX_CubicBezier
{
    void MakePoint(List<Vector2> p, float t)
    {
        for (int i = 0; i < p.Count; i++)
        {
            Vector2 a, b, c, d, e, point;

            a = Vector2.Lerp(p[0], p[1], t);
            b = Vector2.Lerp(p[1], p[2], t);
            c = Vector2.Lerp(p[2], p[3], t);
            d = Vector2.Lerp(a, b, t);
            e = Vector2.Lerp(b, c, t);
            point = Vector2.Lerp(d, e, t);
        }
    }
    
}
