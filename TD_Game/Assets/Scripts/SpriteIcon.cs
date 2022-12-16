using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteIcon : MonoBehaviour
{
    Transform sprite;

    private void Awake()
    {
        sprite = transform.Find("Sprite");
    }
    private void OnMouseDown()
    {
        Debug.Log("CL");
        sprite.position = new Vector3(sprite.position.x, - 2f, -1);
    }
    private void OnMouseUp()
    {
        sprite.position = new Vector3(sprite.position.x, 0f, -1);
    }
}
