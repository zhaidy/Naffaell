<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GetPlayer.aspx.cs" Inherits="GetPlayer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .tableA td {
            padding: 10px 5px;
            text-align: center;
            border-bottom: 1px solid #eee;
            border-top: transparent;
            border-left: transparent;
            border-right: transparent;
        }

        .tableA th {
            padding: 10px 5px;
            text-align: center;
            border-bottom: 1px solid #eee;
            border-top: transparent;
            border-left: transparent;
            border-right: transparent;
        }

        .tableB td {
            padding: 10px 5px;
            text-align: center;
            border-bottom: transparent;
            border-top: transparent;
            border-left: transparent;
            border-right: transparent;
        }

        .tableB th {
            padding: 10px 5px;
            text-align: center;
            border-bottom: transparent;
            border-top: transparent;
            border-left: transparent;
            border-right: transparent;
        }
    </style>
    <script src="https://code.jquery.com/jquery-2.1.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script type="text/javascript">
        function showDetails() {
            jQuery.noConflict();
            $('#myModal').modal('show');
        }
        $(document).on('click', '[src*=plus]', function (e) {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999' style='text-align: center'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "Images/minus.png");
        });
        $(document).on('click', '[src*=minus]', function (e) {
            $(this).attr("src", "Images/plus.png");
            $(this).closest("tr").next().remove();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-header">
        <div class="container">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label">服务器</label>
                    <div class="col-sm-10">
                        <asp:DropDownList ID="dpServer" runat="server" CssClass="form-control">
                            <asp:ListItem Value="电信一">艾欧尼亚</asp:ListItem>
                            <asp:ListItem Value="电信二">祖安</asp:ListItem>
                            <asp:ListItem Value="电信三">诺克萨斯</asp:ListItem>
                            <asp:ListItem Value="电信四">班德尔城</asp:ListItem>
                            <asp:ListItem Value="电信五">皮尔特沃夫</asp:ListItem>
                            <asp:ListItem Value="电信六">战争学院</asp:ListItem>
                            <asp:ListItem Value="电信七">巨神峰</asp:ListItem>
                            <asp:ListItem Value="电信八">雷瑟守备</asp:ListItem>
                            <asp:ListItem Value="电信九">裁决之地</asp:ListItem>
                            <asp:ListItem Value="电信十">黑色玫瑰</asp:ListItem>
                            <asp:ListItem Value="电信十一">暗影岛</asp:ListItem>
                            <asp:ListItem Value="电信十二">钢铁烈阳</asp:ListItem>
                            <asp:ListItem Value="电信十三">水晶之痕</asp:ListItem>
                            <asp:ListItem Value="电信十四">均衡教派</asp:ListItem>
                            <asp:ListItem Value="电信十五">影流</asp:ListItem>
                            <asp:ListItem Value="电信十六">守望之海</asp:ListItem>
                            <asp:ListItem Value="电信十七">征服之海</asp:ListItem>
                            <asp:ListItem Value="电信十八">卡拉曼达</asp:ListItem>
                            <asp:ListItem Value="电信十九">皮城警备</asp:ListItem>
                            <asp:ListItem Value="网通一" Selected="True">比尔吉沃特</asp:ListItem>
                            <asp:ListItem Value="网通二">德玛西亚</asp:ListItem>
                            <asp:ListItem Value="网通三">弗雷尔卓德</asp:ListItem>
                            <asp:ListItem Value="网通四">无畏先锋</asp:ListItem>
                            <asp:ListItem Value="网通五">怒瑞玛</asp:ListItem>
                            <asp:ListItem Value="网通六">扭曲丛林</asp:ListItem>
                            <asp:ListItem Value="网通七">巨龙之巢</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">游戏ID</label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txtPlayerId" runat="server" Text="" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <asp:Button ID="btnSubmit" runat="server" Text="查询" OnClick="btnSubmit_Click" CssClass="btn btn-success" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="container">
        <asp:UpdatePanel ID="upMain" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                <asp:GridView ID="gvPlayerProfile" runat="server" CssClass="table table-condensed tableA" AutoGenerateColumns="false" 
                    Style="border-right: transparent; border-left: transparent; border: transparent;" Caption="召唤师" DataKeyNames="player_id"  OnRowDataBound="gvPlayerProfile_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="头像">
                            <ItemTemplate>
                                <asp:Image ID="imgIcon" runat="server" ImageUrl='<%# Bind("icon") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="player_id" HeaderText="游戏 ID" />
                        <asp:BoundField DataField="server" HeaderText="大区" />
                        <asp:BoundField DataField="level" HeaderText="等级" />
                        <asp:BoundField DataField="fighting" HeaderText="战斗力" />
                        <asp:TemplateField HeaderText="常用英雄">
                            <ItemTemplate>
                                <asp:DataList ID="dlComChamp"
                                    RepeatDirection="Horizontal"
                                    RepeatLayout="Table"
                                    RepeatColumns="0" runat="server" ShowHeader="false" HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Image ID="champIcon" runat="server" ImageUrl='<%# Bind("icon") %>' ToolTip='<%# Eval("champion_name_ch") + " " + Eval("count") + "次" %>' />
                                    </ItemTemplate>
                                </asp:DataList>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:GridView ID="gvNormalStat" runat="server" CssClass="table table-condensed tableA" AutoGenerateColumns="false" Caption="匹配"
                    Style="border-right: transparent; border-left: transparent; border: transparent;">
                    <Columns>
                        <asp:BoundField DataField="type" HeaderText="模式" />
                        <asp:BoundField DataField="total_matches" HeaderText="场次" />
                        <asp:BoundField DataField="win_rate" HeaderText="胜率" />
                        <asp:BoundField DataField="matches_winned" HeaderText="胜场" />
                        <asp:BoundField DataField="matches_lost" HeaderText="负场" />
                    </Columns>
                </asp:GridView>
                <asp:GridView ID="gvRankStat" runat="server" CssClass="table table-condensed tableA" AutoGenerateColumns="false" Caption="排位"
                    Style="border-right: transparent; border-left: transparent; border: transparent;">
                    <Columns>
                        <asp:BoundField DataField="type" HeaderText="模式" />
                        <asp:BoundField DataField="rank" HeaderText="段位/级别" />
                        <asp:BoundField DataField="point" HeaderText="胜点" />
                        <asp:BoundField DataField="total_matches" HeaderText="场次" />
                        <asp:BoundField DataField="win_rate" HeaderText="胜率" />
                        <asp:BoundField DataField="matches_winned" HeaderText="胜场" />
                        <asp:BoundField DataField="matches_lost" HeaderText="负场" />
                    </Columns>
                </asp:GridView>
                <%--<asp:GridView ID="gvComChamp" runat="server" CssClass="table table-condensed tableA" AutoGenerateColumns="false" Caption="常用英雄"
                    Style="border-right: transparent; border-left: transparent; border: transparent;">
                    <Columns>
                        <asp:TemplateField HeaderText="头像">
                            <ItemTemplate>
                                <asp:Image ID="imgIcon" runat="server" ImageUrl='<%# Bind("icon") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="champion_name_ch" HeaderText="英雄" />
                        <asp:BoundField DataField="count" HeaderText="次数" />
                    </Columns>
                </asp:GridView>--%>
                <asp:GridView ID="gvPlayedChamps" runat="server" CssClass="table table-condensed tableA" AutoGenerateColumns="false" Caption="英雄记录"
                    Style="border-right: transparent; border-left: transparent; border: transparent;" AllowPaging="True" PageSize="8" OnPageIndexChanging="gvPlayedChamps_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="头像">
                            <ItemTemplate>
                                <asp:Image ID="imgIcon" runat="server" ImageUrl='<%# Bind("icon") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="championNameCN" HeaderText="英雄" />
                        <asp:BoundField DataField="matchStat" HeaderText="场次" />
                        <asp:BoundField DataField="winRate" HeaderText="胜率" />
                        <asp:BoundField DataField="averageKDARating" HeaderText="场均战损" />
                        <asp:BoundField DataField="averageKDA" HeaderText="场均杀死助" />
                        <asp:BoundField DataField="averageDamage" HeaderText="场均分钟输出" />
                        <asp:BoundField DataField="averageEarn" HeaderText="场均分钟经济" />
                        <asp:BoundField DataField="averageMinionsKilled" HeaderText="场均十分钟补兵" />
                        <asp:BoundField DataField="totalMVP" HeaderText="MVP次数" />
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="pagination-ys" />
                </asp:GridView>
                <asp:GridView ID="gvMatchList" runat="server" CssClass="table table-condensed tableA" AutoGenerateColumns="false" DataKeyNames="id" Caption="最近比赛"
                    Style="border-right: transparent; border-left: transparent; border: transparent;" AllowPaging="True" PageSize="8" OnPageIndexChanging="gvMatchList_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="头像">
                            <ItemTemplate>
                                <asp:Image ID="imgIcon" runat="server" ImageUrl='<%# Bind("icon") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="champion_name_ch" HeaderText="英雄" />
                        <asp:BoundField DataField="status" HeaderText="胜/负" />
                        <asp:BoundField DataField="mode" HeaderText="模式" />
                        <asp:BoundField DataField="date" HeaderText="日期" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnMatchDetail" runat="server" Text="详细" OnClick="btnMatchDetail_Click" CssClass="btn btn-info" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="pagination-ys" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="upDetails" runat="server">
        <ContentTemplate>
            <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
                aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span></button>
                            <asp:Label ID="lblMsg2" Text="" runat="server" class="h4 modal-title"></asp:Label>
                        </div>
                        <div class="modal-body">
                            <div class="container-fluid">
                                <asp:GridView ID="gvMatchHeader" runat="server" CssClass="table table-condensed tableA" AutoGenerateColumns="false"
                                    Style="border-right: transparent; border-left: transparent; border: transparent;">
                                    <Columns>
                                        <asp:BoundField DataField="mode" HeaderText="类型" />
                                        <asp:BoundField DataField="duration" HeaderText="时长" />
                                        <asp:BoundField DataField="endTime" HeaderText="结束" />
                                        <asp:BoundField DataField="kills" HeaderText="人头" />
                                        <asp:BoundField DataField="gold" HeaderText="金钱" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="container-fluid">
                                <asp:GridView ID="gvMatchDetailsA" runat="server" CssClass="table table-hover table-condensed tableA" 
                                    AutoGenerateColumns="false" DataKeyNames="matchId" OnRowDataBound="gvMatchDetailsA_RowDataBound" Caption="<span style='color:green;'>胜利</span>"
                                    Style="border-right: transparent; border-left: transparent; border: transparent;">
                                    <Columns>
                                        <asp:TemplateField HeaderText="召唤师">
                                            <ItemTemplate>
                                                <asp:Image ID="imgChampIcon" runat="server" ImageUrl='<%# Bind("champIcon") %>' ToolTip='<%# Bind("champion_name_ch") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="container-fluid" style="text-align: left;">
                                                    <div class="row">
                                                        <asp:Label ID="lblPlayerId" runat="server" Text='<%# Bind("playerId") %>'></asp:Label>
                                                    </div>
                                                    <div class="row">
                                                        <asp:Image ID="spell1Icon" runat="server" ImageUrl='<%# Bind("firstSpellIcon") %>' />
                                                        <asp:Image ID="spell2Icon" runat="server" ImageUrl='<%# Bind("secondSpellIcon") %>' />
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="KDA">
                                            <ItemTemplate>
                                                <asp:Label ID="lblKDA" runat="server" Text='<%# Bind("KDA") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<span style='color:#FF5400;'>经济</span>/补刀">
                                            <ItemTemplate>
                                                <div class="container-fluid">
                                                    <div class="row">
                                                        <asp:Label ID="lblGold" runat="server" Text='<%# Bind("gold") %>' ForeColor="#ff5400"></asp:Label>
                                                    </div>
                                                    <div class="row">
                                                        <asp:Label ID="lblLastHits" runat="server" Text='<%# Bind("lastHits") %>'></asp:Label>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<span style='color:#FF5400;'>插眼</span>/反眼">
                                            <ItemTemplate>
                                                <div class="container-fluid">
                                                    <div class="row">
                                                        <asp:Label ID="lblWards" runat="server" Text='<%# Bind("wards") %>' ForeColor="#ff5400"></asp:Label>
                                                    </div>
                                                    <div class="row">
                                                        <asp:Label ID="lblDewards" runat="server" Text='<%# Bind("dewards") %>'></asp:Label>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="出装">
                                            <ItemTemplate>
                                                <div>
                                                    <asp:GridView ID="gvItemsA" runat="server" AutoGenerateColumns="false" ShowHeader="false" BorderWidth="0px">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Image ID="itemIcon1" runat="server" ImageUrl='<%# Bind("itemIcon1") %>' ToolTip='<%# Bind("itemDesc1") %>' />
                                                                    <asp:Image ID="itemIcon2" runat="server" ImageUrl='<%# Bind("itemIcon2") %>' ToolTip='<%# Bind("itemDesc2") %>' />
                                                                    <asp:Image ID="itemIcon3" runat="server" ImageUrl='<%# Bind("itemIcon3") %>' ToolTip='<%# Bind("itemDesc3") %>' />
                                                                    <asp:Image ID="itemIcon4" runat="server" ImageUrl='<%# Bind("itemIcon4") %>' ToolTip='<%# Bind("itemDesc4") %>' />
                                                                    <asp:Image ID="itemIcon5" runat="server" ImageUrl='<%# Bind("itemIcon5") %>' ToolTip='<%# Bind("itemDesc5") %>' />
                                                                    <asp:Image ID="itemIcon6" runat="server" ImageUrl='<%# Bind("itemIcon6") %>' ToolTip='<%# Bind("itemDesc6") %>' />
                                                                    <asp:Image ID="itemIcon7" runat="server" ImageUrl='<%# Bind("itemIcon7") %>' ToolTip='<%# Bind("itemDesc7") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Image ID="img" runat="server" Style="cursor: pointer" src="Images/plus.png" />
                                                <asp:Panel ID="pnlDetails" runat="server" Style="display: none">
                                                    <table class="table table-condensed tableB" style="border-right: transparent; border-left: transparent; border: transparent;">
                                                        <tr>
                                                            <td style="color: #D8A68B;">战局评分:</td>
                                                            <td colspan="3" style="color: #ff9900;">
                                                                <asp:Label ID="lblWarScore" runat="server" Text='<%# Bind("warScore") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="color: black;">推塔:</td>
                                                            <td>
                                                                <asp:Label ID="lblTowersDestroyed" runat="server" Text='<%# Bind("towersDestroyed") %>'></asp:Label>
                                                            </td>
                                                            <td>兵营:</td>
                                                            <td>
                                                                <asp:Label ID="lblBarracksDestroyed" runat="server" Text='<%# Bind("barracksDestroyed") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>最大连杀:</td>
                                                            <td>
                                                                <asp:Label ID="lblMaxContKills" runat="server" Text='<%# Bind("maxContKills") %>'></asp:Label>
                                                            </td>
                                                            <td>最大多杀:</td>
                                                            <td>
                                                                <asp:Label ID="lblMaxMultiKills" runat="server" Text='<%# Bind("maxMultiKills") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>最大暴击:</td>
                                                            <td>
                                                                <asp:Label ID="lblMaxCrit" runat="server" Text='<%# Bind("maxCrit") %>'></asp:Label>
                                                            </td>
                                                            <td>总治疗:</td>
                                                            <td>
                                                                <asp:Label ID="lblTotalHeal" runat="server" Text='<%# Bind("totalHeal") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>输出伤害:</td>
                                                            <td>
                                                                <asp:Label ID="lblTotalDmg" runat="server" Text='<%# Bind("totalDmg") %>'></asp:Label>
                                                            </td>
                                                            <td>承受敌害:</td>
                                                            <td>
                                                                <asp:Label ID="lblTotalTank" runat="server" Text='<%# Bind("totalTank") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>给对方英雄造成总伤害:</td>
                                                            <td colspan="3">
                                                                <asp:Label ID="lblTotalHeroDmg" runat="server" Text='<%# Bind("totalHeroDmg") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>给对方英雄的物理伤害:</td>
                                                            <td colspan="3">
                                                                <asp:Label ID="lblTotalHeroPhyDmg" runat="server" Text='<%# Bind("totalHeroPhyDmg") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>给对方英雄的魔法伤害:</td>
                                                            <td colspan="3">
                                                                <asp:Label ID="lblTotalHeroMagicDmg" runat="server" Text='<%# Bind("totalHeroMagicDmg") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>给对方英雄的真实伤害:</td>
                                                            <td colspan="3">
                                                                <asp:Label ID="lblTotalHeroTrueDmg" runat="server" Text='<%# Bind("totalHeroTrueDmg") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="container-fluid">
                                <asp:GridView ID="gvMatchDetailsB" runat="server" CssClass="table table-hover table-condensed tableA"
                                    AutoGenerateColumns="false" DataKeyNames="matchId" OnRowDataBound="gvMatchDetailsB_RowDataBound" Caption="<span style='color:red;'>失败</span>"
                                    Style="border-right: transparent; border-left: transparent; border: transparent;">
                                    <Columns>
                                        <asp:TemplateField HeaderText="召唤师">
                                            <ItemTemplate>
                                                <asp:Image ID="imgChampIcon" runat="server" ImageUrl='<%# Bind("champIcon") %>' ToolTip='<%# Bind("champion_name_ch") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="container-fluid" style="text-align: left;">
                                                    <div class="row">
                                                        <asp:Label ID="lblPlayerId" runat="server" Text='<%# Bind("playerId") %>'></asp:Label>
                                                    </div>
                                                    <div class="row">
                                                        <asp:Image ID="spell1Icon" runat="server" ImageUrl='<%# Bind("firstSpellIcon") %>' />
                                                        <asp:Image ID="spell2Icon" runat="server" ImageUrl='<%# Bind("secondSpellIcon") %>' />
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="KDA">
                                            <ItemTemplate>
                                                <asp:Label ID="lblKDA" runat="server" Text='<%# Bind("KDA") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<span style='color:#FF5400;'>经济</span>/补刀">
                                            <ItemTemplate>
                                                <div class="container-fluid">
                                                    <div class="row">
                                                        <asp:Label ID="lblGold" runat="server" Text='<%# Bind("gold") %>' ForeColor="#ff5400"></asp:Label>
                                                    </div>
                                                    <div class="row">
                                                        <asp:Label ID="lblLastHits" runat="server" Text='<%# Bind("lastHits") %>'></asp:Label>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<span style='color:#FF5400;'>插眼</span>/反眼">
                                            <ItemTemplate>
                                                <div class="container-fluid">
                                                    <div class="row">
                                                        <asp:Label ID="lblWards" runat="server" Text='<%# Bind("wards") %>' ForeColor="#ff5400"></asp:Label>
                                                    </div>
                                                    <div class="row">
                                                        <asp:Label ID="lblDewards" runat="server" Text='<%# Bind("dewards") %>'></asp:Label>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="出装">
                                            <ItemTemplate>
                                                <div>
                                                    <asp:GridView ID="gvItemsB" runat="server" AutoGenerateColumns="false" ShowHeader="false" BorderWidth="0px">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Image ID="itemIcon1" runat="server" ImageUrl='<%# Bind("itemIcon1") %>' ToolTip='<%# Bind("itemDesc1") %>' />
                                                                    <asp:Image ID="itemIcon2" runat="server" ImageUrl='<%# Bind("itemIcon2") %>' ToolTip='<%# Bind("itemDesc2") %>' />
                                                                    <asp:Image ID="itemIcon3" runat="server" ImageUrl='<%# Bind("itemIcon3") %>' ToolTip='<%# Bind("itemDesc3") %>' />
                                                                    <asp:Image ID="itemIcon4" runat="server" ImageUrl='<%# Bind("itemIcon4") %>' ToolTip='<%# Bind("itemDesc4") %>' />
                                                                    <asp:Image ID="itemIcon5" runat="server" ImageUrl='<%# Bind("itemIcon5") %>' ToolTip='<%# Bind("itemDesc5") %>' />
                                                                    <asp:Image ID="itemIcon6" runat="server" ImageUrl='<%# Bind("itemIcon6") %>' ToolTip='<%# Bind("itemDesc6") %>' />
                                                                    <asp:Image ID="itemIcon7" runat="server" ImageUrl='<%# Bind("itemIcon7") %>' ToolTip='<%# Bind("itemDesc7") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Image ID="img" runat="server" Style="cursor: pointer" src="Images/plus.png" />
                                                <asp:Panel ID="pnlDetails" runat="server" Style="display: none">
                                                    <table class="table table-condensed tableB" style="border-right: transparent; border-left: transparent; border: transparent;"">
                                                        <tr>
                                                            <td style="color: #D8A68B;">战局评分:</td>
                                                            <td colspan="3" style="color: #ff9900;">
                                                                <asp:Label ID="lblWarScore" runat="server" Text='<%# Bind("warScore") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="color: black;">推塔:</td>
                                                            <td>
                                                                <asp:Label ID="lblTowersDestroyed" runat="server" Text='<%# Bind("towersDestroyed") %>'></asp:Label>
                                                            </td>
                                                            <td>兵营:</td>
                                                            <td>
                                                                <asp:Label ID="lblBarracksDestroyed" runat="server" Text='<%# Bind("barracksDestroyed") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>最大连杀:</td>
                                                            <td>
                                                                <asp:Label ID="lblMaxContKills" runat="server" Text='<%# Bind("maxContKills") %>'></asp:Label>
                                                            </td>
                                                            <td>最大多杀:</td>
                                                            <td>
                                                                <asp:Label ID="lblMaxMultiKills" runat="server" Text='<%# Bind("maxMultiKills") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>最大暴击:</td>
                                                            <td>
                                                                <asp:Label ID="lblMaxCrit" runat="server" Text='<%# Bind("maxCrit") %>'></asp:Label>
                                                            </td>
                                                            <td>总治疗:</td>
                                                            <td>
                                                                <asp:Label ID="lblTotalHeal" runat="server" Text='<%# Bind("totalHeal") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>输出伤害:</td>
                                                            <td>
                                                                <asp:Label ID="lblTotalDmg" runat="server" Text='<%# Bind("totalDmg") %>'></asp:Label>
                                                            </td>
                                                            <td>承受敌害:</td>
                                                            <td>
                                                                <asp:Label ID="lblTotalTank" runat="server" Text='<%# Bind("totalTank") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>给对方英雄造成总伤害:</td>
                                                            <td colspan="3">
                                                                <asp:Label ID="lblTotalHeroDmg" runat="server" Text='<%# Bind("totalHeroDmg") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>给对方英雄的物理伤害:</td>
                                                            <td colspan="3">
                                                                <asp:Label ID="lblTotalHeroPhyDmg" runat="server" Text='<%# Bind("totalHeroPhyDmg") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>给对方英雄的魔法伤害:</td>
                                                            <td colspan="3">
                                                                <asp:Label ID="lblTotalHeroMagicDmg" runat="server" Text='<%# Bind("totalHeroMagicDmg") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>给对方英雄的真实伤害:</td>
                                                            <td colspan="3">
                                                                <asp:Label ID="lblTotalHeroTrueDmg" runat="server" Text='<%# Bind("totalHeroTrueDmg") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <%--<asp:GridView ID="gvMatchDetailsA" runat="server" CssClass="table table-condensed" AutoGenerateColumns="false" Font-Size="8pt">
                                <Columns>
                                    <asp:BoundField DataField="playerId" HeaderText="游戏ID" />
                                    <asp:TemplateField HeaderText="英雄">
                                        <ItemTemplate>
                                            <asp:Image ID="imgChampIcon" runat="server" ImageUrl='<%# Bind("champIcon") %>' />
                                            <asp:Label ID="lblChampName" runat="server" Text='<%# Bind("champion_name_ch") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="召唤师技能">
                                        <ItemTemplate>
                                            <asp:Image ID="spell1Icon" runat="server" ImageUrl='<%# Bind("firstSpellIcon") %>' />
                                            <asp:Image ID="spell2Icon" runat="server" ImageUrl='<%# Bind("secondSpellIcon") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="honour" HeaderText="荣誉" />
                                    <asp:BoundField DataField="gold" HeaderText="金钱" />
                                    <asp:BoundField DataField="KDA" HeaderText="KDA" />
                                    <asp:BoundField DataField="itemName" HeaderText="出装" />
                                    <asp:BoundField DataField="warScore" HeaderText="战局评分" />
                                    <asp:BoundField DataField="lastHits" HeaderText="补兵" />
                                    <asp:BoundField DataField="creeps" HeaderText="野怪" />
                                    <asp:BoundField DataField="towersDestroyed" HeaderText="推塔" />
                                    <asp:BoundField DataField="barracksDestroyed" HeaderText="兵营" />
                                    <asp:BoundField DataField="wards" HeaderText="放眼数" />
                                    <asp:BoundField DataField="dewards" HeaderText="排眼数" />
                                    <asp:BoundField DataField="maxContKills" HeaderText="最大连杀" />
                                    <asp:BoundField DataField="maxMultiKills" HeaderText="最大多杀" />
                                    <asp:BoundField DataField="maxCrit" HeaderText="最大暴击" />
                                    <asp:BoundField DataField="totalHeal" HeaderText="总治疗" />
                                    <asp:BoundField DataField="totalDmg" HeaderText="输出伤害" />
                                    <asp:BoundField DataField="totalTank" HeaderText="承受敌害" />
                                    <asp:BoundField DataField="totalHeroDmg" HeaderText="给对方英雄造成总伤害" />
                                    <asp:BoundField DataField="totalHeroPhyDmg" HeaderText="给对方英雄的物理伤害" />
                                    <asp:BoundField DataField="totalHeroMagicDmg" HeaderText="给对方英雄的魔法伤害" />
                                    <asp:BoundField DataField="totalHeroTrueDmg" HeaderText="给对方英雄的真实伤害" />
                                </Columns>
                            </asp:GridView>
                            <asp:GridView ID="gvMatchDetailsB" runat="server" CssClass="table table-condensed" AutoGenerateColumns="false" Font-Size="8pt">
                                <Columns>
                                    <asp:BoundField DataField="playerId" HeaderText="游戏ID" />
                                    <asp:TemplateField HeaderText="英雄">
                                        <ItemTemplate>
                                            <asp:Image ID="imgChampIcon" runat="server" ImageUrl='<%# Bind("champIcon") %>' />
                                            <asp:Label ID="lblChampName" runat="server" Text='<%# Bind("champion_name_ch") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="召唤师技能">
                                        <ItemTemplate>
                                            <asp:Image ID="spell1Icon" runat="server" ImageUrl='<%# Bind("firstSpellIcon") %>' />
                                            <asp:Image ID="spell2Icon" runat="server" ImageUrl='<%# Bind("secondSpellIcon") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="honour" HeaderText="荣誉" />
                                    <asp:BoundField DataField="gold" HeaderText="金钱" />
                                    <asp:BoundField DataField="KDA" HeaderText="KDA" />
                                    <asp:BoundField DataField="itemName" HeaderText="出装" />
                                    <asp:BoundField DataField="warScore" HeaderText="战局评分" />
                                    <asp:BoundField DataField="lastHits" HeaderText="补兵" />
                                    <asp:BoundField DataField="creeps" HeaderText="野怪" />
                                    <asp:BoundField DataField="towersDestroyed" HeaderText="推塔" />
                                    <asp:BoundField DataField="barracksDestroyed" HeaderText="兵营" />
                                    <asp:BoundField DataField="wards" HeaderText="放眼数" />
                                    <asp:BoundField DataField="dewards" HeaderText="排眼数" />
                                    <asp:BoundField DataField="maxContKills" HeaderText="最大连杀" />
                                    <asp:BoundField DataField="maxMultiKills" HeaderText="最大多杀" />
                                    <asp:BoundField DataField="maxCrit" HeaderText="最大暴击" />
                                    <asp:BoundField DataField="totalHeal" HeaderText="总治疗" />
                                    <asp:BoundField DataField="totalDmg" HeaderText="输出伤害" />
                                    <asp:BoundField DataField="totalTank" HeaderText="承受敌害" />
                                    <asp:BoundField DataField="totalHeroDmg" HeaderText="给对方英雄造成总伤害" />
                                    <asp:BoundField DataField="totalHeroPhyDmg" HeaderText="给对方英雄的物理伤害" />
                                    <asp:BoundField DataField="totalHeroMagicDmg" HeaderText="给对方英雄的魔法伤害" />
                                    <asp:BoundField DataField="totalHeroTrueDmg" HeaderText="给对方英雄的真实伤害" />
                                </Columns>
                            </asp:GridView>--%>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                关闭</button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

