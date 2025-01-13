using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerMovement : MonoBehaviour
{
    public float attackerSpeed = 6;
    public Vector2 startPosition;
    public PlayerMovement playerMovement;

    private void OnEnable()
    {
        transform.position = startPosition;
    }

    void Update()
    { 
        if(playerMovement.isPlay == true){
             // 왼쪽으로 이동
            transform.Translate(Vector2.left * Time.deltaTime * attackerSpeed);

            // 왼쪽 넘어가면 안보이게 
            if (transform.position.x < -6)
            {
                gameObject.SetActive(false);
            }
        }
    }

}
