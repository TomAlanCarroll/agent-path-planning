using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentPathPlanning
{
    class QTableExport
    {
        private static string EXPORT_DIRECTORY = @"QTable";

        public static void Save(double[, ,] qTable)
        {
            // Verify the directory exists
            if (!Directory.Exists(EXPORT_DIRECTORY))
            {
                Directory.CreateDirectory(EXPORT_DIRECTORY);
            }

            // Save a CSV file for each direction
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (direction != Direction.NONE)
                {
                    using (StreamWriter file = new StreamWriter(EXPORT_DIRECTORY + @"\" + direction.ToString() + ".csv"))
                    {
                        for (int rowIndex = 0; rowIndex < qTable.GetLength(0); rowIndex++)
                        {
                            for (int columnIndex = 0; columnIndex < qTable.GetLength(1); columnIndex++)
                            {
                                file.Write(qTable[rowIndex, columnIndex, (int)direction]);

                                if (columnIndex == qTable.GetLength(1) - 1)
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
    }
}
