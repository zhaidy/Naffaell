<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GetPlayer.aspx.cs" Inherits="GetPlayer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-header">
        <div class="container">
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
                    <asp:TextBox ID="txtPlayerId" runat="server" Text="我不是个善良的人" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                    <asp:Button ID="btnSubmit" runat="server" Text="查询" OnClick="btnSubmit_Click" CssClass="btn btn-success" />
                </div>
            </div>
        </div>
    </div>
    <div class="container">
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
        <asp:GridView ID="gvPlayerProfile" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false">
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
                <asp:BoundField DataField="first_win" HeaderText="首胜" />
            </Columns>
        </asp:GridView>
        <asp:GridView ID="gvPlayedChamps" runat="server" CssClass="table table-bordered" AutoGenerateColumns="true">
        </asp:GridView>
        <asp:GridView ID="gvComChamp" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false" Caption="常用英雄">
            <Columns>
                <asp:TemplateField HeaderText="头像">
                    <ItemTemplate>
                        <asp:Image ID="imgIcon" runat="server" ImageUrl='<%# Bind("icon") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="champion_name_ch" HeaderText="英雄" />
                <asp:BoundField DataField="count" HeaderText="次数" />
            </Columns>
        </asp:GridView>
        <asp:GridView ID="gvNormalStat" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false" Caption="匹配">
            <Columns>
                <asp:BoundField DataField="type" HeaderText="模式" />
                <asp:BoundField DataField="total_matches" HeaderText="场次" />
                <asp:BoundField DataField="win_rate" HeaderText="胜率" />
                <asp:BoundField DataField="matches_winned" HeaderText="胜场" />
                <asp:BoundField DataField="matches_lost" HeaderText="负场" />
            </Columns>
        </asp:GridView>
        <asp:GridView ID="gvRankStat" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false" Caption="排位">
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
        <asp:GridView ID="gvMatchList" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false" DataKeyNames="id" Caption="最近比赛">
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
                <%--<asp:TemplateField>
                    <ItemTemplate>
                        <asp:GridView ID="gvDetail" runat="server"></asp:GridView>
                    </ItemTemplate>
                </asp:TemplateField>--%>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

