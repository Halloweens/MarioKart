using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Collections;

public class HighscoreScreen : MonoBehaviour
{

    private string highscoreFileName = "Highscores_save.txt";

    private Dictionary<string, float> nameList = new Dictionary<string, float>();
    KeyValuePair<string, float> lastkey;

    void Start()
    {
        if (!File.Exists(highscoreFileName))
        {
            CreateANewFile();
        }
        else
        {
            ParseFileToDictio();
        }

        Debug.Log("Sort result by time");
        List<float> sortingByTime = new List<float>();


        foreach (KeyValuePair<string, float> pair in nameList)
        {
            sortingByTime.Add(pair.Value);
        }
        sortingByTime.Sort();
        Debug.Log(sortingByTime.Count);
        int idx = 0;

        StartCoroutine(PrintOnScreen(idx, sortingByTime));
    }

    void CreateANewFile()
    {
        File.Create(highscoreFileName);
    }
    void ParseFileToDictio()
    {
        string[] linesInFile = File.ReadAllLines(highscoreFileName);

        foreach (string line in linesInFile)
        {
            string[] parsedLine = line.Split('|');
            nameList.Add(parsedLine[0], (float)Convert.ToDouble(parsedLine[1]));
        }
    }

    void PrintName(int i, List<float> sortingByTime)
    {
        int num = 0;
        if (i >= 10)
            num = i % 10;
        else
            num = i;

        GameObject nameText = GameObject.Find("Name (" + (num + 1).ToString() + ")");

        if (nameText && nameText.GetComponent<Text>())
        {
            foreach (KeyValuePair<string, float> pair in nameList)
            {
                if (pair.Value == sortingByTime[i] && pair.Key != lastkey.Key)
                {
                    nameText.GetComponent<Text>().text = pair.Key;
                    lastkey = pair;

                    return;
                }
            }
        }
    }
    void PrintTime(int i, List<float> sortingByTime)
    {
        int num = 0;
        if (i >= 10)
            num = i % 10;
        else
            num = i;

        GameObject timeText = GameObject.Find("Time (" + (num + 1).ToString() + ")");
        if (timeText && timeText.GetComponent<Text>())
        {
            timeText.GetComponent<Text>().text = sortingByTime[i].ToString();
        }
    }

    IEnumerator PrintOnScreen(int idx, List<float> sortingByTime)
    {
        while (true)
        {
            for (int i = idx; i < (idx + 10); i++)
            {
                PrintName(i, sortingByTime);
                PrintTime(i, sortingByTime);
            }
            idx += 10;
            yield return new WaitForSeconds(5f);

            if (idx >= 40)
                idx = 0;
        }
    }
}
