using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentPathPlanning
{
    class AStarExport
    {
        private static string EXPORT_DIRECTORY = @"AStar";

        public static void Save(Cell[,] cells)
        {
            // Verify the directory exists
            if (!Directory.Exists(EXPORT_DIRECTORY))
            {
                Directory.CreateDirectory(EXPORT_DIRECTORY);
            }

            // Save a CSV file for f, g, and h
            using (StreamWriter file = new StreamWriter(EXPORT_DIRECTORY + @"\f.csv"))
            {
                for (int rowIndex = 0; rowIndex < cells.GetLength(0); rowIndex++)
                {
                    for (int columnIndex = 0; columnIndex < cells.GetLength(1); columnIndex++)
                    {
                        file.Write(cells[rowIndex, columnIndex].GetFScore());

                        if (columnIndex == cells.GetLength(1) - 1)
                        {
                            file.WriteLine();
                        }
                        else
                        {
                            file.Write(",");
                        }
                    }
                }
            }

            using (StreamWriter file = new StreamWriter(EXPORT_DIRECTORY + @"\g.csv"))
            {
                for (int rowIndex = 0; rowIndex < cells.GetLength(0); rowIndex++)
                {
                    for (int columnIndex = 0; columnIndex < cells.GetLength(1); columnIndex++)
                    {
                        file.Write(cells[rowIndex, columnIndex].GetGScore());

                        if (columnIndex == cells.GetLength(1) - 1)
                        {
                            file.WriteLine();
                        }
                        else
                        {
                            file.Write(",");
                        }
                    }
                }
            }

            using (StreamWriter file = new StreamWriter(EXPORT_DIRECTORY + @"\h.csv"))
            {
                for (int rowIndex = 0; rowIndex < cells.GetLength(0); rowIndex++)
                {
                    for (int columnIndex = 0; columnIndex < cells.GetLength(1); columnIndex++)
                    {
                        file.Write(cells[rowIndex, columnIndex].GetHScore());

                        if (columnIndex == cells.GetLength(1) - 1)
                        {
                            file.WriteLine();
                        }
                        else
                        {
                            file.Write(",");
                        }
                    }
                }
            }
        }
    }
}
