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
                        <asp:ListItem Value="网通一">比尔吉沃特</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-2 control-label">游戏ID</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtPlayerId" runat="server" Text="zz雨" CssClass="form-control"></asp:TextBox>
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
        <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered"></asp:GridView>
        <asp:GridView ID="GridView2" runat="server" CssClass="table table-bordered"></asp:GridView>
        <asp:GridView ID="GridView3" runat="server" CssClass="table table-bordered"></asp:GridView>
        <asp:GridView ID="GridView4" runat="server" CssClass="table table-bordered"></asp:GridView>
    </div>
</asp:Content>

