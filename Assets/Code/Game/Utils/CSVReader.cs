using UnityEngine;
using System.Collections;
using System.Linq;

namespace RO
{
	public class CSVReader
	{
		static public void DebugOutputGrid (string[,] grid)
		{
			string textOutput = ""; 
			for (int y = 0; y < grid.GetUpperBound(1); y++) {	
				for (int x = 0; x < grid.GetUpperBound(0); x++) {
					
					textOutput += grid [x, y]; 
					textOutput += "|"; 
				}
				textOutput += "\n"; 
			}
			RO.LoggerUnused.Log (textOutput);
		}
		
		// splits a CSV file into a 2D string array
		static public string[,] SplitCsvGrid (string csvText)
		{
			csvText = csvText.Replace ("\r", "");
			string[] row = csvText.Replace ("\n", "@").Split ('@'); 
			// finds the max width of row
			int width = 0; 
			for (int i = 0; i < row.Length -1; i++) {
				string[] col = SplitCsvLine (row [i]); 
				width = Mathf.Max (width, col.Length); 
			}			
			// creates new 2D string grid to output to
			string[,] outputGrid = new string[row.Length - 1, width]; 
			for (int y = 0; y < row.Length-1; y++) {
				string[] col = SplitCsvLine (row [y]); 
				for (int x = 0; x < col.Length; x++) {
					outputGrid [y, x] = col [x]; 					
					// This line was to replace "" with " in my output. 
					// Include or edit it as you wish.
					outputGrid [y, x] = outputGrid [y, x].Replace ("\\", "/");
				}
			}
			
			return outputGrid; 
		}
		
		// splits a CSV row 
		static public string[] SplitCsvLine (string line)
		{

			return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches (line,
			                                                                                                    @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)", 
			                                                                                                    System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
			        select m.Groups [1].Value).ToArray ();
		}
	
	}
} // namespace RO
