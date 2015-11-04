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
    public IList<playerMatchDetails> _playerMatchDetails = new List<playerMatchDetails>();
    public IList<matchDetails> _matchDetailsA = new List<matchDetails>();
    public IList<honour> _honourA = new List<honour>();
    public IList<items> _itemA = new List<items>();
    public IList<matchDetails> _matchDetailsB = new List<matchDetails>();
    public IList<honour> _honourB = new List<honour>();
    public IList<items> _itemB = new List<items>();
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
            if (!child.InnerHtml.Contains("yy"))
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

        //new comChamp
        string comHeroUrl = "http://lolbox.duowan.com/new/api/index.php?_do=personal/championslist&serverName=" + server + "&playerName=" + playerId;
        string json = "";
        using (WebClient wc = new WebClient())
        {
            json = wc.DownloadString(comHeroUrl);
        }
        IList<played_champs_display> _played_champs_display = new List<played_champs_display>();
        played_champs playedChamps = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<played_champs>(json);

        for (int i = 0; i < playedChamps.content.Count; i++)
        {
            string averageKDAString = string.Join("/", playedChamps.content[i].averageKDA);
            string averageDamageString = playedChamps.content[i].averageDamage.First();
            string averageEarnString = playedChamps.content[i].averageEarn.First();
            _played_champs_display.Add(new played_champs_display
            {
                icon = "http://img.lolbox.duowan.com/champions/" + playedChamps.content[i].championName + "_40x40.jpg",
                championName = playedChamps.content[i].championName,
                championNameCN = playedChamps.content[i].championNameCN,
                winRate = playedChamps.content[i].winRate,
                matchStat = playedChamps.content[i].matchStat,
                averageKDA = averageKDAString,
                averageKDARating = playedChamps.content[i].averageKDARating,
                averageDamage = averageDamageString,
                averageEarn = averageEarnString,
                averageMinionsKilled = playedChamps.content[i].averageMinionsKilled,
                totalMVP = playedChamps.content[i].totalMVP,
                totalHope = playedChamps.content[i].totalHope
            });
        }

        //get match list
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
                            playerId = _playerId,
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
        gvPlayedChamps.DataSource = _played_champs_display;
        gvPlayedChamps.DataBind();
        gvComChamp.DataSource = _com_champ;
        gvComChamp.DataBind();
        gvNormalStat.DataSource = _normal_statistics;
        gvNormalStat.DataBind();
        gvRankStat.DataSource = _rank_statistics;
        gvRankStat.DataBind();
        gvMatchList.DataSource = _match_list;
        gvMatchList.DataBind();
    }
    protected void btnMatchDetail_Click(object sender, EventArgs e)
    {
        //match detail
        System.Web.UI.WebControls.Button btn = sender as System.Web.UI.WebControls.Button;
        GridViewRow trow = btn.NamingContainer as GridViewRow;
        string id = gvMatchList.DataKeys[trow.RowIndex].Values[0].ToString();

        string matchListDetailUrl = "http://lolbox.duowan.com/matchList/ajaxMatchDetail2.php?matchId=" + id + "&serverName=" + server + "&playerName=" + playerId;
        var getMatchHistoryDetailWeb = new HtmlWeb();
        var MatchDetaildoc = getMatchHistoryDetailWeb.Load(matchListDetailUrl);
        IEnumerable<HtmlNode> headerNodes = MatchDetaildoc.DocumentNode.Descendants().Where(x => x.Name == "div" && x.Attributes.Contains("class")
            && x.Attributes["class"].Value.Split().Contains("r-top"));
        IList<matchHeader> _matchHeader = new List<matchHeader>(); //match_Header
        foreach (HtmlNode child in headerNodes)
        {
            string mode = "";
            string duration = "";
            string endTime = "";
            string kills = "";
            string gold = "";
            foreach (HtmlNode spanNode in child.SelectNodes("span"))
            {
                if (spanNode.InnerText.Contains("类型"))
                {
                    mode = spanNode.InnerText.Replace("类型：", "");
                }
                if (spanNode.InnerText.Contains("时长"))
                {
                    duration = spanNode.InnerText.Replace("时长：", "");
                }
                if (spanNode.InnerText.Contains("结束"))
                {
                    endTime = spanNode.InnerText.Replace("结束：", "");
                }
                if (spanNode.InnerText.Contains("人头"))
                {
                    kills = spanNode.InnerText.Replace("人头：", "");
                }
                if (spanNode.InnerText.Contains("金钱"))
                {
                    gold = spanNode.InnerText.Replace("金钱：", "");
                }
            }
            _matchHeader.Add(new matchHeader
            {
                mode = mode,
                duration = duration,
                endTime = endTime,
                kills = kills,
                gold = gold
            });
        }

        IEnumerable<HtmlNode> tableADivNodes = MatchDetaildoc.DocumentNode.Descendants().Where(x => x.Name == "div" && x.Attributes.Contains("id")
            && x.Attributes["id"].Value.Split().Contains("zj-table--A"));
        foreach (HtmlNode child in tableADivNodes)
        {
            IEnumerable<HtmlNode> tableNodes = child.Descendants().Where(x => x.Name == "table");
            foreach (HtmlNode table in tableNodes)
            {
                foreach (HtmlNode row in table.SelectNodes("tr"))
                {
                    string player = "";
                    string honour = "";
                    string gold = "";
                    string KDA = "";
                    string itemIcon1 = "";
                    string itemName1 = "";
                    string itemDescription1 = "";
                    string itemIcon2 = "";
                    string itemName2 = "";
                    string itemDescription2 = "";
                    string itemIcon3 = "";
                    string itemName3 = "";
                    string itemDescription3 = "";
                    string itemIcon4 = "";
                    string itemName4 = "";
                    string itemDescription4 = "";
                    string itemIcon5 = "";
                    string itemName5 = "";
                    string itemDescription5 = "";
                    string itemIcon6 = "";
                    string itemName6 = "";
                    string itemDescription6 = "";
                    HtmlNode teamNameDivNode = row.SelectSingleNode("td[@class='col1']").SelectSingleNode("div");
                    HtmlNode playerNode = teamNameDivNode.SelectSingleNode("span[@class='avatar']").SelectSingleNode("img");
                    player = playerNode.Attributes["data-playername"].Value;
                    HtmlNodeCollection honourNodes = teamNameDivNode.SelectNodes("em");
                    if (honourNodes != null)
                    {
                        foreach (HtmlNode em in honourNodes)
                        {
                            honour = em.Attributes["title"].Value;
                            _honourA.Add(new honour
                            {
                                playerId = player,
                                matchId = id,
                                honourDesc = honour
                            });
                        }
                    }
                    HtmlNode goldNode = row.SelectSingleNode("td[@class='col2']");
                    gold = goldNode.InnerText;
                    HtmlNode KDANode = row.SelectSingleNode("td[@class='col3']");
                    KDA = KDANode.InnerText;
                    HtmlNode itemNode = row.SelectSingleNode("td[@class='col4']").SelectSingleNode("div[@class='u-weapon']").SelectSingleNode("ul[@class='chuzhuang']");
                    HtmlNodeCollection itemNodes = itemNode.SelectNodes("li");
                    foreach (HtmlNode node in itemNodes)
                    {
                        if (itemNodes.Count > 0)
                        {
                            itemIcon1 = Regex.Match(itemNodes[0].InnerHtml, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                            itemName1 = Regex.Match(itemNodes[0].SelectSingleNode("img").Attributes["title"].Value, @".+\s").Value.Replace(" ", "");
                            itemDescription1 = itemNodes[0].SelectSingleNode("img").Attributes["title"].Value;
                        }
                        if (itemNodes.Count > 1)
                        {
                            itemIcon2 = Regex.Match(itemNodes[1].InnerHtml, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                            itemName2 = Regex.Match(itemNodes[1].SelectSingleNode("img").Attributes["title"].Value, @".+\s").Value.Replace(" ", "");
                            itemDescription2 = itemNodes[1].SelectSingleNode("img").Attributes["title"].Value;
                        }
                        if (itemNodes.Count > 2)
                        {
                            itemIcon3 = Regex.Match(itemNodes[2].InnerHtml, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                            itemName3 = Regex.Match(itemNodes[2].SelectSingleNode("img").Attributes["title"].Value, @".+\s").Value.Replace(" ", "");
                            itemDescription3 = itemNodes[2].SelectSingleNode("img").Attributes["title"].Value;
                        }
                        if (itemNodes.Count > 3)
                        {
                            itemIcon4 = Regex.Match(itemNodes[3].InnerHtml, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                            itemName4 = Regex.Match(itemNodes[3].SelectSingleNode("img").Attributes["title"].Value, @".+\s").Value.Replace(" ", "");
                            itemDescription4 = itemNodes[3].SelectSingleNode("img").Attributes["title"].Value;
                        }
                        if (itemNodes.Count > 4)
                        {
                            itemIcon5 = Regex.Match(itemNodes[4].InnerHtml, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                            itemName5 = Regex.Match(itemNodes[4].SelectSingleNode("img").Attributes["title"].Value, @".+\s").Value.Replace(" ", "");
                            itemDescription5 = itemNodes[4].SelectSingleNode("img").Attributes["title"].Value;
                        }
                        if (itemNodes.Count > 5)
                        {
                            itemIcon6 = Regex.Match(itemNodes[5].InnerHtml, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                            itemName6 = Regex.Match(itemNodes[5].SelectSingleNode("img").Attributes["title"].Value, @".+\s").Value.Replace(" ", "");
                            itemDescription6 = itemNodes[5].SelectSingleNode("img").Attributes["title"].Value;
                        }
                    }
                    _matchDetailsA.Add(new matchDetails
                    {
                        playerId = player,
                        matchId = id,
                        gold = gold,
                        KDA = KDA

                    });
                    _itemA.Add(new items
                    {
                        playerId = player,
                        matchId = id,
                        itemIcon1 = itemIcon1,
                        itemName1 = itemName1,
                        itemDesc1 = itemDescription1,
                        itemIcon2 = itemIcon2,
                        itemName2 = itemName2,
                        itemDesc2 = itemDescription2,
                        itemIcon3 = itemIcon3,
                        itemName3 = itemName3,
                        itemDesc3 = itemDescription3,
                        itemIcon4 = itemIcon4,
                        itemName4 = itemName4,
                        itemDesc4 = itemDescription4,
                        itemIcon5 = itemIcon5,
                        itemName5 = itemName5,
                        itemDesc5 = itemDescription5,
                        itemIcon6 = itemIcon6,
                        itemName6 = itemName6,
                        itemDesc6 = itemDescription6
                    });
                }
            }
        }

        IEnumerable<HtmlNode> tableBDivNodes = MatchDetaildoc.DocumentNode.Descendants().Where(x => x.Name == "div" && x.Attributes.Contains("id")
            && x.Attributes["id"].Value.Split().Contains("zj-table--B"));
        foreach (HtmlNode child in tableBDivNodes)
        {
            IEnumerable<HtmlNode> tableNodes = child.Descendants().Where(x => x.Name == "table");
            foreach (HtmlNode table in tableNodes)
            {
                foreach (HtmlNode row in table.SelectNodes("tr"))
                {
                    string player = "";
                    string honour = "";
                    string gold = "";
                    string KDA = "";
                    string itemIcon1 = "";
                    string itemName1 = "";
                    string itemDescription1 = "";
                    string itemIcon2 = "";
                    string itemName2 = "";
                    string itemDescription2 = "";
                    string itemIcon3 = "";
                    string itemName3 = "";
                    string itemDescription3 = "";
                    string itemIcon4 = "";
                    string itemName4 = "";
                    string itemDescription4 = "";
                    string itemIcon5 = "";
                    string itemName5 = "";
                    string itemDescription5 = "";
                    string itemIcon6 = "";
                    string itemName6 = "";
                    string itemDescription6 = "";
                    HtmlNode teamNameDivNode = row.SelectSingleNode("td[@class='col1']").SelectSingleNode("div");
                    HtmlNode playerNode = teamNameDivNode.SelectSingleNode("span[@class='avatar']").SelectSingleNode("img");
                    player = playerNode.Attributes["data-playername"].Value;
                    HtmlNodeCollection honourNodes = teamNameDivNode.SelectNodes("em");
                    if (honourNodes != null)
                    {
                        foreach (HtmlNode em in honourNodes)
                        {
                            honour = em.Attributes["title"].Value;
                            _honourB.Add(new honour
                            {
                                playerId = player,
                                matchId = id,
                                honourDesc = honour
                            });
                        }
                    }
                    HtmlNode goldNode = row.SelectSingleNode("td[@class='col2']");
                    gold = goldNode.InnerText;
                    HtmlNode KDANode = row.SelectSingleNode("td[@class='col3']");
                    KDA = KDANode.InnerText;
                    HtmlNode itemNode = row.SelectSingleNode("td[@class='col4']").SelectSingleNode("div[@class='u-weapon']").SelectSingleNode("ul[@class='chuzhuang']");
                    HtmlNodeCollection itemNodes = itemNode.SelectNodes("li");
                    foreach (HtmlNode node in itemNodes)
                    {
                        if (itemNodes.Count > 0)
                        {
                            itemIcon1 = Regex.Match(itemNodes[0].InnerHtml, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                            itemName1 = Regex.Match(itemNodes[0].SelectSingleNode("img").Attributes["title"].Value, @".+\s").Value.Replace(" ", "");
                            itemDescription1 = itemNodes[0].SelectSingleNode("img").Attributes["title"].Value;
                        }
                        if (itemNodes.Count > 1)
                        {
                            itemIcon2 = Regex.Match(itemNodes[1].InnerHtml, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                            itemName2 = Regex.Match(itemNodes[1].SelectSingleNode("img").Attributes["title"].Value, @".+\s").Value.Replace(" ", "");
                            itemDescription2 = itemNodes[1].SelectSingleNode("img").Attributes["title"].Value;
                        }
                        if (itemNodes.Count > 2)
                        {
                            itemIcon3 = Regex.Match(itemNodes[2].InnerHtml, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                            itemName3 = Regex.Match(itemNodes[2].SelectSingleNode("img").Attributes["title"].Value, @".+\s").Value.Replace(" ", "");
                            itemDescription3 = itemNodes[2].SelectSingleNode("img").Attributes["title"].Value;
                        }
                        if (itemNodes.Count > 3)
                        {
                            itemIcon4 = Regex.Match(itemNodes[3].InnerHtml, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                            itemName4 = Regex.Match(itemNodes[3].SelectSingleNode("img").Attributes["title"].Value, @".+\s").Value.Replace(" ", "");
                            itemDescription4 = itemNodes[3].SelectSingleNode("img").Attributes["title"].Value;
                        }
                        if (itemNodes.Count > 4)
                        {
                            itemIcon5 = Regex.Match(itemNodes[4].InnerHtml, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                            itemName5 = Regex.Match(itemNodes[4].SelectSingleNode("img").Attributes["title"].Value, @".+\s").Value.Replace(" ", "");
                            itemDescription5 = itemNodes[4].SelectSingleNode("img").Attributes["title"].Value;
                        }
                        if (itemNodes.Count > 5)
                        {
                            itemIcon6 = Regex.Match(itemNodes[5].InnerHtml, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                            itemName6 = Regex.Match(itemNodes[5].SelectSingleNode("img").Attributes["title"].Value, @".+\s").Value.Replace(" ", "");
                            itemDescription6 = itemNodes[5].SelectSingleNode("img").Attributes["title"].Value;
                        }
                    }
                    _matchDetailsB.Add(new matchDetails
                    {
                        playerId = player,
                        matchId = id,
                        gold = gold,
                        KDA = KDA

                    });
                    _itemB.Add(new items
                    {
                        playerId = player,
                        matchId = id,
                        itemIcon1 = itemIcon1,
                        itemName1 = itemName1,
                        itemDesc1 = itemDescription1,
                        itemIcon2 = itemIcon2,
                        itemName2 = itemName2,
                        itemDesc2 = itemDescription2,
                        itemIcon3 = itemIcon3,
                        itemName3 = itemName3,
                        itemDesc3 = itemDescription3,
                        itemIcon4 = itemIcon4,
                        itemName4 = itemName4,
                        itemDesc4 = itemDescription4,
                        itemIcon5 = itemIcon5,
                        itemName5 = itemName5,
                        itemDesc5 = itemDescription5,
                        itemIcon6 = itemIcon6,
                        itemName6 = itemName6,
                        itemDesc6 = itemDescription6
                    });
                }
            }
        }

        IEnumerable<HtmlNode> playerTipsNodes = MatchDetaildoc.DocumentNode.Descendants().Where(x => x.Name == "div" && x.Attributes.Contains("class")
            && x.Attributes["class"].Value.Split().Contains("layer"));
        foreach (HtmlNode child in playerTipsNodes)
        {
            string warScore = "";
            string lastHits = "";
            string creeps = "";
            string towersDestroyed = "";
            string barracksDestroyed = "";
            string wards = "";
            string dewards = "";
            string maxContKills = "";
            string maxMultiKills = "";
            string maxCrit = "";
            string totalHeal = "";
            string totalDmg = "";
            string totalTank = "";
            string totalHeroDmg = "";
            string totalHeroPhyDmg = "";
            string totalHeroMagicDmg = "";
            string totalHeroTrueDmg = "";
            HtmlNode divModTipsMain = child.SelectSingleNode("div[@class='mod-tips-main']");
            HtmlNode divTopLeft = child.SelectSingleNode("div[@class='mod-tips-top']").SelectSingleNode("div[@class='tip-topleft']");
            MatchCollection matches = Regex.Matches(divTopLeft.InnerHtml, "<img.+?src=[\"'](.+?)spell(.+?)[\"'].*?>", RegexOptions.IgnoreCase);
            string champIcon = Regex.Match(divTopLeft.InnerHtml, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
            string firstSpellIcon = Regex.Match(matches[0].Value, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
            string secondSpellIcon = Regex.Match(matches[1].Value, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
            IEnumerable<HtmlNode> tableNodes = divModTipsMain.Descendants().Where(x => x.Name == "table");
            foreach (HtmlNode table in tableNodes)
            {
                foreach (HtmlNode row in table.SelectNodes("tr"))
                {
                    HtmlNodeCollection cell = row.SelectNodes("td");
                    if (cell != null)
                    {
                        if (cell.Count >= 2 && cell[0].InnerText == "战局评分:")
                        {
                            warScore = cell[1].InnerText.Replace(" ", "").Replace("\r\n", "");
                        }
                        if (cell.Count >= 4 && cell[0].InnerText == "补兵:")
                        {
                            lastHits = cell[1].InnerText.Replace(" ", "").Replace("\r\n", "");
                        }
                        if (cell.Count >= 4 && cell[2].InnerText == "野怪:")
                        {
                            creeps = cell[3].InnerText.Replace(" ", "").Replace("\r\n", "");
                        }
                        if (cell.Count >= 4 && cell[0].InnerText == "推塔:")
                        {
                            towersDestroyed = cell[1].InnerText.Replace(" ", "").Replace("\r\n", "");
                        }
                        if (cell.Count >= 4 && cell[2].InnerText == "兵营:")
                        {
                            barracksDestroyed = cell[3].InnerText.Replace(" ", "").Replace("\r\n", "");
                        }
                        if (cell.Count >= 4 && cell[0].InnerText == "放眼数:")
                        {
                            wards = cell[1].InnerText.Replace(" ", "").Replace("\r\n", "");
                        }
                        if (cell.Count >= 4 && cell[2].InnerText == "排眼数:")
                        {
                            dewards = cell[3].InnerText.Replace(" ", "").Replace("\r\n", "");
                        }
                        if (cell.Count >= 4 && cell[0].InnerText == "最大连杀:")
                        {
                            maxContKills = cell[1].InnerText.Replace(" ", "").Replace("\r\n", "");
                        }
                        if (cell.Count >= 4 && cell[2].InnerText == "最大多杀:")
                        {
                            maxMultiKills = cell[3].InnerText.Replace(" ", "").Replace("\r\n", "");
                        }
                        if (cell.Count >= 4 && cell[0].InnerText == "最大暴击:")
                        {
                            maxCrit = cell[1].InnerText.Replace(" ", "").Replace("\r\n", "");
                        }
                        if (cell.Count >= 4 && cell[2].InnerText == "总治疗:")
                        {
                            totalHeal = cell[3].InnerText.Replace(" ", "").Replace("\r\n", "");
                        }
                        if (cell.Count >= 4 && cell[0].InnerText == "输出伤害:")
                        {
                            totalDmg = cell[1].InnerText.Replace(" ", "").Replace("\r\n", "");
                        }
                        if (cell.Count >= 4 && cell[2].InnerText == "承受敌害:")
                        {
                            totalTank = cell[3].InnerText.Replace(" ", "").Replace("\r\n", "");
                        }
                    }
                }
            }
            HtmlNodeCollection pNodes = divModTipsMain.SelectNodes("p");
            foreach (HtmlNode node in pNodes)
            {
                if (node.InnerHtml.Contains("给对方英雄造成总伤害:"))
                {
                    totalHeroDmg = node.InnerText.Replace("给对方英雄造成总伤害:", "").Replace(" ", "").Replace("\r\n", "");
                }
                if (node.InnerHtml.Contains("给对方英雄的物理伤害:"))
                {
                    totalHeroPhyDmg = node.InnerText.Replace("给对方英雄的物理伤害:", "").Replace(" ", "").Replace("\r\n", "");
                }
                if (node.InnerHtml.Contains("给对方英雄的魔法伤害:"))
                {
                    totalHeroMagicDmg = node.InnerText.Replace("给对方英雄的魔法伤害:", "").Replace(" ", "").Replace("\r\n", "");
                }
                if (node.InnerHtml.Contains("给对方英雄的真实伤害:"))
                {
                    totalHeroTrueDmg = node.InnerText.Replace("给对方英雄的真实伤害:", "").Replace(" ", "").Replace("\r\n", "");
                }
            }
            string test = divTopLeft.SelectSingleNode("p[@class='tip-user-name']").InnerText.Replace(" ", "").Replace("\r\n", "");
            _playerMatchDetails.Add(new playerMatchDetails
            {
                matchId = id,
                playerId = divTopLeft.SelectSingleNode("p[@class='tip-user-name']").InnerText.Replace(" ", "").Replace("\r\n", ""),
                champion_name_ch = divTopLeft.SelectSingleNode("div[@class='tip-user-detail']").SelectSingleNode("span[@class='tip-tip-user-name2']").InnerText,
                champIcon = champIcon,
                firstSpellIcon = firstSpellIcon,
                secondSpellIcon = secondSpellIcon,
                warScore = warScore,
                lastHits = lastHits,
                creeps = creeps,
                towersDestroyed = towersDestroyed,
                barracksDestroyed = barracksDestroyed,
                wards = wards,
                dewards = dewards,
                maxContKills = maxContKills,
                maxMultiKills = maxMultiKills,
                maxCrit = maxCrit,
                totalHeal = totalHeal,
                totalDmg = totalDmg,
                totalTank = totalTank,
                totalHeroDmg = totalHeroDmg,
                totalHeroPhyDmg = totalHeroPhyDmg,
                totalHeroMagicDmg = totalHeroMagicDmg,
                totalHeroTrueDmg = totalHeroTrueDmg
            });
        }

        var _joinedMatchDetailsA = from ma in _matchDetailsA
                                   join pmd in _playerMatchDetails
                                   on new { mId = ma.matchId, pId = ma.playerId } equals new { mId = pmd.matchId, pId = pmd.playerId }
                                   select new
                                   {
                                       ma.matchId,
                                       ma.playerId,
                                       ma.gold,
                                       ma.KDA,
                                       pmd.champion_name,
                                       pmd.champion_name_ch,
                                       pmd.champIcon,
                                       pmd.firstSpellIcon,
                                       pmd.secondSpellIcon,
                                       pmd.warScore,
                                       pmd.lastHits,
                                       pmd.creeps,
                                       pmd.towersDestroyed,
                                       pmd.barracksDestroyed,
                                       pmd.wards,
                                       pmd.dewards,
                                       pmd.maxContKills,
                                       pmd.maxMultiKills,
                                       pmd.maxCrit,
                                       pmd.totalHeal,
                                       pmd.totalDmg,
                                       pmd.totalTank,
                                       pmd.totalHeroDmg,
                                       pmd.totalHeroPhyDmg,
                                       pmd.totalHeroMagicDmg,
                                       pmd.totalHeroTrueDmg
                                   };
        var _joinedMatchDetailsB = from mb in _matchDetailsB
                                   join pmd in _playerMatchDetails
                                   on new { mId = mb.matchId, pId = mb.playerId } equals new { mId = pmd.matchId, pId = pmd.playerId }
                                   select new
                                   {
                                       mb.matchId,
                                       mb.playerId,
                                       mb.gold,
                                       mb.KDA,
                                       pmd.champion_name,
                                       pmd.champion_name_ch,
                                       pmd.champIcon,
                                       pmd.firstSpellIcon,
                                       pmd.secondSpellIcon,
                                       pmd.warScore,
                                       pmd.lastHits,
                                       pmd.creeps,
                                       pmd.towersDestroyed,
                                       pmd.barracksDestroyed,
                                       pmd.wards,
                                       pmd.dewards,
                                       pmd.maxContKills,
                                       pmd.maxMultiKills,
                                       pmd.maxCrit,
                                       pmd.totalHeal,
                                       pmd.totalDmg,
                                       pmd.totalTank,
                                       pmd.totalHeroDmg,
                                       pmd.totalHeroPhyDmg,
                                       pmd.totalHeroMagicDmg,
                                       pmd.totalHeroTrueDmg
                                   };

        gvMatchHeader.DataSource = _matchHeader;
        gvMatchHeader.DataBind();
        gvMatchDetailsA.DataSource = _joinedMatchDetailsA;
        gvMatchDetailsA.DataBind();
        gvMatchDetailsB.DataSource = _joinedMatchDetailsB;
        gvMatchDetailsB.DataBind();

        ScriptManager.RegisterClientScriptBlock(upDetails, upDetails.GetType(), "showDetails", "showDetails()", true);
    }
    protected void gvMatchDetailsA_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string matchId = gvMatchDetailsA.DataKeys[e.Row.RowIndex].Value.ToString();
            GridView gvItemsA = e.Row.FindControl("gvItemsA") as GridView;
            System.Web.UI.WebControls.Label lblPlayerId = e.Row.FindControl("lblPlayerId") as System.Web.UI.WebControls.Label;
            string playerId = lblPlayerId.Text;
            var _joinedItemsA = from ma in _matchDetailsA
                                join ia in _itemA
                                on new { mId = ma.matchId, pId = ma.playerId } equals new { mId = ia.matchId, pId = ia.playerId }
                                select new
                                {
                                    ma.matchId,
                                    ma.playerId,
                                    ia.itemName1,
                                    ia.itemIcon1,
                                    ia.itemDesc1,
                                    ia.itemName2,
                                    ia.itemIcon2,
                                    ia.itemDesc2,
                                    ia.itemName3,
                                    ia.itemIcon3,
                                    ia.itemDesc3,
                                    ia.itemName4,
                                    ia.itemIcon4,
                                    ia.itemDesc4,
                                    ia.itemName5,
                                    ia.itemIcon5,
                                    ia.itemDesc5,
                                    ia.itemName6,
                                    ia.itemIcon6,
                                    ia.itemDesc6
                                };
            var _itemsA = from ia in _joinedItemsA
                          where ia.matchId == matchId && ia.playerId == playerId
                          select new
                          {
                              ia.itemName1,
                              ia.itemIcon1,
                              ia.itemDesc1,
                              ia.itemName2,
                              ia.itemIcon2,
                              ia.itemDesc2,
                              ia.itemName3,
                              ia.itemIcon3,
                              ia.itemDesc3,
                              ia.itemName4,
                              ia.itemIcon4,
                              ia.itemDesc4,
                              ia.itemName5,
                              ia.itemIcon5,
                              ia.itemDesc5,
                              ia.itemName6,
                              ia.itemIcon6,
                              ia.itemDesc6
                          };
            gvItemsA.DataSource = _itemsA;
            gvItemsA.DataBind();
        }
    }
    protected void gvMatchDetailsB_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string matchId = gvMatchDetailsA.DataKeys[e.Row.RowIndex].Value.ToString();
            GridView gvItemsB = e.Row.FindControl("gvItemsB") as GridView;
            System.Web.UI.WebControls.Label lblPlayerId = e.Row.FindControl("lblPlayerId") as System.Web.UI.WebControls.Label;
            string playerId = lblPlayerId.Text;
            var _joinedItemsB = from mb in _matchDetailsB
                                join ib in _itemB
                                on new { mId = mb.matchId, pId = mb.playerId } equals new { mId = ib.matchId, pId = ib.playerId }
                                select new
                                {
                                    mb.matchId,
                                    mb.playerId,
                                    ib.itemName1,
                                    ib.itemIcon1,
                                    ib.itemDesc1,
                                    ib.itemName2,
                                    ib.itemIcon2,
                                    ib.itemDesc2,
                                    ib.itemName3,
                                    ib.itemIcon3,
                                    ib.itemDesc3,
                                    ib.itemName4,
                                    ib.itemIcon4,
                                    ib.itemDesc4,
                                    ib.itemName5,
                                    ib.itemIcon5,
                                    ib.itemDesc5,
                                    ib.itemName6,
                                    ib.itemIcon6,
                                    ib.itemDesc6
                                };
            var _itemsB = from ib in _joinedItemsB
                          where ib.matchId == matchId && ib.playerId == playerId
                          select new
                          {
                              ib.itemName1,
                              ib.itemIcon1,
                              ib.itemDesc1,
                              ib.itemName2,
                              ib.itemIcon2,
                              ib.itemDesc2,
                              ib.itemName3,
                              ib.itemIcon3,
                              ib.itemDesc3,
                              ib.itemName4,
                              ib.itemIcon4,
                              ib.itemDesc4,
                              ib.itemName5,
                              ib.itemIcon5,
                              ib.itemDesc5,
                              ib.itemName6,
                              ib.itemIcon6,
                              ib.itemDesc6
                          };
            gvItemsB.DataSource = _itemsB;
            gvItemsB.DataBind();
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
    public class played_champs_display
    {
        public string icon { get; set; }
        public string championName { get; set; }
        public string championNameCN { get; set; }
        public string winRate { get; set; }
        public string matchStat { get; set; }
        public string averageKDA { get; set; }
        public string averageKDARating { get; set; }
        public string averageDamage { get; set; }
        public string averageEarn { get; set; }
        public string averageMinionsKilled { get; set; }
        public string totalMVP { get; set; }
        public string totalHope { get; set; }
    }
    public class played_champs
    {
        public string snFull { get; set; }
        public string championsNum { get; set; }
        public List<played_champs_content> content { get; set; }
    }
    public class played_champs_content
    {
        public string championName { get; set; }
        public string championNameCN { get; set; }
        public string winRate { get; set; }
        public string matchStat { get; set; }
        public string[] averageKDA { get; set; }
        public string averageKDARating { get; set; }
        public string[] averageDamage { get; set; }
        public string[] averageEarn { get; set; }
        public string averageMinionsKilled { get; set; }
        public string totalMVP { get; set; }
        public string totalHope { get; set; }
    }
    public class match_list
    {
        public string id { get; set; }
        public string playerId { get; set; }
        public string champion_name_ch { get; set; }
        public string icon { get; set; }
        public string status { get; set; }
        public string mode { get; set; }
        public string date { get; set; }
    }
    public class matchHeader
    {
        public string mode { get; set; }
        public string duration { get; set; }
        public string endTime { get; set; }
        public string kills { get; set; }
        public string gold { get; set; }
    }
    public class matchDetails
    {
        public string matchId { get; set; }
        public string playerId { get; set; }
        public string gold { get; set; }
        public string KDA { get; set; }
    }
    public class honour
    {
        public string playerId { get; set; }
        public string matchId { get; set; }
        public string honourDesc { get; set; }
    }
    public class items
    {
        public string playerId { get; set; }
        public string matchId { get; set; }
        public string itemIcon1 { get; set; }
        public string itemName1 { get; set; }
        public string itemDesc1 { get; set; }
        public string itemIcon2 { get; set; }
        public string itemName2 { get; set; }
        public string itemDesc2 { get; set; }
        public string itemIcon3 { get; set; }
        public string itemName3 { get; set; }
        public string itemDesc3 { get; set; }
        public string itemIcon4 { get; set; }
        public string itemName4 { get; set; }
        public string itemDesc4 { get; set; }
        public string itemIcon5 { get; set; }
        public string itemName5 { get; set; }
        public string itemDesc5 { get; set; }
        public string itemIcon6 { get; set; }
        public string itemName6 { get; set; }
        public string itemDesc6 { get; set; }
    }
    public class playerMatchDetails
    {
        public string matchId { get; set; }
        public string playerId { get; set; }
        public string champion_name { get; set; }
        public string champion_name_ch { get; set; }
        public string champIcon { get; set; }
        public string firstSpellIcon { get; set; }
        public string secondSpellIcon { get; set; }
        public string warScore { get; set; }
        public string lastHits { get; set; }
        public string creeps { get; set; }
        public string towersDestroyed { get; set; }
        public string barracksDestroyed { get; set; }
        public string wards { get; set; }
        public string dewards { get; set; }
        public string maxContKills { get; set; }
        public string maxMultiKills { get; set; }
        public string maxCrit { get; set; }
        public string totalHeal { get; set; }
        public string totalDmg { get; set; }
        public string totalTank { get; set; }
        public string totalHeroDmg { get; set; }
        public string totalHeroPhyDmg { get; set; }
        public string totalHeroMagicDmg { get; set; }
        public string totalHeroTrueDmg { get; set; }
    }
}