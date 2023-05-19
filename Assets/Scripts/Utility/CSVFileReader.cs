using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Data;
using System;

namespace ARDataVisualization.Utility
{
    /// <summary>
    /// A static class for extracting data from a csv file.
    /// </summary>
    public static class CSVFileReader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">csv file path</param>
        /// <returns></returns>
        public static DataTable ReadCSVFile(string path)
        {
            DataTable data = new()
            {
                TableName = Path.GetFileNameWithoutExtension(path)
            };

            try
            {
                //Creates a StreamReader with the file's path.
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    int lineNumber = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        //Split the line for an array of each value.
                        string[] values = line.Split(',');
                        if (lineNumber == 0)
                        {
                            foreach (string value in values)
                            {
                                //Create columns for each value
                                data.Columns.Add(new DataColumn(value));
                            }
                        }
                        else //After line "zero", each line is a row in our readData.
                        {
                            int valNumber = 0;
                            DataRow row = data.NewRow();
                            foreach (string value in values)
                            {
                                row[valNumber] = value;
                                valNumber++;
                            }
                            data.Rows.Add(row);
                        }
                        lineNumber++;
                    }

                }
            }
            catch (Exception e)
            {
                Debug.Log("The file could not be read:");
                Debug.Log(e.Message);
            }

            return data;
        }
    }
}
