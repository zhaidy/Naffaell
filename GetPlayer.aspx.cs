using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Forms;

public partial class GetPlayer : System.Web.UI.Page
{
    public IList<com_champ> _com_champ = new List<com_champ>(); //常用英雄 : name, count
    public IList<player_profile> _player_profile = new List<player_profile>(); //Player Profile : player_id, server, fighting
    public IList<normal_statistics> _normal_statistics = new List<normal_statistics>(); //normal statistics
    public IList<rank_statistics> _rank_statistics = new List<rank_statistics>(); //rank statistics
    public IList<match_list> _match_list = new List<match_list>(); //match_list
    string _playerId = "";
    string _server = "";
    string server = "";
    string playerId = "";
    string tier = "";
    string rank = "";
    string points = "";
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        _playerId = txtPlayerId.Text;
        _server = dpServer.SelectedValue;
        server = System.Web.HttpUtility.UrlEncode(dpServer.SelectedValue, System.Text.Encoding.UTF8);
        playerId = System.Web.HttpUtility.UrlEncode(txtPlayerId.Text, System.Text.Encoding.UTF8);

        sendRequest(server, playerId);
        parseHtml(server, playerId);
    }
    private void sendRequest(string server, string playerId)
    {
        WebRequest request = WebRequest.Create("http://lolbox.duowan.com/ajaxGetWarzone.php?playerName=" + playerId + "&serverName=" + server);
        request.Credentials = CredentialCache.DefaultCredentials;
        WebResponse response = request.GetResponse();
        Stream dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);
        string responseFromServer = reader.ReadToEnd();
        reader.Close();
        response.Close();
        tier = Regex.Match(Regex.Match(responseFromServer, @"tier"".*"",""rank").Value, @""".*""").Value.Replace(@"""", "").Replace(":", "").Replace(",", "");
        tier = Unicode2Chinese(tier);
        rank = tier + " " + Regex.Match(Regex.Match(responseFromServer, @"rank"".*"",").Value, @"""\w+""").Value.Replace(@"""", "");
        points = Regex.Match(Regex.Match(responseFromServer, @"league_points"".*,").Value, ":.*,").Value.Replace(":", "").Replace(",", "").Replace(@"""", "");
    }
    private string Unicode2Chinese(string strUnicode)
    {
        string[] splitString = new string[1];
        splitString[0] = "\\u";
        string[] unicodeArray = strUnicode.Split(splitString, StringSplitOptions.RemoveEmptyEntries);
        StringBuilder sb = new StringBuilder();

        foreach (string item in unicodeArray)
        {
            byte[] codes = new byte[2];
            int code1, code2;
            code1 = Convert.ToInt32(item.Substring(0, 2), 16);
            code2 = Convert.ToInt32(item.Substring(2), 16);
            codes[0] = (byte)code2;
            codes[1] = (byte)code1;
            sb.Append(Encoding.Unicode.GetString(codes));
        }

        return sb.ToString();
    }
    //public string GetWebpage(string url)
    //{
    //    _url = url;
    //    Thread thread = new Thread(new ThreadStart(GetWebPageWorker));
    //    thread.SetApartmentState(ApartmentState.STA);
    //    thread.Start();
    //    thread.Join();
    //    string s = html;
    //    return s;
    //}
    //protected void GetWebPageWorker()
    //{
    //    using (WebBrowser browser = new WebBrowser())
    //    {
    //        browser.ScrollBarsEnabled = false;
    //        browser.ScriptErrorsSuppressed = true;
    //        browser.Navigate(_url);

    //        // Wait for control to load page
    //        while (browser.ReadyState != WebBrowserReadyState.Complete)
    //        {
    //            System.Windows.Forms.Application.DoEvents();
    //        }

    //        html = browser.DocumentText;
    //    }
    //}

    private void parseHtml(string server, string playerId)
    {
        string url = "http://lolbox.duowan.com/playerDetail.php?serverName=" + server + "&playerName=" + playerId;
        string fighting = "";
        string level = "";
        string playerIcon = "";
        string first_win = "";

        //StringReader sr = new StringReader(GetWebpage(url));
        //HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
        var getHtmlWeb = new HtmlWeb();
        var htmlDocument = getHtmlWeb.Load(url);
        //htmlDocument.Load(url);
        HtmlNode bodyNode = htmlDocument.DocumentNode.SelectSingleNode("//body");

        //战斗力 : update_datetime, fighting
        IEnumerable<HtmlNode> fightingNodes = htmlDocument.DocumentNode.Descendants().Where(x => x.Name == "div" && x.Attributes.Contains("class")
            && x.Attributes["class"].Value.Split().Contains("fighting"));
        foreach (HtmlNode child in fightingNodes)
        {
            HtmlNode node = child.SelectSingleNode("//span");
            fighting = Regex.Match(child.InnerText, @"\d+").Value;
        }

        //get icon, level
        IEnumerable<HtmlNode> profileNodes = htmlDocument.DocumentNode.Descendants().Where(x => x.Name == "div" && x.Attributes.Contains("class")
            && x.Attributes["class"].Value.Split().Contains("avatar"));
        foreach (HtmlNode child in profileNodes)
        {
            if(!child.InnerHtml.Contains("yy"))
            {
                level = child.SelectSingleNode("//em").InnerText;
                playerIcon = Regex.Match(child.InnerHtml, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
            }
        }

        //get first win
        IEnumerable<HtmlNode> firstWinNodes = htmlDocument.DocumentNode.Descendants().Where(x => x.Name == "div" && x.Attributes.Contains("class")
            && x.Attributes["class"].Value.Split().Contains("act"));
        foreach (HtmlNode child in firstWinNodes)
        {
            first_win = child.InnerText.Replace("&nbsp;", "");
        }
        //add profile
        _player_profile.Add(new player_profile 
        {
            player_id = _playerId,
            server = _server,
            fighting = fighting,
            level = level,
            icon = playerIcon,
            first_win = first_win
        });
        
        //常用英雄 : name, count
        IEnumerable<HtmlNode> com_heroNodes = htmlDocument.DocumentNode.Descendants().Where(x => x.Name == "div" && x.Attributes.Contains("class")
            && x.Attributes["class"].Value.Split().Contains("com-hero"));
        foreach (HtmlNode child in com_heroNodes)
        {
            if (child.NodeType != HtmlNodeType.Text)
            {
                HtmlNodeCollection nodes = child.SelectNodes("//li");
                foreach (HtmlNode node in nodes)
                {
                    string _count = Regex.Match(Regex.Match(node.InnerHtml, @"\d次").Value, @"\d").Value;
                    if (node.Attributes.Contains("champion-name-ch"))
                    {
                        _com_champ.Add(new com_champ
                        {
                            champion_name_ch = node.Attributes["champion-name-ch"].Value,
                            champion_name = node.Attributes["champion-name"].Value,
                            icon = Regex.Match(node.InnerHtml, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value,
                            count = _count
                        });
                    }
                }
            }
        }

        //get normal statistics
        IEnumerable<HtmlNode> tableNodes = htmlDocument.DocumentNode.Descendants().Where(x => x.Name == "table");
        foreach (HtmlNode table in tableNodes)
        {
            foreach (HtmlNode row in table.SelectNodes("tr"))
            {
                HtmlNodeCollection cell = row.SelectNodes("td");
                if (cell != null)
                {
                    if (cell.Count == 6) //normal
                    {
                        _normal_statistics.Add(new normal_statistics
                        {
                            type = cell[0].InnerText,
                            total_matches = cell[1].InnerText,
                            win_rate = cell[2].InnerText,
                            matches_winned = cell[3].InnerText,
                            matches_lost = cell[4].InnerText,
                            update_datetime = cell[5].InnerText
                        });
                    }
                    if (cell.Count == 8) //current season
                    {
                        _rank_statistics.Add(new rank_statistics
                        {
                            type = cell[0].InnerText,
                            rank = rank,
                            point = points,
                            total_matches = cell[3].InnerText,
                            win_rate = cell[4].InnerText,
                            matches_winned = cell[5].InnerText,
                            matches_lost = cell[6].InnerText,
                            update_datetime = cell[7].InnerText
                        });
                    }
                    if (cell.Count == 7) //history
                    {
                        _rank_statistics.Add(new rank_statistics
                        {
                            type = cell[0].InnerText,
                            rank = "",
                            point = "",
                            total_matches = cell[3].InnerText,
                            win_rate = cell[4].InnerText,
                            matches_winned = cell[5].InnerText,
                            matches_lost = cell[6].InnerText,
                            update_datetime = ""
                        });
                    }
                }
            }
        }


        string matchListUrl = "http://lolbox.duowan.com/matchList.php?serverName=" + server + "&playerName=" + playerId;

        for (int i = 1; i <= 8; i++)
        {
            var getMatchHistoryWeb = new HtmlWeb();
            var doc = getMatchHistoryWeb.Load(matchListUrl + "&page=" + i.ToString());

            HtmlNode matchHistorybodyNode = doc.DocumentNode.SelectSingleNode("//body");

            //match_list
            IEnumerable<HtmlNode> historyNodes = doc.DocumentNode.Descendants().Where(x => x.Name == "div" && x.Attributes.Contains("class")
                && x.Attributes["class"].Value.Split().Contains("l-box"));
            foreach (HtmlNode child in historyNodes)
            {
                foreach (HtmlNode ul in child.SelectNodes("ul"))
                {
                    foreach (HtmlNode li in ul.SelectNodes("li"))
                    {
                        string id = li.Attributes["id"].Value.Substring(3);
                        HtmlNode championSpan = li.SelectSingleNode("span[@class='avatar']");
                        string icon = Regex.Match(championSpan.InnerHtml, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                        string champion_name_ch = championSpan.SelectSingleNode("img").Attributes["title"].Value;
                        string status = li.SelectSingleNode("p").SelectSingleNode("em").InnerText;
                        HtmlNode modeNode = li.SelectSingleNode("p[@class='info']").SelectSingleNode("span[@class='game']");
                        string mode = modeNode.InnerText;
                        string date = Regex.Replace(li.SelectSingleNode("p[@class='info']").LastChild.InnerText, @"\s", "").Replace("&nbsp;", "");
                        _match_list.Add(new match_list
                        {
                            id = id,
                            icon = icon,
                            champion_name_ch = champion_name_ch,
                            status = status,
                            mode = mode,
                            date = date
                        });
                    }
                }
            }
        }
        gvPlayerProfile.DataSource = _player_profile;
        gvPlayerProfile.DataBind();
        gvComChamp.DataSource = _com_champ;
        gvComChamp.DataBind();
        gvNormalStat.DataSource = _normal_statistics;
        gvNormalStat.DataBind();
        gvRankStat.DataSource = _rank_statistics;
        gvRankStat.DataBind();
        gvMatchList.DataSource = _match_list;
        gvMatchList.DataBind();
    }
    protected void gvMatchList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string id = gvMatchList.DataKeys[e.Row.RowIndex].Value.ToString();
            GridView gvDetail = e.Row.FindControl("gvDetail") as GridView;

            //15549326330
            string matchListDetailUrl = "http://lolbox.duowan.com/matchList/ajaxMatchDetail2.php?matchId=" + id + "&serverName=" + server + "&playerName=" + playerId;
            var getMatchHistoryDetailWeb = new HtmlWeb();
            var MatchDetaildoc = getMatchHistoryDetailWeb.Load(matchListDetailUrl);
            HtmlNodeCollection matchHistoryDetailbodyNode = MatchDetaildoc.DocumentNode.SelectNodes("//a[text()='" + _playerId + "']");

            //gvDetail.DataSource =
            //gvDetail.DataBind();
        }
    }
    public class player_profile
    {
        public string player_id { get; set; }
        public string server { get; set; }
        public string icon { get; set; }
        public string level { get; set; }
        public string fighting { get; set; }
        public string first_win { get; set; }
    }
    public class normal_statistics
    {
        public string type { get; set; }
        public string total_matches { get; set; }
        public string win_rate { get; set; }
        public string matches_winned { get; set; }
        public string matches_lost { get; set; }
        public string update_datetime { get; set; }
    }
    public class rank_statistics
    {
        public string type { get; set; }
        public string rank { get; set; }
        public string point { get; set; }
        public string total_matches { get; set; }
        public string win_rate { get; set; }
        public string matches_winned { get; set; }
        public string matches_lost { get; set; }
        public string update_datetime { get; set; }
    }
    public class com_champ
    {
        public string champion_name_ch { get; set; }
        public string champion_name { get; set; }
        public string icon { get; set; }
        public string count { get; set; }
    }
    public class match_list
    {
        public string id { get; set; }
        public string champion_name_ch { get; set; }
        public string icon { get; set; }
        public string status { get; set; }
        public string mode { get; set; }
        public string date { get; set; }
    }
}