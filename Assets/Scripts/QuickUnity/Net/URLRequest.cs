using UnityEngine;
using System.Collections;

namespace QuickUnity.Net
{
		public class URLRequest
		{
				protected string strURL = null;

				public string URL {
						get {
								return strURL;
						}

						set {
								strURL = value;
						}
				}

				protected string strMethod = URLRequestMethod.GET;

				public string Method {
						get {
								return strMethod;
						}

						set {
								strMethod = value;
						}
				}
        
				public URLRequest (string URL = null)
				{
						this.URL = URL;
				}
		}
}