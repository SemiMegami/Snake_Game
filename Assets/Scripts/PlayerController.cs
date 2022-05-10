using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float Speed;
    SnakeHead head;
    
    MoveDirection moveDirection;
    public MoveDirection CannotMoveDirAtStart;
    float xInput;
    float zInput;
    public bool moveable;
    private void Awake()
    {
        CannotMoveDirAtStart = MoveDirection.NONE;
    }
    void Start()
    {
        moveDirection = MoveDirection.NONE;
        head = GetComponent<SnakeHead>();
        moveable = true;
    }


    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        zInput = Input.GetAxisRaw("Vertical");
        if (moveable && moveDirection == MoveDirection.NONE && (xInput!=0 || zInput!=0)){
            if (CannotMoveDirAtStart != MoveDirection.NONE)
            {
                switch (CannotMoveDirAtStart)
                {
                    case MoveDirection.LEFT:
                        if (xInput < 0) return;
                        break;
                    case MoveDirection.RIGHT:
                        if (xInput > 0) return;
                        break;
                    case MoveDirection.DOWN:
                        if (zInput < 0) return;
                        break;
                    case MoveDirection.UP:
                        if (zInput > 0) return;
                        break;
                }
            }
            StartMoving();
        }
        Move();
    }

    public void StartMoving()
    {
        CancelInvoke("SetAction");
        InvokeRepeating("SetAction", 0, 1 / Speed);
      
    }
    void SetAction()
    {
        if (moveable)
        {
            float x = Mathf.Round(transform.position.x * 2) / 2;
            float z = Mathf.Round(transform.position.z * 2) / 2;
            bool needCheck = true;
            transform.position = new Vector3(x, 0.5f, z);
            if (moveDirection != MoveDirection.LEFT && moveDirection != MoveDirection.RIGHT)
            {
                if (xInput < 0)
                {
                    moveDirection = MoveDirection.LEFT;
                    TurnFace(moveDirection, 0.7f / Speed);
                    needCheck = false;
                }
                else if (xInput > 0)
                {
                    moveDirection = MoveDirection.RIGHT;
                    TurnFace(moveDirection, 0.7f / Speed);
                    needCheck = false;
                }
            }
            if (needCheck && moveDirection != MoveDirection.DOWN && moveDirection != MoveDirection.UP)
            {
                if (zInput < 0)
                {
                    moveDirection = MoveDirection.DOWN;
                    TurnFace(moveDirection, 0.7f / Speed);
                }
                else if (zInput > 0)
                {
                    moveDirection = MoveDirection.UP;
                    TurnFace(moveDirection, 0.7f / Speed);
                }

            }
        }
        else
        {
            moveDirection = MoveDirection.NONE;
        }
       
        head.SetAction(moveable);
    }
    void Move()
    {
        switch (moveDirection)
        {
            case MoveDirection.LEFT:
                transform.Translate(- Speed * Time.deltaTime, 0,0, Space.World);
                break;
            case MoveDirection.RIGHT:
                transform.Translate(Speed * Time.deltaTime, 0, 0, Space.World);
                break;
            case MoveDirection.DOWN:
                transform.Translate(0, 0, -Speed * Time.deltaTime, Space.World);
                break;
            case MoveDirection.UP:
                transform.Translate(0,0,  Speed * Time.deltaTime, Space.World);
                break;        
        }
    }

    public void TurnFace(MoveDirection direction, float duration = 0)
    {

        Vector3 directionVec = new Vector3();


        switch (direction)
        {
            case MoveDirection.LEFT:
                directionVec = new Vector3(0, 270, 0);
                break;
            case MoveDirection.RIGHT:
                directionVec = new Vector3(0, 90, 0);
                break;
            case MoveDirection.DOWN:
                directionVec = new Vector3(0, 180, 0);
                break;
            case MoveDirection.UP:
                directionVec = new Vector3(0, 0, 0);
                break;
        }


        if (duration == 0)
        {
            transform.rotation = Quaternion.Euler(directionVec);
        }
        else
        {
            float dTheta = directionVec.y - transform.rotation.eulerAngles.y;
            dTheta = Mathf.Repeat(dTheta + 180, 360) - 180;
            StartCoroutine(Turn(Mathf.Abs(dTheta) / duration, dTheta));
        }
    }

    IEnumerator Turn(float speed, float amount)
    {
        float targetRotation = transform.rotation.eulerAngles.y + amount;
        float theta = 0;
        while (Mathf.Abs(theta) <= Mathf.Abs(amount))
        {
            float dTheta = Mathf.Sign(amount) *  speed * Time.deltaTime;
            theta += dTheta;
            transform.Rotate(new Vector3(0, dTheta, 0));
            yield return null;
        }

        //CurrectRotation
        transform.rotation = Quaternion.Euler(0, targetRotation, 0);
    }

}


public enum MoveDirection
{
    LEFT, RIGHT, DOWN, UP, NONE
}
