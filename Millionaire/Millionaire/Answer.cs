using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire
{
    class Answer
    {
        string text;
        bool isTrue;

        public Answer(string text, bool isTrue)
        {
            this.text = text;
            this.isTrue = isTrue;
        }

        public Answer(string text)
        {
            this.text = text;
            isTrue = false;
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public bool IsTrue
        {
            get { return isTrue; }
            set { isTrue = value; }
        }
    }
}
