using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailMovement : MonoBehaviour
{
    public Transform target;
    public float Speed;
    float xMove;
    float zMove;
    public bool Canmove;
    public Vector3 markPoint; 
    void Start()
    {
       
        xMove = 0;
        zMove = 0;
    }

    private void LateUpdate()
    {

        transform.Translate(new Vector3(xMove, 0, zMove) * Time.deltaTime * Speed, Space.World);
        transform.LookAt(target);
    }

    public void SetMoveDirection(bool moveable)
    {
        float x = Mathf.Round(transform.position.x * 2) / 2;
        float z = Mathf.Round(transform.position.z * 2) / 2;
        transform.position = new Vector3(x, 0.5f, z);
        markPoint = transform.position;

        if (moveable)
        {
            float dx = target.position.x - transform.position.x;
            float dz = target.position.z - transform.position.z;
            if (Mathf.Abs(dx) > Mathf.Abs(dz))
            {
                xMove = Mathf.Sign(dx);
                zMove = 0;
            }
            if (Mathf.Abs(dz) > Mathf.Abs(dx))
            {
                xMove = 0;
                zMove = Mathf.Sign(dz);
            }
        }
        else
        {
            xMove = 0;
            zMove = 0;
        }
    }


}
