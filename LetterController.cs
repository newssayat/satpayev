using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Crossword_Logic
{
    public class LetterController : MonoBehaviour
    {
        
        public static bool isLetterTouched = false;
        public static int touchedLetterPos = 0;
        public static Vector3 findedObjectCoor;

        public GameObject crossword;
        private CrosswordController crosswordController;

        public GameObject backgroundObject;
        public GameObject textObject;

        private GameObject startObject;

        private Vector3 direction;

        private Vector3 touchPosition;
        private Rigidbody2D rb;

        private bool isObjectFind = false;
        
        private GameObject letter;

        public Text test;

        private void Awake()
        {
            crosswordController = crossword.GetComponent<CrosswordController>();
        }

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            foreach (var answer in crosswordController.answers)
            {
                crosswordController.FindWord(
                    PlayerPrefs.GetString("level" + GameManager.instance.currentLevel + "/" + answer.ToLower(),
                        ""));
            }

            for (int i = 0; i < crosswordController.CountLettersLength(); i++)
            {
                crosswordController.FindOneLetter(PlayerPrefs.GetInt("level" + GameManager.instance.currentLevel + "/" + i,
                    -1));
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.touchCount > 0)
            {
                EnterLetters();
            }
            else
            {
                if (isObjectFind && backgroundObject.transform.localScale.x > 0f)
                {
                    touchedLetterPos = 0;
                    LettersPosController.isStart = false;
                    LettersPosController.isFinish = true;
                    
                    isObjectFind = !RemoveGameObjects();
                }
            }
        }

        private void EnterLetters()
        {
            Touch touch = Input.GetTouch(0);
            if (Camera.main != null) touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            Vector3 obPosition = GameObject.Find(backgroundObject.name).transform.position;

            bool check = (Mathf.Abs(touchPosition.x - obPosition.x) <= 0.25f) &&
                         (Mathf.Abs(touchPosition.y - obPosition.y) <= 0.25f);
            if (check)
            {
                startObject = GameObject.Find(backgroundObject.name);
                letter = GameObject.Find(textObject.name);
                isObjectFind = true;
                
                LettersPosController.isStart = true;
            }

            if (isObjectFind && backgroundObject.transform.localScale.x < 1f)
            {
                
                isLetterTouched = true;
                findedObjectCoor = startObject.transform.position;

                touchedLetterPos++;
                
                CrosswordController.word += letter.GetComponent<Text>().text;
                test.text = CrosswordController.word;
                startObject.transform.localScale = new Vector3(1,1,1);
            }
        }

        private bool RemoveGameObjects()
        {
            var bgScale = startObject.transform.localScale;
            bgScale -= new Vector3(bgScale.x, 0, 0);
            startObject.transform.localScale = bgScale;

            if (CrosswordController.word.Length == 1)
            {
                CrosswordController.word = "";
            }


            test.text = CrosswordController.word;
            if (crosswordController.FindWord(CrosswordController.word))
            {
                PlayerPrefs.SetString(
                    "level" + GameManager.instance.currentLevel + "/" + CrosswordController.word.ToLower(),
                    CrosswordController.word);


            }
            else
            {

            }

            CrosswordController.word = "";
            return true;
        }
    }
}