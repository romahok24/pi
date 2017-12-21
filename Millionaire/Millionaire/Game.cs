using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Millionaire
{
    class Game
    {
        private bool[] helps = new bool[3];
        private int[] money = new int[15];
        private int currentMoneyNumber;
        private int level;
        private int questionNumber;

        public Game()
        {
            helps = new bool[] { true, true, true };
            money = new int[] { 50, 100, 300, 500, 1000, 2000, 4000, 8000, 16000, 32000, 64000,
                125000, 250000, 500000, 1000000 };
            currentMoneyNumber = 0;
            level = 1;
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public int MoneyCount
        {
            get { return money.Length; }
        }

        public int CurrentMoneyNumber
        {
            get { return currentMoneyNumber; }
            set { currentMoneyNumber = value; }
        }

        public int QuestionNumber
        {
            get { return questionNumber; }
            set { questionNumber = value; }
        }

        public int GetMoneyValue(int index)
        {
            if (index < 0)
                return 0;
            return money[index];
        }

        public bool IsHelp(int index)
        {
            return helps[index];
        }

        public List<Question> GetHelp(int number, List<Question> questions)
        {
            Random r = new Random();
            int randomNum;
            string text = "";
            int index = questions[questionNumber].GetTryeAnswerIndex();

            if (number == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i == index)
                    {
                        continue;
                    }
                    else
                    {
                        questions[questionNumber][i].Text = "";
                        i++;
                    }

                }
            }
            else if (number == 1)
            {
                string[] phrases = new string[] {
                    "Я уверен, это ",
                    "Абсолютно точно, это ",
                    "Я не уверен, но скорее всего это ",
                    "Скорее всего это ",
                    "Я бы ответил "
                };

                randomNum = r.Next(0, phrases.Length - 1);
                text = phrases[randomNum] + questions[questionNumber].GetTryeAnswerText();
                MessageBox.Show(text);
            }
            else
            {
                randomNum = r.Next(50, 100);
                int[] percents = new int[4];
                percents[0] = r.Next(0, 100 - randomNum);
                percents[1] = r.Next(0, 100 - randomNum - percents[0]);
                percents[2] = 100 - randomNum - percents[0] - percents[1];
                percents[3] = 100 - randomNum;


                for (int i = 0, j = 0; i < 4; i++)
                {
                    if (i == index)
                        text += questions[questionNumber][i].Text + " - " + randomNum + "%\n";

                    else if (questions[questionNumber][i].Text == "")
                    {
                        j = 3;

                        continue;
                    }

                    else
                    {
                        text += questions[questionNumber][i].Text + " - " + percents[j] + "%\n";
                        j++;
                    }
                }

                MessageBox.Show("Результаты голосования:\n\n" + text);

            }

            helps[number] = false;

            return questions;
        }

    }
}
