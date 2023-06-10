using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    
    public static int gridWidth =  10;
    public static int gridHeight = 25;

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    private int AddSpeedFall;
    public static bool MoveRight = false;
    public static bool MoveLeft = false;
    public static bool MoveDown = false;
    public static bool MoveRotate = false;
    public static Vector2 positionMino = new Vector2(5.0f, 25.0f);
    public Text highScoreText;
    public int scoreOneLine = 5;
    public int scoreTwoLine = 10;
    public int scoreThreeLine = 20;
    public int scoreFourLine = 50;

    private int currentLevel ;
    public int customLevel = 0;
    private int numLinesCleared = 0;

    public static float fallSpeed = 1.0f;
    public float maxSpeedFall = 9;
    
    public Slider SliderSpeed;
    public Text hud_score;
    public Text hud_level;
    public Text hud_lines;
    public Text AddSpeedSlide;
    public Text GameOver_score;

    private int numberOfRowsThisTurn = 0;
    public static int currentScore = 0;

    public AudioClip clearedLineSound;
    public AudioClip clearedLineSoundHight;

    private AudioSource audioSource;

    private GameObject previewTetromino;
    private GameObject nextTetromino;
    private bool gameStarted = false;
    private Vector2 previewTetrominoPosition = new Vector2 (-2.5f, 20);
    
    private int startingHighScore ;

    public static bool isPaused = false;
    public GameObject pausePanel;
    public GameObject blurBackground;

    

    void Start()
    {
        StartingGame();
        SpawnNextTetromino ();
        audioSource = GetComponent<AudioSource>();
        UnPauseButton();
     
        
    }
    void Update() {
        UpdateScore();
        UpdateUI();
        SaveSetting(AddSpeedFall);
        UpdateLevel(numLinesCleared);
        UpdateSpeed(AddSpeedFall);
        UpdateNewHighScore();
        CheckUserInput();

    }

    void CheckUserInput () {
        if(Input.GetKeyUp(KeyCode.P)) {
            if (Time.timeScale == 1){
                PauseButton();
            } else {
                UnPauseButton();
            }
        }
    }
    public void PauseButton () {
        Time.timeScale = 0 ; 
        isPaused = true;
        audioSource.Pause();
        blurBackground.SetActive(true);
        pausePanel.SetActive(true);
    }
    public void UnPauseButton() {
        Time.timeScale = 1;
        isPaused = false; 
        audioSource.Play();
        blurBackground.SetActive(false);
        pausePanel.SetActive(false);
    }

    void StartingGame() {
        currentScore = 0;
        hud_score.text = "0" ;
        SliderSpeed.maxValue = maxSpeedFall;
        SliderSpeed.value = PlayerPrefs.GetInt("speedsettings");
        startingHighScore = PlayerPrefs.GetInt("highscore");
        highScoreText.text = startingHighScore.ToString();
    
    }
    void UpdateHighScore (){
        if (currentScore > startingHighScore) {
            PlayerPrefs.SetInt("highscore", currentScore); 
        }
    }
    void UpdateNewHighScore() {
        if (startingHighScore <= currentScore){
            highScoreText.text = currentScore.ToString();
        }
    }
    void UpdateLevel(int value) {
        currentLevel = customLevel ;
        if (value >= (customLevel * 10))  {
            customLevel = (value / 10 ) + 1 ;  
        }
    }
    void SaveSetting(int value){
       PlayerPrefs.SetInt("speedsettings", value);
    }

    void UpdateSpeed(int value) {
      
        if (value > currentLevel ) {
           
            fallSpeed = 1.0f - ((float)value * 0.1f );
        } else {
            fallSpeed = 1.0f - ((float)currentLevel * 0.1f);
            if (maxSpeedFall < currentLevel){
                fallSpeed = 1.0f - (maxSpeedFall * 0.1f);
            }
        }

    }
    public void ChangedValue (float value){
        AddSpeedFall = (int)value ;
        
        AddSpeedSlide.text = value.ToString();
       
    }
    public void UpdateUI () {
        GameOver_score.text = currentScore.ToString();
        
        hud_score.text = currentScore.ToString();
        hud_level.text = currentLevel.ToString();
        hud_lines.text = numLinesCleared.ToString();
        SliderSpeed.minValue = currentLevel;
    }
    public void UpdateScore () {
        if (numberOfRowsThisTurn > 0 ){
            if (numberOfRowsThisTurn == 1){
                ClearedOneLine();
            }else if (numberOfRowsThisTurn ==2){
                ClearedTwoLines();
            }else if (numberOfRowsThisTurn == 3){   
                ClearedThreeLines();
            }else if (numberOfRowsThisTurn == 4){
                ClearedFourLines();
            }
            numberOfRowsThisTurn = 0;
        }
    }

    public void ClearedOneLine () {
        currentScore += scoreOneLine  + (currentLevel * 5);
        numLinesCleared ++;
        PlayLineClearedSoundHight();
    }
    public void ClearedTwoLines() {
        currentScore += scoreTwoLine + (currentLevel * 10);
        numLinesCleared += 2;
        PlayLineClearedSoundHight();

    }
    public void ClearedThreeLines() {
        currentScore += scoreThreeLine + (currentLevel * 20);
        numLinesCleared += 3;
        PlayLineClearedSound();
    }
    public void ClearedFourLines() {
        currentScore += scoreFourLine + (currentLevel * 30);
        numLinesCleared += 4;
        PlayLineClearedSound();
    }

    public void PlayLineClearedSound() {
        audioSource.PlayOneShot(clearedLineSound);
    }
    public void PlayLineClearedSoundHight() {
        audioSource.PlayOneShot(clearedLineSoundHight);
    }
    public bool CheckIsAboveGrid (Tetrimino tetrimino) {
        for (int x = 0; x < gridWidth; ++x ) {
            foreach (Transform mino in tetrimino.transform){
                Vector2 pos = Round (mino.position);
                if (pos.y > gridHeight - 1){
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsFullRowAt (int y ){
        // - The parameter y, is the row we will interate over in the grid array to check each x position for a transform.
        for (int x = 0; x < gridWidth; ++x) {
            // - If we find a position that return NULL instead of a tranform, then we know that the row is not full.
            if (grid [x, y] == null) {
                // - so we return false
                return false;
            }
        }
        //- Since we found a full row, we increment the full row variable.
        numberOfRowsThisTurn++;
        return true;
    }
    public void DeleteMinoAt (int y ) {
        for (int x = 0; x < gridWidth; ++x){
            Destroy (grid [x,y].gameObject);
            grid[x,y] = null;
        }
    }
    public void MoveRowDown (int y ) {
        for (int x = 0; x < gridWidth; ++x) {
            if (grid[x,y] != null) {
                grid[x,y -1] = grid[x,y];
                grid[x, y ] = null;
                grid [x, y-1].position += new Vector3(0, -1, 0);
            }
        }
    }
    public void MoveAllRowsDown (int y) {
        for (int i = y; i < gridHeight; ++i ){
            MoveRowDown(i);
        }
    }

    public void DeleteRow () {
        for (int y = 0; y < gridHeight; ++y) {
            if (IsFullRowAt(y)){
                DeleteMinoAt(y);
                MoveAllRowsDown (y + 1);
                --y;
            }
        }
    }
    public void UpdateGrid (Tetrimino tetrimino){
        for (int y = 0; y < gridHeight; ++y){
            for (int x= 0; x < gridWidth; ++ x){
                if (grid[x, y] != null){
                    if (grid[x,y].parent == tetrimino.transform){
                        grid[x,y] = null;
                    }
                }
            }
        }
        foreach (Transform mino in tetrimino.transform){
            Vector2 pos = Round (mino.position);
            if (pos.y < gridHeight) {
                grid [(int)pos.x, (int)pos.y] = mino;
            }
        }
    }

    public Transform GetTransformAtGridPosition (Vector2 pos) {
        if (pos.y > gridHeight -1){
            return null;
        } else {
            return grid [(int)pos.x, (int)pos.y];
        }
    }
    public void SpawnNextTetromino () {
        
        if (!gameStarted){
            gameStarted = true;
            nextTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), positionMino, Quaternion.identity );
            previewTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), previewTetrominoPosition, Quaternion.identity);
            previewTetromino.GetComponent<Tetrimino>().enabled =false;
            previewTetromino.transform.localScale =  new Vector3(0.6f, 0.6f, 1f);

        }else {
            previewTetromino.transform.localScale =  new Vector3(1f, 1f, 1f);
            previewTetromino.transform.localPosition = positionMino;
            nextTetromino = previewTetromino;
            nextTetromino.GetComponent<Tetrimino>().enabled = true;
            previewTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), previewTetrominoPosition, Quaternion.identity);
            
            previewTetromino.GetComponent<Tetrimino>().enabled =false;
            previewTetromino.transform.localScale =  new Vector3(0.6f, 0.6f, 1f);
         

            
        }
   
    }
    public bool CheckIsInsideGrid (Vector2 pos){
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
    }
    public Vector2 Round (Vector2 pos){
        return new Vector2 (Mathf.Round(pos.x), Mathf.Round(pos.y));
    }
    string GetRandomTetromino () {
        int randomTetromino = Random.Range(1, 9);
        string randomTetrominoName = "Prefabs/Tetrimono_T";
        switch (randomTetromino){
            case 1:
            randomTetrominoName = "Prefabs/Tetrimono_T";
            break;
            case 2:
            randomTetrominoName = "Prefabs/Tetrimono_I";
            break;
            case 3:
            randomTetrominoName = "Prefabs/Tetrimono_O";
            break;
            case 4:
            randomTetrominoName = "Prefabs/Tetrimono_J";
            break;
            case 5:
            randomTetrominoName = "Prefabs/Tetrimono_L";
            break;
            case 6:
            randomTetrominoName = "Prefabs/Tetrimono_S";
            break;
            case 7:
            randomTetrominoName = "Prefabs/Tetrimono_Z";
            break;
            case 8:
            randomTetrominoName = "Prefabs/Tetrimono_V";
            break;
        }
        return randomTetrominoName;
    }

    public void MoveRightButton () {
       MoveRight = true;
    }
    public void MoveRightButtonUp () {
       MoveRight = false;
       Tetrimino.movedImmediateHorizontal = false;
    }
    public void MoveLeftButton(){
        MoveLeft = true;
    }
    public void MoveLeftButtonUp(){
        MoveLeft = false;
        Tetrimino.movedImmediateHorizontal = false;
    }
    public void MoveDownButton() {
        MoveDown = true;
       
    }
    public void MoveDownButtonUp() {
        MoveDown = false;
        Tetrimino.pushDownKey = true;
        Tetrimino.movedImmediateVertical = false;
    }
    public void MoveRotateButton(){
        MoveRotate = true;
    }
    public void RestartButton () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }


    public void GameOver () {
        UpdateHighScore ();
        Debug.Log("Game Over");
    }

}
