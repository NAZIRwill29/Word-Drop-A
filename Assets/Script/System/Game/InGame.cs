using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame : MonoBehaviour
{
    //  0        1
    //normal    win
    public AudioClip[] inGameAudioClip;
    public InGameUi inGameUi;
    public GameObject laddersObj;
    public Ladders ladders;
    public GroundManager groundManager;
    public BuilderInRun builderInRun;
    public Monster monster;
    public BackgroundManagement backgroundManagement;
    public Spawn spawn;
    public Water water;
    public GameObject confettiWin;
    //  0     1       2       3       4        5           6       
    //car1  car2    car3    boat1   boat2   airplane    helicopter
    [Tooltip("car1, 2, 3, boat1, 2, sky1, 2")]
    public int playerVehicleIndex;
    public bool isLadder, isGround, isFence, isSlime;
    public int ladderPt = 6, groundPt = 3, fencePt = 3, slimePt = 4;
    public float dangerDist;
    //current stage detail
    public int currentStageNo;
    public string sceneName;
    //next stage detail
    public string nextStageName;
    public int nextStageMode;
    //book spawn
    public bool isBookSpawnOne;
    public float bookSpawnTime;
    public bool isIncreaseDifficulty;
    [SerializeField] private float playerPos, waterPos, monsterPos;
    //  0       1       2       3
    //ladder  ground  fence   slime
    public Sprite[] builderSprite;
    //every 50 = 1 sec
    [SerializeField] private float timeIncNum = 50, lastIncNumTime;
    //[SerializeField] private int numDiff;
    //spawn------
    public float lastCharTime, lastObsTime;
    [Tooltip("change")] public float dragChar, dragObs, timeCharDuration, timeObsDuration;
    public float timeBook;
    public float lastBookTime, lastCoinTime;
    [Tooltip("change")] public float timeBookDuration = 25, timeCoinDuration = 15, dragCoin = 1.75f, dragBook = 1.5f;
    public float timeCharDurationOri, timeObsDurationOri, dragCharOri, dragObsOri, increaseNum, increaseNumObs;
    public float dragCoinOri, dragBookOri, increaseNumCoin, increaseNumBook;
    //challenge stage
    public bool isChallengeStage;
    [Tooltip("only for challenge stage")] public GameObject[] spawnerChallengeObj, waterChallengeObj, monsterChallengeObj;
    [Tooltip("only for challenge stage")] public Spawn[] spawnerChallenge;
    //[Tooltip("only for challenge stage")] public Water[] waterChallenge;
    //[Tooltip("only for challenge stage")] public Monster[] monsterChallenge;
    [Tooltip("only for challenge stage")] public GameObject[] objFallGroup;
    private int spawnIndex, monsterIndex, waterIndex;
    [SerializeField] private float lastChallengeChange, timeChallengeChange = 10;

    void Start()
    {
        //reset word point event every stage enter
        GameManager.instance.gameMenuUi.ResetWordPointEvent();
        ResetAllSpawnNum();
        if (isBookSpawnOne)
            bookSpawnTime = Random.Range(20, inGameUi.totalTime);
        //Challenge MODE()
        if (isChallengeStage)
            spawn = spawnerChallenge[0];
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isStartGame)
            return;
        if (GameManager.instance.isPauseGame)
            return;
        playerPos = GameManager.instance.player.transform.position.y - GameManager.instance.player.objHeight / 2;
        if (water)
        {
            waterPos = water.transform.position.y + water.objHeight / 2;
            dangerDist = playerPos - waterPos;
        }
        if (monster)
        {
            monsterPos = monster.transform.position.y + monster.objHeight / 2;
            dangerDist = playerPos - monsterPos;
        }
        inGameUi.UpdateDangerIndicator(dangerDist);
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isStartGame)
            return;
        if (GameManager.instance.isPauseGame)
            return;
        if (spawn.isSpawnStop)
            return;
        //increase difficulty by increase speed fall of object
        if (isIncreaseDifficulty)
        {
            if (Time.time - lastIncNumTime > timeIncNum)
            {
                incDifficulty();
            }
            //Challenge MODE()
            if (!isChallengeStage)
                return;
            // make win and stop challenge
            if (timeChallengeChange >= 1400)
            {
                if (!inGameUi.isRun)
                    //TODO () - change to win challenge
                    GameManager.instance.player.Win(true);
                else
                    GameManager.instance.player.Win(false);
            }
            else
            {
                //for run mode only
                if (!inGameUi.isRun)
                    return;
                if (Time.time - lastChallengeChange > timeChallengeChange)
                {
                    lastChallengeChange = Time.time;
                    ChangeItemInChallengeMode();
                }
            }
        }
    }

    //call when pause game
    public void PauseGame(bool isPause)
    {
        if (builderInRun)
            builderInRun.PauseGame(isPause);
        spawn.FreezeAllObjects(isPause);
        if (backgroundManagement)
            backgroundManagement.FreezeBackgrounds(isPause);
    }

    //call when start stage play
    public void StartStagePlay()
    {
        ResetAllSpawnNum();
        ResetLastTimeSpawn();
    }

    public void ResetAllSpawnNum()
    {
        lastIncNumTime = Time.time;
        spawn.ResetAllSpawnNum();
    }

    public void ResetLastTimeSpawn()
    {
        lastIncNumTime = Time.time;
        spawn.ResetLastTimeSpawn();
    }

    //increase difficulty
    private void incDifficulty()
    {
        //numDiff++;
        //Debug.Log("inc diff x" + numDiff);
        //Debug.Log("increase diff");
        lastIncNumTime = Time.time;
        //increase obj drop
        spawn.IncreaseFreqSpeed();
        //increase background run
        if (backgroundManagement)
            backgroundManagement.IncreaseSpeedBackground();
        //increase monster speed
        if (monster)
            monster.IncreaseSpeed(0.0005f);
        //increae water speed
        if (water)
            water.IncreaseSpeed(0.0005f);
    }

    public void BuildLadder()
    {
        ladders.AddActiveLadders(false);
    }
    public void BuildGround()
    {
        groundManager.AddGround();
    }
    public void BuildFence()
    {
        builderInRun.BuildObj(0);
    }
    public void BuildSlime()
    {
        builderInRun.BuildObj(1);
    }

    //turnOnOff sound inGame
    public void TurnOnOffInGameSound(bool isMute)
    {
        if (monster)
            monster.monsterAudioSource.mute = isMute;
        if (groundManager)
            groundManager.groundManagerAudioSource.mute = isMute;
    }

    //change sound volume
    public void ChangeSoundVolume(float num)
    {
        if (monster)
            monster.monsterAudioSource.volume = num;
        if (groundManager)
            groundManager.groundManagerAudioSource.volume = num;
    }

    //Challenge MODE()
    //change item in challengeMode
    public void ChangeItemInChallengeMode()
    {
        ChangeSpawnInChallengeMode();
        if (!inGameUi.isRun)
            ChangeWaterInChallengeMode();
        else
            ChangeMonsterInChallengeMode();
    }
    //set spawn
    private void ChangeSpawnInChallengeMode()
    {
        spawnIndex++;
        //change spawn is in inbound
        if (spawnIndex < spawnerChallengeObj.Length)
        {
            //hide all spawn
            foreach (var item in spawnerChallengeObj)
            {
                item.SetActive(false);
            }
            //show obj fall gp
            objFallGroup[spawnIndex].SetActive(true);
            //show desired spawn
            spawnerChallengeObj[spawnIndex].SetActive(true);
            //change spawn assigned
            spawn = spawnerChallenge[spawnIndex];
            //timeSpawnChange += 20;
        }
        //make win
        else
        {
            if (inGameUi.isRun)
                return;
            if (spawnIndex > 12)
                GameManager.instance.player.Win(true);
        }
    }
    //set water
    private void ChangeWaterInChallengeMode()
    {
        waterIndex++;
        //change water is in inbound
        if (waterIndex < waterChallengeObj.Length)
        {
            //hide all water
            foreach (var item in waterChallengeObj)
            {
                item.SetActive(false);
            }
            waterChallengeObj[waterIndex].SetActive(true);
            //water = waterChallenge[waterIndex];
        }
    }
    //set monster
    private void ChangeMonsterInChallengeMode()
    {
        monsterIndex++;
        //change monster is in inbound
        if (monsterIndex < monsterChallengeObj.Length)
        {
            //hide all monster
            foreach (var item in monsterChallengeObj)
            {
                item.SetActive(false);
            }
            monsterChallengeObj[monsterIndex].SetActive(true);
            //monster = monsterChallenge[monsterIndex];
        }
    }
}
