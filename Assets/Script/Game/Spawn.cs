using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private InGame inGame;
    public GameObject[] charObj, obstacleObj, bookObj, coinObj;
    private int index, charIndex, obsIndex, bookIndex, coinIndex;
    private float posX;
    //char
    public bool isSpawnStop, isTutorialMode;
    //alphabet list
    private char[] alphabets =
    {
        'A', 'A', 'A', 'A', 'A', 'A', 'A',
        'B', 'B', 'B',
        'C', 'C', 'C',
        'D', 'D', 'D',
        'E', 'E', 'E', 'E', 'E', 'E', 'E',
        'F', 'F', 'F',
        'G', 'G', 'G', 'G', 'G', 'G',
        'H', 'H', 'H',
        'I', 'I', 'I', 'I', 'I', 'I', 'I',
        'J', 'J',
        'K', 'K', 'K',
        'L', 'L', 'L',
        'M', 'M', 'M',
        'N', 'N', 'N', 'N', 'N', 'N',
        'O', 'O', 'O', 'O', 'O', 'O', 'O',
        'P', 'P',
        'Q',
        'R', 'R',
        'S', 'S',
        'T',
        'U', 'U', 'U', 'U', 'U', 'U', 'U',
        'V',
        'W', 'W',
        'X',
        'Y', 'Y', 'Y',
        'Z'
    };

    void Start()
    {
        //store ori value
        inGame.timeCharDurationOri = inGame.timeCharDuration;
        inGame.timeObsDurationOri = inGame.timeObsDuration;
        inGame.dragCharOri = inGame.dragChar;
        inGame.dragObsOri = inGame.dragObs;
        inGame.dragCoinOri = inGame.dragCoin;
        inGame.dragBookOri = inGame.dragBook;
        ChangeFreqSpeedChar(inGame.dragChar, inGame.timeCharDuration);
        ChangeFreqSpeedObs(inGame.dragObs, inGame.timeObsDuration);
        ChangeFreqSpeedCoin(inGame.dragCoin);
        ChangeFreqSpeedBook(inGame.dragBook);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isStartGame)
            return;
        if (GameManager.instance.isPauseGame)
            return;
        if (isSpawnStop)
            return;
        if (charObj.Length > 0)
            SpawnChar();
        if (obstacleObj.Length > 0)
            SpawnObstacle();
        if (coinObj.Length > 0)
            SpawnCoin();
        if (inGame.isBookSpawnOne)
        {
            if (bookObj.Length > 0)
                SpawnBookOne();
        }
        else
        {
            if (bookObj.Length > 0)
                SpawnBook();
        }
    }

    public void SpawnChar()
    {
        //check duration
        if (Time.time - inGame.lastCharTime > inGame.timeCharDuration)
        {
            inGame.lastCharTime = Time.time;
            //Debug.Log("Spawn char");
            SpawnObject(charObj[charIndex]);
            //TUTORIAL MODE
            if (!isTutorialMode)
                charObj[charIndex].GetComponent<Char>().SetAlphabet(alphabets[Random.Range(0, alphabets.Length)]);
            charObj[charIndex].GetComponent<Char>().ChangeIsTouched(false);
            charIndex++;
            if (charIndex == charObj.Length - 1)
                charIndex = 0;
        }
    }

    public void SpawnObstacle()
    {
        //check duration
        if (Time.time - inGame.lastObsTime > inGame.timeObsDuration)
        {
            inGame.lastObsTime = Time.time;
            //Debug.Log("Spawn obs");
            SpawnObject(obstacleObj[obsIndex]);
            obstacleObj[obsIndex].GetComponent<Obstacle>().ChangeIsTouched(false);
            obsIndex++;
            if (obsIndex == obstacleObj.Length - 1)
                obsIndex = 0;
        }
    }

    public void SpawnCoin()
    {
        //check duration
        if (Time.time - inGame.lastCoinTime > inGame.timeCoinDuration)
        {
            inGame.lastCoinTime = Time.time;
            //Debug.Log("Spawn obs");
            SpawnObject(coinObj[coinIndex]);
            coinObj[coinIndex].GetComponent<Coin>().ChangeIsTouched(false);
            coinIndex++;
            //Debug.Log("spawn coin");
            if (coinIndex == coinObj.Length - 1)
                coinIndex = 0;
        }
    }

    public void SpawnBook()
    {
        //check duration
        if (Time.time - inGame.lastBookTime > inGame.timeBookDuration)
        {
            inGame.lastBookTime = Time.time;
            //Debug.Log("Spawn obs");
            SpawnObject(bookObj[bookIndex]);
            bookObj[bookIndex].GetComponent<Book>().ChangeIsTouched(false);
            bookIndex++;
            //Debug.Log("spawn book");
            if (bookIndex == bookObj.Length - 1)
                bookIndex = 0;
        }
    }
    //spawn book one time only
    public void SpawnBookOne()
    {
        inGame.timeBook += Time.deltaTime;
        if (inGame.timeBook != inGame.bookSpawnTime)
            return;
        SpawnObject(bookObj[bookIndex]);
        bookObj[bookIndex].GetComponent<Book>().ChangeIsTouched(false);
        //Debug.Log("spawn book");
    }

    //spawn object
    private void SpawnObject(GameObject obj)
    {
        index = Random.Range(0, 1);
        posX = Random.Range(-GameManager.instance.boundary.boundX + 0.2f, GameManager.instance.boundary.boundX - 0.2f);
        if (index == 0)
        {
            if (!obj.GetComponent<Obstacle>())
                obj.transform.position = new Vector3(posX, transform.position.y, transform.position.z);
            else
                obj.transform.position = new Vector3(-posX, transform.position.y, transform.position.z);
        }
    }

    //reset all variable for spawn num
    public void ResetAllSpawnNum()
    {
        ResetLastTimeSpawn();
        inGame.timeBook = 0;
        charIndex = 0;
        obsIndex = 0;
        coinIndex = 0;
        bookIndex = 0;
    }

    //reset last time spawn
    public void ResetLastTimeSpawn()
    {
        inGame.lastCharTime = Time.time;
        inGame.lastObsTime = Time.time;
        inGame.lastCoinTime = Time.time;
        inGame.lastBookTime = Time.time;
    }

    //call when want pause game
    public void FreezeAllObjects(bool isPause)
    {
        if (isPause)
        {
            foreach (var item in charObj)
            {
                item.GetComponent<Char>().PauseGame(true);
            }
            foreach (var item in obstacleObj)
            {
                item.GetComponent<Obstacle>().PauseGame(true);
            }
            foreach (var item in bookObj)
            {
                item.GetComponent<Book>().PauseGame(true);
            }
            foreach (var item in coinObj)
            {
                item.GetComponent<Coin>().PauseGame(true);
            }
        }
        else
        {
            foreach (var item in charObj)
            {
                item.GetComponent<Char>().PauseGame(false);
            }
            foreach (var item in obstacleObj)
            {
                item.GetComponent<Obstacle>().PauseGame(false);
            }
            foreach (var item in bookObj)
            {
                item.GetComponent<Book>().PauseGame(false);
            }
            foreach (var item in coinObj)
            {
                item.GetComponent<Coin>().PauseGame(false);
            }
        }
    }

    public void StopSpawn(bool isStop)
    {
        isSpawnStop = isStop;
    }

    public void IncreaseFreqSpeed()
    {
        //Debug.Log("inc obj speed");
        inGame.increaseNum += 0.003f;
        inGame.increaseNumObs += 0.004f;
        inGame.increaseNumCoin += 0.005f;
        inGame.increaseNumBook += 0.006f;
        //Debug.Log("increase num = " + increaseNum);
        //Debug.Log("dragChar = " + dragChar);
        ChangeFreqSpeedChar(inGame.dragChar - inGame.increaseNum, inGame.timeCharDuration - inGame.increaseNum / 2);
        ChangeFreqSpeedObs(inGame.dragObs - inGame.increaseNumObs, inGame.timeObsDuration - inGame.increaseNumObs / 2);
        //Debug.Log("increase num = " + increaseNum);
        ChangeFreqSpeedCoin(inGame.dragCoin - inGame.increaseNumCoin);
        ChangeFreqSpeedBook(inGame.dragBook - inGame.increaseNumBook);
    }

    //variable change
    public void ChangeFreqSpeedChar(float dragNum, float duration)
    {
        //Debug.Log("dragNum = " + dragNum);
        //Debug.Log("duration = " + duration);
        if (duration < 0.15f)
            return;
        if (dragNum < 1.0f)
            return;
        inGame.timeCharDuration = duration;
        foreach (var item in charObj)
        {
            item.GetComponent<Rigidbody2D>().drag = dragNum;
        }
    }
    public void ChangeFreqSpeedObs(float dragNum, float duration)
    {
        if (duration < 0.2f)
            return;
        if (dragNum < 0.8f)
            return;
        inGame.timeObsDuration = duration;
        foreach (var item in obstacleObj)
        {
            item.GetComponent<Rigidbody2D>().drag = dragNum;
        }
    }
    public void ChangeFreqSpeedCoin(float dragNum)
    {
        if (dragNum < 0.45f)
            return;
        foreach (var item in coinObj)
        {
            item.GetComponent<Rigidbody2D>().drag = dragNum;
        }
    }
    public void ChangeFreqSpeedBook(float dragNum)
    {
        if (dragNum < 0.35f)
            return;
        foreach (var item in bookObj)
        {
            item.GetComponent<Rigidbody2D>().drag = dragNum;
        }
    }


}
