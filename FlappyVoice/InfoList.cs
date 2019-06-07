using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyVoice
{
    static class InfoList
    {
        static public string PlayerName { get; set; }
        static public int Highscore { get; set; }
        static public int TrackedClicks { get; set; }
        static private string FileName = @"../../GameInfo.txt";
        static public string[] AllLines = File.ReadAllLines(FileName);
        //private FileStream FS { get; set; }
        //private StreamReader SR { get; set; }
        //public StreamWriter SW { get; set; }

        static public void GenerateEmptyInfoList()
        {
            if (File.Exists(FileName) == false)
            {
                FileStream FS = File.Create(FileName);
                FS.Close();
                StreamWriter SW = new StreamWriter(FileName);
                SW.Write("Josh;0;0");
                SW.Close();
            }
        }

        static public string[] ReadInfoList(string Name)
        {
            string line;
            using (StreamReader SR = new StreamReader(FileName, true))
            {
                SR.DiscardBufferedData();
                GenerateEmptyInfoList();
                while (SR.EndOfStream == false)
                {
                    line = SR.ReadLine();
                    string[] playerInfo = line.Split(';');
                    if (Name == playerInfo[0] /*|| Score == Int32.Parse(splitLine[1]) || Clicks == Int32.Parse(splitLine[2])*/)
                    {
                        return playerInfo;
                    }
                }
                SR.Close();
            }
            return null;
        }

        static public void WriteInfoList(string NewName, int NewScore, int NewClickCount)
        {
            string[] playerInfo = ReadInfoList(NewName);
            if (playerInfo == null)
            {
                using (StreamWriter SW = new StreamWriter(FileName, true))
                {
                    SW.WriteLine($"{NewName};{NewScore};{NewClickCount}");
                    SW.Close();
                }
            }
            else if (playerInfo != null)
            {
                string nameExist = playerInfo[0];
                int scoreExist = Int16.Parse(playerInfo[1]);
                int clickCountExist = Int16.Parse(playerInfo[2]);
                if (NewScore > scoreExist)
                {
                    OverwriteInfoList(nameExist, NewScore, clickCountExist);
                }
            }
        }

        static public void OverwriteInfoList(string NameExist, int newScore, int ClickCountExist)
        {
            for (int i = 0; i < AllLines.Length; i++)
            {
                string[] currentLine = AllLines[i].Split(';');
                if (currentLine[0] == NameExist)
                {
                    AllLines[i] = $"{NameExist};{newScore};{ClickCountExist}";
                    File.WriteAllLines(FileName, AllLines);
                }
            }
        }

        static public string[] BestPlayer()
        {
            GenerateEmptyInfoList();
            string line;
            string[] bestPlayer = {"Test", "0", "0"};
            int highscore = 0;

            using (StreamReader SR = new StreamReader(FileName, true))
            {
                while (SR.EndOfStream == false)
                {
                    line = SR.ReadLine();
                    string[] playerInfo = line.Split(';');
                    if (Int16.Parse(playerInfo[1]) > highscore)
                    {
                        highscore = Int16.Parse(playerInfo[1]);
                        bestPlayer = playerInfo;
                    }
                }
                SR.Close();
            }

            return bestPlayer;
        }

    }
}