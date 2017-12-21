using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Millionaire
{
    class Question
    {
        string text;
        int level;
        Answer[] answers = new Answer[4];

        public Question(string text, int level, Answer[] answers)
        {
            this.text = text;
            this.level = level;
            this.answers = answers;
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public Answer this[int i]
        {
            get { return answers[i]; }
            set { answers[i] = value; }
        }

        public static List<Question> GetQuestionsList(string filename)
        {
            XmlDocument xDoc = new XmlDocument();

            try
            {
                xDoc.Load(filename);
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("Ошибка доступа к файлу с вопросами!\n" + e.Message);
                return null;
            }

            XmlElement xRoot = xDoc.DocumentElement;
            List<Question> questions = new List<Question>();

            foreach (XmlNode xNode in xRoot)
            {
                Answer[] answers = new Answer[4];
                string text = "";
                int level = 0;
                int i = 0;

                foreach (XmlNode childNode in xNode.ChildNodes)
                {
                    if (childNode.Name == "text")
                        text = childNode.InnerText;

                    if (childNode.Name == "level")
                        level = Convert.ToInt32(childNode.InnerText);

                    if (childNode.Name == "answer" && childNode.Attributes.Count > 0)
                    {
                        XmlNode attr = childNode.Attributes.GetNamedItem("value");

                        if (attr.Value == "true")
                        {
                            answers[i] = new Answer(childNode.InnerText, true);
                            i++;
                        }
                    }
                    else if (childNode.Name == "answer")
                    {
                        answers[i] = new Answer(childNode.InnerText);
                        i++;
                    }
                }

                questions.Add(new Question(text, level, answers));
            }

            return questions;
        }

        public static void SetQuestionsList(List<Question> questions, string filename)
        {
            XDocument xDoc = new XDocument();
            XElement questionsElement = new XElement("questions");

            foreach (var q in questions)
            {
                XElement questionElement = new XElement("question");
                XElement text = new XElement("text", q.Text);
                XElement level = new XElement("level", q.Level);
                questionElement.Add(text);
                questionElement.Add(level);

                foreach (var a in q.answers)
                {
                    XElement answer = new XElement("answer", a.Text);

                    if (a.IsTrue)
                    {
                        XAttribute answerAttr = new XAttribute("value", "true");
                        answer.Add(answerAttr);
                    }
                    questionElement.Add(answer);
                }

                questionsElement.Add(questionElement);
            }

            xDoc.Add(questionsElement);
            xDoc.Save(filename);
        }

        public string GetTryeAnswerText()
        {
            string text = "";

            foreach (var a in answers)
            {
                if (a.IsTrue)
                {
                    text = a.Text;
                    break;
                }
            }
            return text;
        }

        public int GetTryeAnswerIndex()
        {
            int index = 0;

            foreach (var a in answers)
            {
                if (a.IsTrue)
                    break;
            
                index++;
            }

            return index;
        }
    }
}
