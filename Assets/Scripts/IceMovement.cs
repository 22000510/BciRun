using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMovement : MonoBehaviour
{

    public SpriteRenderer[] tiles;
    public Sprite[] groundImg;
    public float speed;
    public PlayerMovement playerMovement;
    private SpriteRenderer temp;
    void Start()
    {
        temp = tiles[0];
    }

    void Update()
    {  
        if(playerMovement.isPlay == true){
            for (int i = 0; i < tiles.Length; i++){
                if (-5 >= tiles[i].transform.position.x){
                    for (int q = 0; q < tiles.Length; q++){
                        if (temp.transform.position.x < tiles[q].transform.position.x)
                            temp = tiles[q];
                    }

                    tiles[i].transform.position = new Vector2(temp.transform.position.x + 2.5f, -0.6753542f);
                    tiles[i].sprite = groundImg[Random.Range(0, groundImg.Length)];
                }
            }
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].transform.Translate(new Vector2(-1,0) * Time.deltaTime * speed);
            }
        }
    }
}
