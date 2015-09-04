using UnityEngine;
using System.Collections;

public class cursorScript : MonoBehaviour 
{

	public Texture2D sprite;
	Vector2 center = Vector2.zero;

	void Start()
	{
        if (!Application.isMobilePlatform) //Use Cursor only on non Mobile Platforms
        {
			Cursor.visible = false;// Temporarily turning this off for testing purposes ~ Jonathan
        }
	}
}
