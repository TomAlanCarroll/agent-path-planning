using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AgentPathPlanning
{
    class GridMapParser
    {
        public static Cell[,] Parse(string fileName)
        {
            Cell[,] cells = null;

            try
            {
                int rowCounter = 0;
                int previousLineCellCount = 0;
                // Change the extension to CSV and read the lines
                string[] lines = File.ReadAllLines(System.IO.Path.ChangeExtension(fileName, ".csv"));

                cells = new Cell[lines.GetLength(0), lines.GetLength(0)];

                foreach (string line in lines)
                {
                    string[] splitLine = line.Split(',');

                    for (int i = 0; i < splitLine.Length; i++)
                    {
                        bool isObstacle = false;
                        bool isAgentStartingCell = false;
                        bool isRewardCell = false;

                        if (splitLine[i].Contains(((int)GridMapCodes.OBSTACLE).ToString()))
                        {
                            isObstacle = true;
                        }
                        else if (splitLine[i].Contains(((int)GridMapCodes.AGENT).ToString()))
                        {
                            isAgentStartingCell = true;
                        }
                        else if (splitLine[i].Contains(((int)GridMapCodes.REWARD).ToString()))
                        {
                            isRewardCell = true;
                        }

                        cells[rowCounter, i] = new Cell(rowCounter, i, isObstacle, isAgentStartingCell, isRewardCell);
                    }

                    if (rowCounter > 0 && splitLine.Length != previousLineCellCount && rowCounter <= previousLineCellCount)
                    {
                        throw new FormatException("Error: Uneven row/column size(s) were found in the CSV file. Please correct and try again.");
                    }
                    else
                    {
                        rowCounter++;
                        previousLineCellCount = splitLine.Length;
                    }
                }
            }
            catch (FormatException e)
            {
                MessageBox.Show(e.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to parse the grid map. Please verify the file is correct and accessible.");
            }

            return cells;
        }
    }
}
