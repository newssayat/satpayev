using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Crossword_Logic
{
    public class LettersPosController : MonoBehaviour
    {
    
        public static bool isStart = false;
        public static bool isFinish = false;
        
        public int lettersCount = 3;

        public Button changePosBtn;

        private LetterController[] letters;
        public bool changing;

        public String _Chars;
        

        
        private Vector3 beginLetterPos;
        private Vector3 lastLetterPos;

        // private int[] arrayByRandomNums;
        private static Animator anim;

        private void Awake()
        {
            letters = GameObject.FindGameObjectsWithTag("letter_group")
                .Select(x => x.GetComponent<LetterController>())
                .ToArray();

            // arrayByRandomNums = new int[lettersCount];
            // RandomizeIndexes();
            initCharacters();
            SortLettersPos();
        }

        void Start()
        {


        }

        private void Update()
        {
            if (isStart)
            {

                Vector3 mouse = Input.mousePosition;
                Vector3 pos = Camera.main.ScreenToWorldPoint(mouse);


            
                if (LetterController.isLetterTouched)
                {
                    LetterController.isLetterTouched = false;

                    
                    if (LetterController.touchedLetterPos > 1)
                    {

                    }
                 
                    lastLetterPos = beginLetterPos;
                }
            


            }

            if (isFinish)
            {
                

                isFinish = false;
            }
        }

        private void initCharacters()
        {
            _Chars = _Chars.Trim();
            char[] chars = _Chars.ToCharArray();

            for (int i = 0; i < GameObject.FindGameObjectsWithTag("letter").Length; i++)
            {
                GameObject.FindGameObjectsWithTag("letter")[i].GetComponent<Text>().text = chars[i].ToString();
                GameObject.FindGameObjectsWithTag("letter")[i].name = chars[i].ToString();
            }
        }

        
        public void RandomizePosition()
        {
            if (changing)
                return;

            changing = true;

            StartCoroutine(AnimatedRandomize());
        }

        private IEnumerator AnimatedRandomize()
        {
            anim.SetTrigger("Animate");
            yield return new WaitForSeconds(0.4f);

            RandomizeIndexes();
            SortLettersPos();
        }

        private void RandomizeIndexes()
        {
            Random random = new Random();
            string oldLetters = GetLettersText();

            do
            {
                letters = letters.OrderBy(x => random.Next()).ToArray();
            } while (GetLettersText().Equals(oldLetters));
        }

        private string GetLettersText()
        {
            return String.Join("", letters.Select(x => x.textObject.name).ToArray());
        }

        private void SortLettersPos()
        {
            double corner = 360f / lettersCount;
            double sumCorner = 0;
            double radius = 150;

            for (int i = 0; i < letters.Length; i++)
            {
                double radian = sumCorner * Math.PI / 180;

                letters[i].transform.localPosition = new Vector3(
                    (float) (Math.Sin(radian) * radius),
                    (float) (Math.Cos(radian) * radius),
                    0
                );

                sumCorner += corner;
            }
        }


    }
}