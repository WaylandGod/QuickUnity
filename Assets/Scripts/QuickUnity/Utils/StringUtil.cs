using UnityEngine;
using System.Collections;
using System.Text;

namespace QuickUnity.Utils
{
		public class StringUtil
		{		
				public static string ConvertANSIToUTF8 (byte[] bytes)
				{
						string str = Encoding.Default.GetString (bytes);
						bytes = Encoding.Default.GetBytes (str);
						bytes = Encoding.Convert (Encoding.Default, Encoding.UTF8, bytes);
						str = Encoding.UTF8.GetString (bytes);
						return str;
				}
		}
}
