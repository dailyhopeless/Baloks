using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Tetrimino : MonoBehaviour
{
    float fall = 0; //- Countdown time for fall speed
    private float fallSpeed;
    public bool allowRotation = true; //- we need use this to specify if we want to allow the tetromino to rotate
    public bool limitRotation = false; //- This is used to limit the rotation of the tetromino to a 90 / -90 rotation (To / from)

    //public static int individualScore = 30;
    public AudioClip moveSound;
    public AudioClip landSound;

    private AudioSource audioSource;
    //private float individualScoreTime;

    public static bool pushDownKey = true;

    private readonly float continousVerticalSpeed = 0.05f;
    private readonly float continuousHorizontalSpeed = 0.1f;
    private readonly float buttonDownWaitMax = 0.2f;
    private float verticalTimer = 0;
    private float horizontalTimer = 0;
    private float buttonDownWaitTimerHorizontal = 0;
    private float buttonDownWaitTimeVertical = 0;

    public static bool movedImmediateHorizontal = false;
    public static bool movedImmediateVertical = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //fallSpeed = GameObject.Find("GameScript").GetComponent<Game>().fallSpeed;

    }

 
    void Update()
    {
        if (!Game.isPaused) {
            CheckUserInput();
            UpdateFallSpeed();
        }

        //UpdateIndividualScore();

    }
    void UpdateFallSpeed () {
        fallSpeed = Game.fallSpeed;
    }
    // void UpdateIndividualScore()
    // {

    //     if (individualScoreTime < 1)
    //     {
    //         individualScoreTime += Time.deltaTime;

    //     }
    //     else
    //     {
    //         individualScoreTime = 0;
    //         individualScore = Mathf.Max(individualScore - 2, 0);
    //     }
    // }

    /// <summary>
    /// Checks the user input.
    /// </summary>
    void CheckUserInput()
    {
        //- This method checks the keys that the player can press to manipulate the position of the tetromino.
        //- The options here will be left, right, up and down.
        //- Left and right will move the tetromino one unit to the or right
        //- Down will move the tetromino 1 unit down
        //- Up will rotate the tetromino
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) )
        {
            movedImmediateHorizontal = false;
            horizontalTimer = 0;
            buttonDownWaitTimerHorizontal = 0;
            
        }
        if (Input.GetKeyUp(KeyCode.DownArrow)) {
            movedImmediateVertical = false;
            pushDownKey = true;
            verticalTimer = 0;
            buttonDownWaitTimeVertical =0;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Game.MoveRight)
        {
            MoveRight();
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Game.MoveLeft)
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Game.MoveRotate)
        {
            Rotate();
        }
        if ((Input.GetKey(KeyCode.DownArrow) && pushDownKey) || Time.time - fall >= fallSpeed || (Game.MoveDown && pushDownKey))
        {
            MoveDown();
        }
    }

    void MoveLeft()
    {
        if (movedImmediateHorizontal)
        {
            if (buttonDownWaitTimerHorizontal < buttonDownWaitMax)
            {
                buttonDownWaitTimerHorizontal += Time.deltaTime;
                return;
            }
            if (horizontalTimer < continuousHorizontalSpeed)
            {
                horizontalTimer += Time.deltaTime;
                return;
            }
        }
        if (!movedImmediateHorizontal)
            movedImmediateHorizontal = true;
        horizontalTimer = 0;
        transform.position += new Vector3(-1, 0, 0);

        if (CheckIsValidPosition())
        {
            FindObjectOfType<Game>().UpdateGrid(this);
            PlayMoveAudio();
        }
        else
        {
            transform.position += new Vector3(1, 0, 0);
            Game.MoveLeft = false;
        }
    }
    void MoveRight()
    {
        if (movedImmediateHorizontal)
        {
            if (buttonDownWaitTimerHorizontal < buttonDownWaitMax)
            {
                buttonDownWaitTimerHorizontal += Time.deltaTime;
                return;
            }
            if (horizontalTimer < continuousHorizontalSpeed)
            {
                horizontalTimer += Time.deltaTime;
                return;
            }
        }
        if (!movedImmediateHorizontal)
            movedImmediateHorizontal = true;
        horizontalTimer = 0;
        //- Fisrt we attempt to move the tetramino to the right
        transform.position += new Vector3(1, 0, 0);
        //- We then check if the tetromino us at a valid position
        if (CheckIsValidPosition())
        {

            //- if it is , we then call the UpdateGrid method which records this tetromonis new position.
            FindObjectOfType<Game>().UpdateGrid(this);
            PlayMoveAudio();
        }
        else
        {
            //- if it isn't we move the tetromino back to the left
            transform.position += new Vector3(-1, 0, 0);
            Game.MoveRight = false;
        }
    }
    void MoveDown()
    {
        if (movedImmediateVertical)
        {
            if (buttonDownWaitTimeVertical < buttonDownWaitMax)
            {
                buttonDownWaitTimeVertical += Time.deltaTime;
                return;
            }

            if (verticalTimer < continousVerticalSpeed)
            {
                verticalTimer += Time.deltaTime;

                return;
            }
        }
        if (!movedImmediateVertical)
            movedImmediateVertical = true;
        verticalTimer = 0;
        transform.position += new Vector3(0, -1, 0);


        if (CheckIsValidPosition())
        {
            FindObjectOfType<Game>().UpdateGrid(this);

            if (Input.GetKey(KeyCode.DownArrow) || Game.MoveDown)
            {
                PlayMoveAudio();
            }
        }
        else
        {
            transform.position += new Vector3(0, 1, 0);

            FindObjectOfType<Game>().DeleteRow();
            //- Check if there are any minos above the grid
            if (FindObjectOfType<Game>().CheckIsAboveGrid(this))
            {
                FindObjectOfType<Game>().GameOver();
            }
            pushDownKey = false;

            //- Spawn the next piece
            FindObjectOfType<Game>().SpawnNextTetromino();
            PlayLandAudio();

            //Game.currentScore += individualScore;

            enabled = false;
            tag = "Untagged";
        }
        fall = Time.time;
    }
    void Rotate()
    {
        if (allowRotation)
        {
            if (transform.rotation.z >= -90)
            {


                Game.MoveRotate = false;
                if (limitRotation)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }
                if (CheckIsValidPosition())
                {
                    FindObjectOfType<Game>().UpdateGrid(this);
                    PlayMoveAudio();
                }
                else
                {
                    if (limitRotation)
                    {
                        if (transform.rotation.eulerAngles.z >= 90)
                        {
                            transform.Rotate(0, 0, -90);
                        }
                        else
                        {
                            transform.Rotate(0, 0, 90);
                        }
                    }
                    else
                    {
                        transform.Rotate(0, 0, -90);
                    }
                }
            }

        }
        else
        {
            Game.MoveRotate = false;
        }
    }
    void PlayMoveAudio()
    {
        audioSource.PlayOneShot(moveSound);
    }
    void PlayLandAudio()
    {
        audioSource.PlayOneShot(landSound);
    }

    bool CheckIsValidPosition()
    {
        foreach (Transform mino in transform)
        {
            Vector2 pos = FindObjectOfType<Game>().Round(mino.position);
            if (FindObjectOfType<Game>().CheckIsInsideGrid(pos) == false)
            {
                return false;
            }
            if (FindObjectOfType<Game>().GetTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().GetTransformAtGridPosition(pos).parent != transform)
            {
                return false;
            }
        }
        return true;
    }
}
