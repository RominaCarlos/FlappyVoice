using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FlappyVoice
{
    class InfoList
    {
        public string PlayerName { get; set; }
        public int Highscore { get; set; }
        public int TrackedClicks { get; set; }
        private string FileName = @"../../GameInfo.txt";
        //private FileStream FS { get; set; }
        //private StreamReader SR { get; set; }
        //public StreamWriter SW { get; set; }

        public InfoList()
        {
            GenerateEmptyInfoList();

        }

        public void GenerateEmptyInfoList()
        {
            if (File.Exists(FileName) == false)
            {
                FileStream FS = File.Create(FileName);
                FS.Close();
                StreamWriter SW = new StreamWriter(FileName);
                SW.Write("Josh;");
                SW.Close();
            }
        }

        public void ReadInfoList(string Name, int Score, int Clicks)
        {
            string line;
            using (StreamReader SR = new StreamReader(FileName))
            {
                GenerateEmptyInfoList();
                while (SR.EndOfStream == false)
                {
                    line = SR.ReadLine();
                    string[] splitLine = line.Split(';');
                    if (Name == splitLine[0] /*|| Score == Int32.Parse(splitLine[1]) || Clicks == Int32.Parse(splitLine[2])*/)
                    {

                        WriteInfoList(splitLine, Score, Clicks);
                    }
                }
                /*
                while (line != null)
                {
                    if (line.Contains(Name))
                    {
                        break;
                    }
                }*/

            }
        }

        public void WriteInfoList(string[] playerInfo, int Score, int Clicks)
        {
            if (Score > Int32.Parse(playerInfo[1]))
            {
                using (StreamWriter SW = new StreamWriter(FileName, false))
                {
                    SW.Write($"{playerInfo[1].ToString()};{playerInfo[2].ToString()}");
                }
            }


        }
    }
}
