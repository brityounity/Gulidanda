using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager gameplayManager = null;
    private DatabaseController databaseController;
    private LifeCounterController lifeCounterController;
    private GameObject hitter;
    private GameObject guli;
    private bool playerWin = false;
    private bool guliHitSoundPlayed;
    private bool matchEndSoundPlayed = false;
    private bool playerOutSoundPlayed;
    protected internal int touchCount = 0;
    protected internal bool timerOnOff = false;
    protected internal bool gamePause = false;
    protected internal float timeLeft = 10f;
    protected internal float powerMeterValue;
    protected internal bool powerMeterOn;
    protected internal bool playerCatchOut;
    //Phase 1 
    protected internal bool guliReadyForFirstHit = false;
    protected internal bool guliFirstHitDone = false;
    protected internal bool guliFirstDrop = false;
    protected internal bool hitTheStick;
    //Phase 2
    protected internal bool setHitterForSecondPhase = false;
    protected internal bool readyForSecondHit = false;
    protected internal bool secondHitDone = false;
    protected internal bool secondSwingDone = false;
    protected internal bool checkGuliDistance = false;
    protected internal bool secondPhaseEnd = false;
    protected internal Vector3 guliStartPosition;


    protected internal Transform throwGuliPosition;
    protected internal GameObject fielderGuli;
   
    public int lifeCount = 3;
    public int currentPhase = 0;
    public int guliHitDistance = 0;
    public Transform guliMaxTargetDistance;
    public Transform guliMinTargetDistance;
    public int guliTargetDistance;
    public bool gameStart;
    public bool gameOver = false;
    public bool winMatch;
    public bool proccedToHit { get; set; }

    public GameObject playerDanda;
    public GameObject groundDanda;
    public GameObject guliDangerArea;
    [Header("UI")]
    public GameObject startButton;
    public GameObject powerMeterHands;
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;
   
    public GameObject outStatus;
    public GameObject catchOutPanel;
    public Image timerImage;
    public Text timerText;
    public Text currentPhaseText;
    public Text lossAmountText;   
    public Text winAmountText;   
    public Text headerTargetDistanceText;   //For Header
    public Text overTargetDistanceText;   //ForGameOverPanel
    public Text winTargetDistanceText;   //ForWinPanel
    public Text headerGuliHitDistanceText;   //For Gameplay
    public Text overGuliHitDistanceText;   //ForGameOverPanel
    public Text winGuliHitDistanceText;   //ForWinPanel
      

    [Header("Fielder")]
    public bool checkFielderDistance = false;
    public int lowestFielderDistanceIndex;
    public float[] lowestFielderDistance;

    [Header("Sounds")]
    public AudioSource gameplaySound;
    public AudioSource playerOutSoundClip;
    public AudioSource playerWinSoundClip;
    public AudioSource playerLoseSoundClip;
    public AudioSource guliHitSound;

    [Header("Camera")]
    public GameObject hitterCamera;
    public GameObject guliCamera;
    void Awake()
    {
        gameplayManager = this;
        gameplaySound.volume = PlayerPrefs.GetFloat("music volume");
        
    }
    public static GameplayManager GetGameplayManager()
    {
        return gameplayManager;
    }

    #region Initiate
    void Start(){
        databaseController = FindObjectOfType<DatabaseController>();
        lifeCounterController = GetComponent<LifeCounterController>();
        hitter = GameObject.FindGameObjectWithTag("Player");
        guli = GameObject.FindGameObjectWithTag("guli");
        fielderGuli = GameObject.FindGameObjectWithTag("fielderGuli");
        databaseController.AddNewMatch();

        guliStartPosition = guli.transform.position;
        SetLevelTarget();
        LoadMaterial();
        ResetData();
    }
    void LoadMaterial(){
        int currentDanda = databaseController.GetUserInfo().current_danda;
        int currentGuli = databaseController.GetUserInfo().current_guli;

        playerDanda.GetComponent<MeshRenderer> ().material = Resources.Load("New/Inventory/Danda/"+currentDanda) as Material; 
        groundDanda.GetComponent<MeshRenderer> ().material = Resources.Load("New/Inventory/Danda/"+currentDanda) as Material; 
        
        guli.GetComponent<MeshRenderer> ().material = Resources.Load("New/Inventory/Guli/"+currentGuli) as Material;
        fielderGuli.GetComponent<MeshRenderer> ().material = guli.GetComponent<MeshRenderer> ().material;
        fielderGuli.SetActive(false);
    }
    void SetLevelTarget(){
        float targetLimit = Random.Range(guliMinTargetDistance.position.z,guliMaxTargetDistance.position.z) * -1;
        guliTargetDistance = Mathf.FloorToInt(targetLimit);
        headerTargetDistanceText.text = guliTargetDistance.ToString()+"M";
    }
    void ResetData(){
        guli.transform.position = guliStartPosition;
        ResetHitterAllAnimations();
        guliHitSoundPlayed = false;
        playerOutSoundPlayed = false;
        timerOnOff = false;
        timeLeft = 10f;
        timerImage.fillAmount = 1;
        touchCount = 0;
        currentPhase = 0;
        powerMeterOn = false;

        gameOver = false;

        playerCatchOut = false;
        checkFielderDistance = false;
        guliReadyForFirstHit = false;
        guliFirstHitDone = false;
        guliFirstDrop = false;
        checkGuliDistance = false;
        secondPhaseEnd = false;

        powerMeterHands.SetActive(true);
        guliDangerArea.SetActive(true);
        gameOverPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        outStatus.SetActive(false);
        catchOutPanel.SetActive(false);
        ActiveHitterCamera();
        headerGuliHitDistanceText.gameObject.SetActive(false);
    }
    private void ResetHitterAllAnimations()
    {
        foreach (var param in hitter.GetComponent<Animator>().parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                hitter.GetComponent<Animator>().ResetTrigger(param.name);
            }
        }
    }
    #endregion

    #region Gameplay
    void Update(){
        if (gameStart == true && !gameOver && gamePause == false)
        {
            GameStart();
            UpdateTimer();
            lifeCounterController.CheckLife();
        }else if(gameStart == true && gameOver == true){
            GameOver(playerWin);
            gameStart = false;
        }

    }
    void UpdateTimer(){
        if(timerOnOff == true)
        {
            timeLeft -= Time.deltaTime;
            int secondLeft = (int)timeLeft % 60;
            timerText.text = secondLeft.ToString();
            
            timerImage.fillAmount = (timeLeft / 10);
            if (secondLeft <= 0)
            {
                lifeCount -= 1;
                if(currentPhase == 0 || currentPhase == 1)
                {
                    ResetData();
                }
                if (lifeCount <= 0)
                {
                    gameOver = true;
                }
                else
                {
                    timeLeft = 11f;
                }

            }
        }
        
    }
    public void OnStartGame(){
        gameStart = true;
        startButton.SetActive(false);
    }
    void GameStart(){
        if (!gameOver)
        {
            currentPhaseText.text = currentPhase.ToString();
            if(lifeCount <= 0)
            {
                gameOver = true;
            }
            if(currentPhase == 0){
                if (Input.GetMouseButtonDown(0))
                {
                    touchCount += 1;
                }

                if(touchCount == 1){
                        
                    powerMeterOn = true;
                    timerOnOff = true;
                    touchCount = 0;
                    powerMeterHands.SetActive(false);
                    currentPhase = 1;

                }
            }
                
            if(currentPhase == 1){
                if (Input.GetMouseButtonDown(0))
                {
                    touchCount += 1;
                }
                if(touchCount == 1){ 
                    hitter.GetComponent<Animator>().SetTrigger("poseforhit");
                }
                else if(touchCount == 2){
                    hitter.GetComponent<Animator>().ResetTrigger("poseforhit");
                    guliReadyForFirstHit = true;
                }
                if(powerMeterOn == true){
                    FindObjectOfType<PowerMeterController>().PowerMeterStart();
                }
                if(guliFirstHitDone == true){
                    hitter.GetComponent<Animator>().SetTrigger("firstswing");
                    if(guliHitSoundPlayed == false)
                    {
                        GuliHitSound();
                    }
                   
                    powerMeterOn = false;
                    timerOnOff = false;
                    timeLeft = 11f;
                    guliDangerArea.SetActive(true);
                }

            }

            if(currentPhase == 2)
            {
                hitter.GetComponent<Animator>().ResetTrigger("poseforhit");
                hitter.GetComponent<Animator>().ResetTrigger("firstswing");
                guliDangerArea.SetActive(false);
                if (setHitterForSecondPhase == true)
                {
                    Debug.Log("second");
                    hitter.transform.position = guli.transform.position + new Vector3(0.6f, 0, 0);
                    hitter.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
                    groundDanda.SetActive(false);
                    playerDanda.SetActive(true);
                    powerMeterOn = true;
                   
                    hitter.GetComponent<Animator>().SetTrigger("secondhitpose");
                    timerOnOff = true;
                    guliHitSoundPlayed = false;
                    readyForSecondHit = true;
                    setHitterForSecondPhase = false;
                }

                if(secondHitDone == true)
                {
                    hitter.GetComponent<Animator>().SetTrigger("secondhit");
                }

                if(secondSwingDone == true)
                {
                    if (guliHitSoundPlayed == false)
                    {
                        GuliHitSound();
                    }
                    hitter.GetComponent<Animator>().ResetTrigger("secondhit");
                    hitter.GetComponent<Animator>().SetTrigger("secpndswing");
                    
                    //hitter.GetComponent<Animator>().ResetTrigger("secpndswing");
                }

                if(powerMeterOn == true)
                {
                    FindObjectOfType<PowerMeterController>().PowerMeterStart();
                    //timerOnOff = false;
                }
                if(checkGuliDistance == true)
                {
                    
                    guliHitDistance = (int)Vector3.Distance(guli.transform.position, guliStartPosition);
                    headerGuliHitDistanceText.gameObject.SetActive(true);
                    headerGuliHitDistanceText.text = guliHitDistance + "M";
                }

                if(secondPhaseEnd== true)
                {
                    if(guliTargetDistance <= guliHitDistance)
                    {
                        playerWin = true;
                    }
                    headerGuliHitDistanceText.gameObject.SetActive(false);
                    gameOver = true;
                }
            }
                
        }
    }

    public void ResetDataForSecondPhase()
    {
        setHitterForSecondPhase = true;
        secondHitDone = false;
        checkGuliDistance = false;
        secondPhaseEnd = false;
        hitter.GetComponent<Animator>().ResetTrigger("secondhit");
    }
    void GameOver(bool result){

        if(result == true)
        {
            if(matchEndSoundPlayed == false)
            {
                PlayPlayerWinSound();
                int betAmount = PlayerPrefs.GetInt("current coin");
                databaseController.AddCoin(betAmount * 2);
            }
            
            Invoke("ActiveWinPanel", 2f);
        }
        else
        {
            if (matchEndSoundPlayed == false)
            {
                PlayerPlayerLoseSound();
            }
            
            Invoke("ActiveLosePanel", 2f);
        }
       
    }

    public void PlayerCatchOut()
    {
        catchOutPanel.SetActive(true);
        PlayPlayerOutSound();
        Invoke("ResetData", 2f);
    }

    public int CheckNearestFielderIndex()
    {
        int inn = lowestFielderDistance.Select((n, i) => new { index = i, value = n })
         .OrderBy(item => item.value)
         .First().index;
        Debug.Log(inn);

        return lowestFielderDistance.Select((n, i) => new { index = i, value = n })
         .OrderBy(item => item.value)
         .First().index;
    }
    public void HitterResetToIdle()
    {
        hitter.GetComponent<Animator>().ResetTrigger("secondswing");
        hitter.GetComponent<Animator>().SetTrigger("idle");
        // hitter.GetComponent<Animator>().ResetTrigger("idle");
    }
    public void DangerAreaOut()
    {
        lifeCount -= 1;
        ResetData();
    }
    #endregion

    #region MatchEnd
    void ActiveLosePanel(){
        gameOverPanel.SetActive(true);
        lossAmountText.text = "-"+PlayerPrefs.GetInt("current coin");
        overTargetDistanceText.text = "Target Distance: "+guliTargetDistance+"M";
        overGuliHitDistanceText.text = "Distance: "+guliHitDistance+"M";
    }
    void ActiveWinPanel(){
        gameWinPanel.SetActive(true);
        databaseController.MatchResult(true);
        winAmountText.text = (PlayerPrefs.GetInt("current coin")*2).ToString();
        winTargetDistanceText.text = "Target Distance: "+guliTargetDistance+"M";
        winGuliHitDistanceText.text = "Distance: "+guliHitDistance+"M";
    }

    public void RetryAgain()
    {
        if (databaseController.GetUserInfo().total_coin >= PlayerPrefs.GetInt("current coin"))
        {
            databaseController.SubtractCoin(PlayerPrefs.GetInt("current coin"));
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            FindObjectOfType<SceneSwitchManager>().LoadHomeScene();
        }

    }
    #endregion

    //Control Camera
    public void ActiveHitterCamera()
    {
        hitterCamera.SetActive(true);
        guliCamera.SetActive(false);
    }
    public void ActiveGuliCamera()
    {
        hitterCamera.SetActive(false);
        guliCamera.SetActive(true);
    }


    //Control Sounds
    public void PlayPlayerOutSound()
    {
        if (PlayerPrefs.GetInt("set music") == 0)
        {
            playerOutSoundClip.volume = 1;
        }
        else
        {
            playerOutSoundClip.volume = PlayerPrefs.GetFloat("music volume");
        }
       
        playerOutSoundClip.PlayOneShot(playerOutSoundClip.clip);
        playerOutSoundPlayed = true;
    }
    public void PlayPlayerWinSound()
    {
        if (PlayerPrefs.GetInt("set music") == 0)
        {
            playerWinSoundClip.volume = 1;
        }
        else
        {
            playerWinSoundClip.volume = PlayerPrefs.GetFloat("music volume");
        }
        
        playerWinSoundClip.PlayOneShot(playerWinSoundClip.clip);
        matchEndSoundPlayed = true;
    }
    public void PlayerPlayerLoseSound()
    {
        if (PlayerPrefs.GetInt("set music") == 0)
        {
            playerLoseSoundClip.volume = 1;
        }
        else
        {
            playerLoseSoundClip.volume = PlayerPrefs.GetFloat("music volume");
        }
       
        playerLoseSoundClip.PlayOneShot(playerLoseSoundClip.clip);
        matchEndSoundPlayed = true;
    }
    public void GuliHitSound()
    {
        if (PlayerPrefs.GetInt("set music") == 0)
        {
            guliHitSound.volume = 1;
        }
        else
        {
            guliHitSound.volume = PlayerPrefs.GetFloat("music volume");
        }
        guliHitSound.PlayOneShot(guliHitSound.clip);
        guliHitSoundPlayed = true;
    }
}
