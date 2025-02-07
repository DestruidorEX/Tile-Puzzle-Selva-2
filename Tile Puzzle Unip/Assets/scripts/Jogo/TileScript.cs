using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public Vector3 targetPosition;
    private Vector3 correctPosition;
    private SpriteRenderer _sprite;
    public int number;

    void Awake() 
    {
        targetPosition = transform.position;
        correctPosition = transform.position;
        _sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.05f);

        if (EstaNoLugarCorreto())
        {
            _sprite.color = Color.green;
        }
        
    }

    public bool EstaNoLugarCorreto()
    {
        return Vector3.Distance(targetPosition, correctPosition) < 0.1f;
    }
}
