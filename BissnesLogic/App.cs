using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Drawing;

namespace BissnesLogic
{
    public class App
    {
        public List<Classes.AnimeList> ConvertAnimeList(string content)
        {
            List<Classes.AnimeList> AnimList = new List<Classes.AnimeList>();
            //  content = HttpUtility.HtmlDecode(content);
            JToken jtoken = JToken.Parse(content);
            if (Convert.ToUInt32(jtoken["error"]) == 0)
            {
                AnimList = jtoken["anime"].Children().Select(c => c.ToObject<Classes.AnimeList>()).ToList();

                return AnimList;
            }
            else
            {
                return null;
            }
        }
        public List<Classes.AnimeView> ConvertAnimeView(string content)
        {
            Classes.AnimeView AnimList = new Classes.AnimeView();
            List<Classes.AnimeView> temp = new List<Classes.AnimeView>();
            try {
                JToken jtoken = JToken.Parse(content);
                if (Convert.ToUInt32(jtoken["error"]) == 0)
                {
                    AnimList = jtoken["anime"].ToObject<Classes.AnimeView>();
                    temp.Add(AnimList);
                    return temp;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        public List<Classes.Raspisanie> ConvertAnimeRaspisanie(string content)
        {
            List<Classes.Raspisanie> AnimList = new List<Classes.Raspisanie>();

            JToken jtoken = JToken.Parse(content);
            if (Convert.ToUInt32(jtoken["error"]) == 0)
            {
                AnimList = jtoken["raspisanie"].Children().Select(c => c.ToObject<Classes.Raspisanie>()).ToList();

                return AnimList;
            }
            else
            {
                return null;
            }
        }

        public string Request(string url)
        {
            string content = null;
            
               HttpWebRequest req;
               HttpWebResponse resp;
               StreamReader sr;
            try {
                req = (HttpWebRequest)HttpWebRequest.Create(string.Format(url));
                resp = (HttpWebResponse)req.GetResponse();
                sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                content = sr.ReadToEnd();
                sr.Close();
                resp.Close();
                return content;
            }
            catch
            {
                return null;
            }
        }
        



    }
}
