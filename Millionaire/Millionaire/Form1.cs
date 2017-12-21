using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Threading;

namespace Millionaire
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        const string questionsFileName = "questions.xml";
        List<Question> questions = new List<Question>();
        Game game;

        private void dataGridRefresh() //переписать
        {
            int i = 1;

            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            foreach (var q in questions)
            {
                int select;
                if (q[0].IsTrue)
                    select = 1;
                else if (q[1].IsTrue)
                    select = 2;
                else if (q[2].IsTrue)
                    select = 3;
                else
                    select = 4;

                dataGridView1.Rows.Add(i, q.Level, q.Text, q[0].Text, q[1].Text, q[2].Text, q[3].Text, select);
                i++;
            }
        }

        private void MoneyRefresh()
        {
            richTextBox1.Clear();

            for (int i = game.MoneyCount - 1; i >= 0; i--)
            {
                if (game.CurrentMoneyNumber == i)
                {
                    richTextBox1.SelectionColor = Color.White;
                    richTextBox1.SelectionBackColor = Color.Orange;
                }

                richTextBox1.AppendText($" {game.GetMoneyValue(i).ToString("C0")} \n");
            }
        }

        private void QuestionRefresh()
        {
            if (game.QuestionNumber < 15)
            {
                label2.Text = questions[game.QuestionNumber].Text;

                button10.BackgroundImage = Properties.Resources.answerLeftDefault;
                button11.BackgroundImage = Properties.Resources.answerRightDefault;
                button14.BackgroundImage = Properties.Resources.answerRightDefault;
                button15.BackgroundImage = Properties.Resources.answerLeftDefault;

                button10.Text = questions[game.QuestionNumber][0].Text;
                button11.Text = questions[game.QuestionNumber][1].Text;
                button14.Text = questions[game.QuestionNumber][2].Text;
                button15.Text = questions[game.QuestionNumber][3].Text;
            }
            else
            {
                MessageBox.Show("Поздравляем! Вы победитель!");
                tabControl1.SelectedTab = tabControl1.TabPages["tabPageMain"];
            }
        }

        private void button_MouseEnter(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.BackgroundImage = Properties.Resources.selectHover; 
        }

        private void button_MouseLeave(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.BackgroundImage = Properties.Resources.selectDefault;
        }

        private void button_MouseDown(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            button.BackgroundImage = Properties.Resources.selectClick;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages["tabPageAbout"];
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages["tabPageMain"];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages["tabPageGame"];
            game = new Game();
            questions = Question.GetQuestionsList(questionsFileName);
            button9.BackgroundImage = Properties.Resources.help1Default;
            button12.BackgroundImage = Properties.Resources.help2Default;
            button13.BackgroundImage = Properties.Resources.help3Default;
            game.QuestionNumber = 0;
            QuestionRefresh();
            MoneyRefresh();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages["tabPageSettings"];
            dataGridRefresh();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            questions = Question.GetQuestionsList(questionsFileName);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridRefresh();
        }

        private void button7_Click(object sender, EventArgs e) //переписать
        {
            questions.Clear();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[1].Value == null)
                {
                    continue;
                }

                Answer[] answers = new Answer[] 
                {
                    new Answer(row.Cells[3].Value.ToString()),
                    new Answer(row.Cells[4].Value.ToString()),
                    new Answer(row.Cells[5].Value.ToString()),
                    new Answer(row.Cells[6].Value.ToString())
                };

                answers[Convert.ToInt32(row.Cells[7].Value.ToString()) - 1].IsTrue = true;
                questions.Add(new Question(row.Cells[2].Value.ToString(), Convert.ToInt32(row.Cells[1].Value.ToString()), answers));
            }

            Question.SetQuestionsList(questions, questionsFileName);
            dataGridRefresh();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (game.IsHelp(0))
            {
                button9.BackgroundImage = Properties.Resources.help1Click;
                game.GetHelp(0, questions);
                QuestionRefresh();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (game.IsHelp(1))
            {
                button12.BackgroundImage = Properties.Resources.help2Click;
                game.GetHelp(1, questions);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (game.IsHelp(2))
            {
                button13.BackgroundImage = Properties.Resources.help3Click;
                game.GetHelp(2, questions);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (button.Text != "")
            {
                button.BackgroundImage = Properties.Resources.answerLeftHover;
                button.Update();

                Thread.Sleep(1000);

                if (button.Text == questions[game.QuestionNumber].GetTryeAnswerText())
                {
                    button.BackgroundImage = Properties.Resources.answerLeftClick;
                    button.Update();
                    Thread.Sleep(1000);
                    game.CurrentMoneyNumber++;
                    game.QuestionNumber++;
                    QuestionRefresh();
                    MoneyRefresh();
                }
                else
                {
                    MessageBox.Show("Это не правильный ответ! Ваш выигрыш: " + game.GetMoneyValue(game.CurrentMoneyNumber - 1));
                    tabControl1.SelectedTab = tabControl1.TabPages["tabPageMain"];
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (button.Text != "")
            {
                button.BackgroundImage = Properties.Resources.answerRightHover;
                button.Update();

                Thread.Sleep(1000);

                if (button.Text == questions[game.QuestionNumber].GetTryeAnswerText())
                {
                    button.BackgroundImage = Properties.Resources.answerRightClick;
                    button.Update();
                    Thread.Sleep(1000);
                    game.CurrentMoneyNumber++;
                    game.QuestionNumber++;
                    QuestionRefresh();
                    MoneyRefresh();
                }
                else
                {
                    MessageBox.Show("Это не правильный ответ! Ваш выигрыш: " + game.GetMoneyValue(game.CurrentMoneyNumber - 1));
                    tabControl1.SelectedTab = tabControl1.TabPages["tabPageMain"];
                }
            }
        }
    }
}
