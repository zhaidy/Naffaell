<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Testing.aspx.cs" Inherits="Testing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="Scripts/jquery-2.1.4.min.js"></script>
    <script type="text/javascript">
        function GetPlayerProfile() {
            $.ajax({
                type: "GET",
                url: "http://naffaell.azurewebsites.net/GetPlayerService.asmx/GetPlayerProfile?server=" + "网通一" + "&playerId=" + "我不是个善良的人",
                dataType: "text",
                success: function (msg) {
                    msg = msg.replace(/\<(.*?)\>/g, "");
                    var result = $.parseJSON(msg);
                    $('#AjaxPlaceHolder').html(result.player_id);
                },
                error: function (e) {
                    $('#AjaxPlaceHolder').html("Unavailable");
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-header">
        <input id="btnTest" type="button" value="Test" onclick="GetPlayerProfile()" />
        <textarea id="test"></textarea>
        <div id="AjaxPlaceHolder">2</div>
    </div>
</asp:Content>

