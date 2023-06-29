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

    public static int AddSpeedFall;
    //public static bool MoveRight, MoveLeft, MoveDown, MoveRotate = false;
   
    public int scoreOneLine = 5;
    public int scoreTwoLine = 10;
    public int scoreThreeLine = 20;
    public int scoreFourLine = 50;

    public static int currentLevel ;
    public static int customLevel ;
    public static int numLinesCleared = 0;

    public static float fallSpeed = 1.0f;
    public float maxSpeedFall = 9;
    
    public Slider SliderSpeed;
    public Text hud_score, hud_level, hud_lines, AddSpeedSlide, GameOver_score, highScoreText;

    public static int numberOfRowsThisTurn = 0;
    public static int currentScore = 0;

    private GameObject previewTetromino;
    public static GameObject nextTetromino;
    private GameObject savedTetromno;
    private GameObject ghosttetromino;

    private bool gameStarted = false;

    public static Vector2 positionMino = new Vector2(5.0f, 25.0f);
    private Vector2 previewTetrominoPosition = new Vector2 (-2.5f, 20);
    private Vector2 savedTetrominoPosition = new Vector2(-2.5f, 8);

    public static int startingHighScore ;

    private float animScore;
    private float animHighScore;
    private float updateAnimValueHighScore;
    public float speedcount = 2.0f;

    public int maxSwaps = 2;
    private int currentSwaps = 1;

    public static bool gameover ;


    public static int tempScore;


    void Start()
    {
        StartingGame();
        SpawnNextTetromino ();
        TempHighScore();

        //UnPauseButton();
        Time.timeScale = 1;


    }
    void Update() {
        UpdateScore();
        UpdateUI();
        DetectSound();
        UpdateLevel(numLinesCleared);
        UpdateSpeed(PlayerPrefs.GetInt(PlayerData.speedsettings.ToString()));
        UpdateNewHighScore();
        CheckUserInput();
     
    }

    void CheckUserInput () {
        
        //if(Input.GetKeyUp(KeyCode.P)) {
        //    if (Time.timeScale == 1){
        //        PauseButton();
        //    } else {
        //        UnPauseButton();
        //    }
        //}
        if (Input.GetKeyUp(KeyCode.Space) || ButtonFocus.swap) {
            GameObject tempNexTetromino = GameObject.FindGameObjectWithTag("currentActiveTetromino");
            SaveTetromino(tempNexTetromino.transform);
           
        }
    }
    //public void SwapButton() {
    //    GameObject tempNexTetromino = GameObject.FindGameObjectWithTag("currentActiveTetromino");
    //    SaveTetromino(tempNexTetromino.transform);
    //}


    void StartingGame() {
        currentScore = 0;
        hud_score.text = "0" ;
        SliderSpeed.maxValue = maxSpeedFall;
        SliderSpeed.value = PlayerPrefs.GetInt("speedsettings");
        startingHighScore = PlayerPrefs.GetInt("highscore");
        highScoreText.text = startingHighScore.ToString();
    
    }
    public void UpdateHighScore (){
        if (currentScore > startingHighScore) {
            PlayerPrefs.SetInt("highscore", currentScore); 
        }
    }
    public void TempHighScore()
    {
        if (!gameover)
        {
           PlayerPrefs.SetInt("tempscore", startingHighScore);
        }
        tempScore = PlayerPrefs.GetInt("tempscore");
    }

    private int AnimCountScore()
    {
        if (animScore < currentScore)
        {
            animScore += (fallSpeed * Time.deltaTime) * speedcount;
            return (int)Mathf.Floor(animScore);
        }
        return  currentScore;
    }

    public void UpdateUI()
    {
        //GameOver_score.text = currentScore.ToString();
        hud_score.text = AnimCountScore().ToString();
        hud_level.text = currentLevel.ToString();
        hud_lines.text = numLinesCleared.ToString();
        SliderSpeed.minValue = currentLevel;
    }
    
    void UpdateNewHighScore() {
        if (startingHighScore < currentScore && AnimCountScore() == currentScore)
        {
            if (startingHighScore == 0)
            {
              
                if (animHighScore < currentScore)
                {
                    animHighScore += (fallSpeed * Time.deltaTime) * 30;
                }
                highScoreText.text = Mathf.Floor(animHighScore).ToString();
            }
            else {
                //animHighScore + startingHighScore;
                
                if (animHighScore < (currentScore - startingHighScore))
                {
                    
                    animHighScore  += (fallSpeed * Time.deltaTime) * 30;
                    updateAnimValueHighScore = startingHighScore + Mathf.Floor(animHighScore)  ;
                }
                
                highScoreText.text = updateAnimValueHighScore.ToString();
            }
            

        }
    }
    public void UpdateLevel(int value) {
        currentLevel = customLevel ;
        if (value >= (customLevel * 10))  {
            customLevel = (value / 10 ) + 1 ;  
        }
        return;
    }
    public void SaveSetting(int value){
       PlayerPrefs.SetInt("speedsettings", value);
    }

    void UpdateSpeed(int value) {
      
        if (value > currentLevel ) {
           
            fallSpeed = 1.0f - (value * 0.1f );
        } else {
            fallSpeed = 1.0f - (currentLevel * 0.1f);
            if (maxSpeedFall < currentLevel){
                fallSpeed = 1.0f - (maxSpeedFall * 0.1f);
            }
        }

    }
    public void ChangedValue (float value){
        AddSpeedFall = (int)value ;
        
        AddSpeedSlide.text = value.ToString();
       
    }

    public int rowsoundanim = 0;
    public void DetectSound() {
       
        if (rowsoundanim > 0)
        {
            
            if (rowsoundanim == 1)
            {
                FindObjectOfType<AudioGame>().PlayLineClearedSound();
            }
            else if (rowsoundanim == 2)
            {
                
                FindObjectOfType<AudioGame>().PlayLineClearedSound();
            }
            else if (rowsoundanim == 3)
            {
                FindObjectOfType<AudioGame>().PlayLineClearedSound();
            }
            else if (rowsoundanim == 4) {
                //FindObjectOfType<AudioGame>().PlayLineClearedSoundHight();
            }
            rowsoundanim = 0;
        } 
        
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
        
    }
    public void ClearedTwoLines() {
        currentScore += scoreTwoLine + (currentLevel * 10);
        numLinesCleared += 2;
        

    }
    public void ClearedThreeLines() {
        currentScore += scoreThreeLine + (currentLevel * 20);
        numLinesCleared += 3;
   
    }
    public void ClearedFourLines() {
        currentScore += scoreFourLine + (currentLevel * 30);
        numLinesCleared += 4;
       
    }


    bool CheckIsValidPosition(GameObject tetromino) {
        foreach (Transform mino in tetromino.transform) {
            Vector2 pos = Round(mino.position);
            if (!CheckIsInsideGrid(pos))
                return false;
            if (GetTransformAtGridPosition(pos) != null && GetTransformAtGridPosition(pos).parent != tetromino.transform)
                return false;
        }
        return true;
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
    public bool IsFullRowAtanim(int y)
    {
        // - The parameter y, is the row we will interate over in the grid array to check each x position for a transform.
        for (int x = 0; x < gridWidth; ++x)
        {
            // - If we find a position that return NULL instead of a tranform, then we know that the row is not full.
            if (grid[x, y] == null)
            {
                // - so we return false
                return false;
            }
        }
        rowsoundanim++;
        return true;
    }

    public void FullAnimatehide() {
        for (int y = 0; y < gridHeight; ++y)
        {
            if (IsFullRowAtanim(y))
            {
                AnimateColorhide(y);

            }
        }
    }
    public bool FullAnimateshowbool()
    {
        for (int y = 0; y < gridHeight; ++y)
        {
            if (IsFullRowAtanim(y))
            {
                return true;
            }

        }
        return false;
    }
    public void FullAnimateshow()
    {
        for (int y = 0; y < gridHeight; ++y)
        {
            if (IsFullRowAtanim(y))
            {
           
                AnimateColorshow(y);
            }
        }
    }
    public void AnimateColorshow(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            Game.grid[x, y].gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public void AnimateColorhide(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            Game.grid[x, y].gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0);
        }
    }
    public void DeleteMinoAt (int y ) {
        for (int x = 0; x < gridWidth; ++x)
        {
            Destroy(grid[x, y].gameObject);
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
            --y;
        }
    }

    public void DeleteRow () {
        for (int y = 0; y < gridHeight; ++y) {
            if (IsFullRowAt(y)){
                //AnimationRow(y);
                //StartCoroutine(Blink(2f, y));
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
            nextTetromino.tag = "currentActiveTetromino";
      
            SpawnGhostTetromino();

        }else {
            previewTetromino.transform.localScale =  new Vector3(1f, 1f, 1f);
            previewTetromino.transform.localPosition = positionMino;
            nextTetromino = previewTetromino;
            nextTetromino.GetComponent<Tetrimino>().enabled = true;
            nextTetromino.tag = "currentActiveTetromino";
            previewTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), previewTetrominoPosition, Quaternion.identity);
            
            previewTetromino.GetComponent<Tetrimino>().enabled =false;
            previewTetromino.transform.localScale =  new Vector3(0.6f, 0.6f, 1f);

      
            SpawnGhostTetromino();
        }
        currentSwaps = 0;
   
    }



    public void SpawnGhostTetromino() {
        if (GameObject.FindGameObjectWithTag ("currentGhostTetromino") != null)
            Destroy(GameObject.FindGameObjectWithTag("currentGhostTetromino"));
        ghosttetromino = (GameObject)Instantiate(nextTetromino, nextTetromino.transform.position, Quaternion.identity);
        Destroy(ghosttetromino.GetComponent<Tetrimino>());
        ghosttetromino.AddComponent<GhostTetromino>();
        
    }

    public void SaveTetromino(Transform t) {
        currentSwaps++;
        if (currentSwaps > maxSwaps)
            return;
        if (savedTetromno != null)
        {
            GameObject tempSavedTetromino = GameObject.FindGameObjectWithTag("currentSaveTetromino");

            tempSavedTetromino.transform.localPosition = new Vector2(gridWidth / 2, gridHeight);
            tempSavedTetromino.transform.localScale = new Vector3(1f, 1f, 1f);
            if (!CheckIsValidPosition(tempSavedTetromino)) {
                tempSavedTetromino.transform.localPosition = savedTetrominoPosition;
                tempSavedTetromino.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
                return;
            }
            savedTetromno = (GameObject)Instantiate(t.gameObject);
            savedTetromno.GetComponent<Tetrimino>().enabled = false;
            savedTetromno.transform.localPosition = savedTetrominoPosition;
            savedTetromno.transform.localScale = new Vector3(1f, 1f, 1f);
            savedTetromno.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
            savedTetromno.tag = "currentSaveTetromino";

            nextTetromino = (GameObject)Instantiate(tempSavedTetromino);
            nextTetromino.GetComponent<Tetrimino>().enabled = true;
            nextTetromino.transform.localPosition = new Vector2(gridWidth / 2, gridHeight);
            nextTetromino.transform.localScale = new Vector3(1f, 1f, 1f);
            nextTetromino.tag = "currentActiveTetromino";
            DestroyImmediate(t.gameObject);
            DestroyImmediate(tempSavedTetromino);

            SpawnGhostTetromino();
          
        }
        else {
            savedTetromno = (GameObject)Instantiate(GameObject.FindGameObjectWithTag("currentActiveTetromino"));
            savedTetromno.GetComponent<Tetrimino>().enabled = false;
            savedTetromno.transform.localPosition = savedTetrominoPosition;
            savedTetromno.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
            savedTetromno.tag = "currentSaveTetromino";

            DestroyImmediate(GameObject.FindGameObjectWithTag("currentActiveTetromino"));

            SpawnNextTetromino();
        }
    }
    public bool CheckIsInsideGrid (Vector2 pos){
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
    }
    public Vector2 Round (Vector2 pos){
        return new Vector2 (Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    public bool CheatMino = false;
    string GetRandomTetromino () {
        int randomTetromino = Random.Range(1, 8);
        string randomTetrominoName = "Prefabs/Tetrimono_O";
        if (!CheatMino)
        {
            switch (randomTetromino)
            {
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
                //case 8:
                //    randomTetrominoName = "Prefabs/Tetrimono_V";
                //    break;
            }
        }
        return randomTetrominoName;
    }

    //public void MoveRightButton () {
    //   MoveRight = true;
    //}
    //public void MoveRightButtonUp () {
    //   MoveRight = false;
    //    Tetrimino.movedImmediateHorizontal = false;
    //}
    //public void MoveLeftButton(){
    //    MoveLeft = true;
    //}
    //public void MoveLeftButtonUp(){
    //    MoveLeft = false;
    //    Tetrimino.movedImmediateHorizontal = false;
    //}
    //public void MoveDownButton() {
    //    MoveDown = true;
       
    //}
    //public void MoveDownButtonUp() {
    //    MoveDown = false;
      
    //    Tetrimino.movedImmediateVertical = false;
    //}
    //public void MoveRotateButton(){
    //    MoveRotate = true;
    //}
    public void RestartButton () {
        SceneManager.LoadScene("Level");
        
    }
  


    public void GameOver () {
        UpdateHighScore ();
        gameover = true;
        if (gameover) 
        FindObjectOfType<CanvasUi>().GameOver();
   
        //CanvasPanelOpen(GameOverPanel);
       
        Debug.Log("Game Over");

    }

}
