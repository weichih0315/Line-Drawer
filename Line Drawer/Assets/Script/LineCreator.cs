using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineCreator : MonoBehaviour {

    [SerializeField]
	private Line linePrefab;

    private Line currentLine;

	void Update ()
	{
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
		{

            Line line = Instantiate(linePrefab);
            line.transform.SetParent(gameObject.transform);
            currentLine = line;
        }

		if (Input.GetMouseButtonUp(0))
		{
            if (currentLine != null)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentLine.EndDraw(mousePos);
                currentLine = null;
            }
		}

		if (currentLine != null)
		{
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentLine.UpdateLine(mousePos);
		}
	}
}
