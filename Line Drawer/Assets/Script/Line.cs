using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

    public float mass = 10.0f;

    public float pointDistance = 0.1f;

    public float radius = 0.1f;

    public LineRenderer lineRenderer;

    private List<Vector2> points;

    private bool isEndDraw = false;

    private float modifierX, modifierX2, modifierY, modifierY2;

    public void UpdateLine (Vector2 mousePos)
	{
		if (points == null)
		{
            points = new List<Vector2>();
            SetPoint(mousePos);
			return;
		}

		if (Vector2.Distance(points.Last(), mousePos) > pointDistance)
			SetPoint(mousePos);
	}

    private void SetPoint (Vector2 point)
	{
        points.Add(point);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
    }

    public void EndDraw(Vector2 point)
    {
        if (points.Count == 1)
        {
            SetPoint(new Vector2(point.x + 0.01f, point.y));
        }

        PolygonCollider2D polygonCollider = lineRenderer.gameObject.AddComponent<PolygonCollider2D>();

        int verticesCount = 2;
        int doublePointCount = points.Count * verticesCount;
        int lastPoint = doublePointCount - 1;

        Vector2[] temps = new Vector2[doublePointCount];

        for (int i = 0; i < points.Count; i++)         
        {
            float angle = i < points.Count - 1? GetAngle(points[i], points[i+1]) : GetAngle(points[i], points[i-1]);    //最後一點  反過來取得角度
            SetModifierPoint(angle);

            if (i < points.Count - 1)
            {
                temps[i] = new Vector2(points[i].x + modifierX, points[i].y + modifierY);
                temps[lastPoint - i] = new Vector2(points[i].x + modifierX2, points[i].y + modifierY2);
            }
            else
            {
                temps[i] = new Vector2(points[i].x + modifierX2, points[i].y + modifierY2);
                temps[lastPoint - i] = new Vector2(points[i].x + modifierX, points[i].y + modifierY);
            }
        }

        polygonCollider.points = temps.ToArray();

        polygonCollider.gameObject.AddComponent<Rigidbody2D>();

        Rigidbody2D rigidbody2D = polygonCollider.gameObject.GetComponent<Rigidbody2D>();
        rigidbody2D.mass = mass;

        isEndDraw = true;
    }

    private float GetAngle(Vector2 a, Vector2 b)  
    {
        if (a.x <= b.x && a.y == b.y) return 0; //因為使用sin去求角度  避免為0的狀況
        if (a.x > b.x && a.y == b.y) return 180; //因為使用sin去求角度  避免為180的狀況

        b -= a;
        float angle = Mathf.Asin(b.y / b.magnitude) * (180 / Mathf.PI);

        angle = (b.x < 0 ? -angle : angle);
        return (angle < 0 ? angle + 360 : angle);
    }

    private void SetModifierPoint(float angle)
    {
        if (angle > 337 || angle < 23)
        {
            SetModifierPoint(-1,1,-1,-1);
        }
        else if (angle > 22 && angle < 68)
        {
            SetModifierPoint(-2, 0, 0, -2);
        }
        else if (angle > 67 && angle < 113)
        {
            SetModifierPoint(-1, -1, 1, -1);
        }
        else if (angle > 112 && angle < 158)
        {
            SetModifierPoint(0, -2, 2, 0);
        }
        else if (angle > 157 && angle < 203)
        {
            SetModifierPoint(1, -1, 1, 1);
        }
        else if (angle > 202 && angle < 248)
        {
            SetModifierPoint(2, 0, 0, 2);
        }
        else if (angle > 247 && angle < 293)
        {
            SetModifierPoint(1, 1, -1, 1);
        }
        else if (angle > 292 && angle < 338)
        {
            SetModifierPoint(0, 2, -2, 0);
        }
    }

    private void SetModifierPoint(float x1, float y1, float x2, float y2)
    {
        modifierX = x1 * radius;
        modifierY = y1 * radius;
        modifierX2 = x2 * radius;
        modifierY2 = y2 * radius;
    }
}
