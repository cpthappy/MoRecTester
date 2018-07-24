using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MoRecTester.IO
{
    class CsvReader
    {
        /// <summary>
        /// The 9X4 _ActionDecider array represents the transition table.
        /// First dimension represents state (S0 to S8) while second dimension represents 
        /// input character (0 to 3). The array gives the next state based on the current state 
        /// and input character. For example, if the current state is 3 and next input character is 
        /// quote (0), then the next state is _ActionDecider[3][0] i.e. 4.
        /// 
        /// " (double quote) (Indicated by 0)
        /// , (Comma) (Indicated by 1)
        /// N(newline) (Indicated by 3)
        /// O(character other than , " and N) (Indicated by 2)
        /// </summary>
        private readonly int[][] _ActionDecider = new int[9][];

        /// <summary>
        /// Path to csv file
        /// </summary>
        public FileInfo FileInfo { get; private set; }

        /// <summary>
        /// Gibt an, ob die Csv-Datei eine Überschrift hat
        /// </summary>
        public bool HasHeader { get; private set; }

        /// <summary>
        /// Aktuelle Zeilennummer
        /// </summary>
        public int CurrentLine { get; private set; }

        public CsvReader(string fileName, bool hasHeader)
        {
            BuildTransitionTable();
            FileInfo = new FileInfo(fileName);
            HasHeader = hasHeader;

        }

        private void BuildTransitionTable()
        {
            //Build the state array
            _ActionDecider[0] = new int[4] { 2, 0, 1, 5 };
            _ActionDecider[1] = new int[4] { 6, 0, 1, 5 };
            _ActionDecider[2] = new int[4] { 4, 3, 3, 6 };
            _ActionDecider[3] = new int[4] { 4, 3, 3, 6 };
            _ActionDecider[4] = new int[4] { 2, 8, 6, 7 };
            _ActionDecider[5] = new int[4] { 5, 5, 5, 5 };
            _ActionDecider[6] = new int[4] { 6, 6, 6, 6 };
            _ActionDecider[7] = new int[4] { 5, 5, 5, 5 };
            _ActionDecider[8] = new int[4] { 0, 0, 0, 0 };
        }

        public IEnumerable<List<string>> ParseFile()
        {
            using (var file = new StreamReader(FileInfo.FullName))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    if (!HasHeader || CurrentLine > 0)
                    {
                        yield return ParseLine(line);
                    }
                    CurrentLine++;
                }
            }
        }

        /// <summary>
        /// Parse a single line and return entry with column data
        /// </summary>
        /// <param name="strInputString">input string</param>
        /// <returns>array list</returns>
        private List<string> ParseLine(string strInputString)
        {
            var currentElement = new StringBuilder();
            var parsedCsv = new List<string>();
            int counter = 0;
            int currentState = 0;

            for (counter = 0; counter < strInputString.Length; counter++)
            {
                currentState = _ActionDecider[currentState][GetInputID(strInputString[counter])];
                //take the necessary action depending upon the state 
                currentState = PerformAction(currentState, strInputString[counter], ref currentElement, ref parsedCsv);
            }
            //End of line reached, hence input ID is 3
            currentState = _ActionDecider[currentState][3];
            PerformAction(currentState, '\0', ref currentElement, ref parsedCsv);

            return parsedCsv;
        }

        /// <summary>
        /// Convert input character to state id
        /// </summary>
        /// <param name="chrInput">input character</param>
        /// <returns></returns>
        private int GetInputID(char chrInput)
        {
            switch (chrInput)
            {
                case '"': return 0;
                case ',': return 1;
                case '\n': return 3;
                default: return 2;
            }
        }

        /// <summary>
        /// Perform action indicated by currentState and inputChar, update currentElement and parsedCsv
        /// </summary>
        /// <param name="currentState">current state</param>
        /// <param name="inputChar">input character</param>
        /// <param name="currentElement">string with current element</param>
        /// <param name="parsedCsv">list with all entries in line</param>
        /// <returns>new state</returns>
        private int PerformAction(int currentState, char inputChar, ref StringBuilder currentElement, ref List<string> parsedCsv)
        {
            var newState = currentState;
            string strTemp = null;
            switch (currentState)
            {
                case 0:
                    //Separate out value to array list
                    strTemp = currentElement.ToString();
                    parsedCsv.Add(strTemp);
                    currentElement = new StringBuilder();
                    break;
                case 1:
                case 3:
                case 4:
                    //accumulate the character
                    currentElement.Append(inputChar);
                    break;
                case 5:
                    //End of line reached. Separate out value to array list
                    strTemp = currentElement.ToString();
                    parsedCsv.Add(strTemp);
                    break;
                case 6:
                    //Erroneous input. Reject line.
                    parsedCsv.Clear();
                    break;
                case 7:
                    //wipe ending " and Separate out value to array list
                    currentElement.Remove(currentElement.Length - 1, 1);
                    strTemp = currentElement.ToString();
                    parsedCsv.Add(strTemp);
                    currentElement = new StringBuilder();
                    newState = 5;
                    break;
                case 8:
                    //wipe ending " and Separate out value to array list
                    currentElement.Remove(currentElement.Length - 1, 1);
                    strTemp = currentElement.ToString();
                    parsedCsv.Add(strTemp);
                    currentElement = new StringBuilder();
                    //goto state 0
                    newState = 0;
                    break;
            }

            return newState;
        }
    }
}
