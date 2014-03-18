using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using QuickUnity.Utils;

public class CSVParseDemo : MonoBehaviour
{
		public static string CSV_FILE = "metadata/csv_test";
		
		// Use this for initialization
		void Start ()
		{
				print ("pasrsing csv file: " + CSV_FILE);
				TextAsset asset = Resources.Load (CSV_FILE, typeof(TextAsset)) as TextAsset;
				string str = StringUtil.ConvertANSIToUTF8 (asset.bytes);
                print(str);
		}
	
		// Update is called once per frame
		void Update ()
		{
		
		}
}
