using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTetromino : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        tag = "currentGhostTetromino";
        foreach (Transform mino in transform) {
            mino.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .2f);
        }
        
    }

    // Update is called once per frame
    void Update()
    {


        //StartCoroutine(FollowActiveTetromino());

        FollowActiveTetromino();
        MoveDown();
    }
    void FollowActiveTetromino() {
        Transform currentActiveTetrominoTranform = Game.nextTetromino.transform;
        transform.position = currentActiveTetrominoTranform.position;
        transform.rotation = currentActiveTetrominoTranform.rotation;
     

    }

    void MoveDown() {
        while (CheckIsValidPosition()) {
            transform.position += new Vector3(0, -1, 0);
        }
        if (!CheckIsValidPosition()) {
            transform.position += new Vector3(0, 1, 0);
        }
    }

    bool CheckIsValidPosition() {
        foreach (Transform mino in transform) {
            Vector2 pos = FindObjectOfType<Game>().Round(mino.position);
            if (FindObjectOfType<Game>().CheckIsInsideGrid(pos) == false )
                return false;
            if (FindObjectOfType<Game>().GetTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().GetTransformAtGridPosition(pos).parent.tag == "currentActiveTetromino")
                return true;
            if (FindObjectOfType<Game>().GetTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().GetTransformAtGridPosition(pos).parent != transform)
                return false;
            }
        return true;
        }
    
}
