using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Random = System.Random;

namespace Crossword_Logic
{
    public class CrosswordController : MonoBehaviour
    {
        public GameObject lettersPosLogic;
        public Color colorTrueAnswer = new Color(27, 200, 0, 150);
        public Color colorText = new Color(0, 0, 0, 255);

        public static String word = "";
        public int solvedTasks = 0;

        private int[] columnsEachAnswer;

        public List<String> solutions;
        public List<String> answers;
        private GameObject[] answersCols;

        // For Buy Word
        private int[] randomLettersPos;
        private bool isPressedFindWord = false;

        private GameObject[] columns;
        public List<GameObject> letters;
        private GameObject[] unSortedLetters;

        private void Awake()
        {
            initSolutions();
            answers = new List<string>();
            solvedTasks = 0;
            unSortedLetters = GameObject.FindGameObjectsWithTag("true_answer");
            columns = GameObject.FindGameObjectsWithTag("column");
        }

        private void initSolutions()
        {
            for (int i = 0; i < solutions.Count; i++)
            {
                solutions[i] = solutions[i].Trim();
                for (int j = 0; j < solutions[i].Length; j++)
                {
                    string location = "answer" + (i + 1) + "/col" + (j + 1) + "/Image" + "/Text";
                    GameObject.Find(location).GetComponent<Text>().text = solutions[i][j].ToString();
                }
            }
        }
        
        // Start is called before the first frame update
        private void Start()
        {
            GetSavedAnswers();

            changeColorTexts();

            // For Buy Words
            RandomizeLettersForBuyWords();
        }

        public void changeColorTexts()
        {
            foreach (GameObject text in GameObject.FindGameObjectsWithTag("text"))
            {
                text.GetComponent<Text>().color = colorText;
                text.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        public int CountLettersLength()
        {
            int length = 0;
            for (int i = 0; i < unSortedLetters.Length; i++)
            {
                if (columns[i].GetComponent<Image>().color.a < 0.05)
                {
                    // letters = letters.Where((source, index) => index != i).ToArray();
                }
                else
                {
                    length++;
                }
            }

            return length;
        }

        private void RandomizeLettersForBuyWords()
        {
            letters.RemoveAll(it => true);
            for (int i = 0; i < unSortedLetters.Length; i++)
            {
                if (columns[i].GetComponent<Image>().color.a < 0.05)
                {
                    // letters = letters.Where((source, index) => index != i).ToArray();
                }
                else
                {
                    letters.Add(unSortedLetters[i]);
                }
            }

            randomLettersPos = new int[letters.Count];
            int[] positions = new int[letters.Count];
            for (int i = 0; i < letters.Count; i++)
            {
                positions[i] = i;
            }

            Random rnd = new Random();
            randomLettersPos = positions.OrderBy(x => rnd.Next()).ToArray();
        }

        private void GetSavedAnswers()
        {
            answersCols = GameObject.FindGameObjectsWithTag("answer_cols");
            columnsEachAnswer = new int[answersCols.Length];
            for (int i = 0; i < answersCols.Length; i++)
            {
                int col_pos = 1;
                while (GameObject.Find("answer" + (i + 1) + "/col" + col_pos) != null)
                {
                    columnsEachAnswer[i]++;
                    col_pos++;
                }

                String answer = "";
                for (int j = 0; j < columnsEachAnswer[i]; j++)
                {
                    string location = "answer" + (i + 1) + "/col" + (j + 1) + "/Image" + "/Text";
                    answer += GameObject.Find(location).GetComponent<Text>().text;

                    string bgLetterLocation = "answer" + (i + 1) + "/col" + (j + 1) + "/Image" + "/Image";
                    GameObject.Find(bgLetterLocation).GetComponent<Image>().color = colorTrueAnswer;
                    GameObject.Find(bgLetterLocation).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                }



                if (PlayerPrefs.GetString("level" + GameManager.instance.currentLevel + "/" + answer.ToLower(),
                    null) != null)
                {
                    answers.Add(answer);
                }
            }
        }


        // Update is called once per frame
        void Update()
        {
            if (answers.Count == solvedTasks)
            {
                GameManager.instance.IncreaseLevel();
            }
        }

        public bool FindWord(String word)
        {
            for (int i = 0; i < answers.Count; i++)
            {
                if (answers[i].ToLower() == word.ToLower())
                {
                    for (int j = 0; j < answers[i].Length; j++)
                    {
                        string location = "answer" + (i + 1) + "/col" + (j + 1) + "/Image";
                        if (GameObject.Find(location).transform.localScale.x < 1f)
                        {
                            GameObject.Find(location).transform.localScale += Vector3.right;

                        }
                    }

                    answers[i] = "FoundWord";
                    solvedTasks++;
                    return true;
                }
            }

            return false;
        }



        public void FindOneLetter(int position)
        {
            RandomizeLettersForBuyWords();
            // letters = GameObject.FindGameObjectsWithTag("text");
            if (position >= 0)
            {
                letters[position].transform.localScale = new Vector3(1, 1, 1);
            }
        }

        public void BuyWord()
        {
            if (!isPressedFindWord)
            {
                isPressedFindWord = true;
                int score = GameManager.instance.score;
                if (score < 0)
                {
                    return;
                }

                for (int i = 0; i < randomLettersPos.Length; i++)
                {
                    if (letters[randomLettersPos[i]].transform.localScale.x == 0)
                    {
                        letters[randomLettersPos[i]].transform.localScale = new Vector3(1, 1, 1);

                        int pos = randomLettersPos[i];
                        PlayerPrefs.SetInt("level" + GameManager.instance.currentLevel + "/" + pos, pos);

                        score -= 0;
                        GameManager.instance.score = score;
                        PlayerPrefs.SetInt("score", score);


                        isPressedFindWord = false;
                        return;
                    }
                }
            }
        }
    }
}