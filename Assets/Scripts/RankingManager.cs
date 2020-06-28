using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{

    string rankKey = "rank";
    string rankNameKey = "Name";
    //랭킹 갱신함수
    void GetRanking()
    {
        SortRank();
        int nameCound = 1;
        for(int index = 0; index < 9; index++)
        {
            if (PlayerPrefs.HasKey(rankKey + nameCound))
            {
                transform.GetChild(index).Find("Score").GetComponent<Text>().text = PlayerPrefs.GetString(rankKey + nameCound);
                transform.GetChild(index).Find("Name").GetComponent<Text>().text = PlayerPrefs.GetString(rankNameKey + nameCound);
            }                               
            else
            {
                transform.GetChild(index).Find("Score").GetComponent<Text>().text = "0";
                transform.GetChild(index).Find("Name").GetComponent<Text>().text = "ABC";
            }
            nameCound++;
        }
    }

    void SortRank()
    {
        int nameCound = 1;
        int temp;
        string tempString;
        int [] tempNum = new int[9];
        string[] tempName = new string[9];
        for (int index = 0; index < 9; index++)
        {
            if (PlayerPrefs.HasKey(rankKey + nameCound))
            {
                tempNum[index] = Convert.ToInt32(PlayerPrefs.GetString(rankKey + nameCound));
                tempName[index] = PlayerPrefs.GetString(rankNameKey + nameCound);
            }
            else
            {
                tempNum[index] = 0;
                tempName[index] = "ABC";
            }
            nameCound++;
        }

        for(int i = 0; i < 8; i++)
        {
            for(int j = i+1; j < 9; j++)
            {
                if(tempNum[i] > tempNum[j])
                {
                    temp = tempNum[i];
                    tempNum[i] = tempNum[j];
                    tempNum[j] = temp;

                    tempString = tempName[i];
                    tempName[i] = tempName[j];
                    tempName[j] = tempString;
                }
            }
        }

        for(int index = 0; index < 9; index ++)
        {
            nameCound--;
            PlayerPrefs.SetString(rankKey + nameCound, tempNum[index].ToString());
            PlayerPrefs.SetString(rankNameKey + nameCound, tempName[index]);            
        }

    }

    void Start()
    {
        GetRanking();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
