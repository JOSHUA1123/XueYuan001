﻿<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="Questions_Edit4.aspx.cs" Inherits="SiteShow.Manage.Questions.Questions_Edit4"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WebEditor" Namespace="WebEditor" TagPrefix="WebEditor" %>
<%@ Register src="../Utility/SortSelect.ascx" tagname="SortSelect" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
<script type="text/javascript">
    //此代码为UpdatePanel刷新完成后要执行的代码
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            _KnowledgeAjax_int();
            $("#suggestListBox").hide();
        }
        else {
            alert("There was an error" + args.get_error().message);
        }
    }
    function load() {
        try{
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            }catch{}
    }
    $(load);
 </script>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
    <div class="quesLeft">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td width="80" class="right">
                    类型：</td>
                <td>
                    <asp:DropDownList ID="ddlType" runat="server" Enabled="False">
                    </asp:DropDownList>
                    &nbsp; 难度：<asp:DropDownList ID="ddlDiff" runat="server">
                        <asp:ListItem Value="1">1</asp:ListItem>
                        <asp:ListItem Value="2">2</asp:ListItem>
                        <asp:ListItem Value="3" Selected="True">3</asp:ListItem>
                        <asp:ListItem Value="4">4</asp:ListItem>
                        <asp:ListItem Value="5">5</asp:ListItem>
                    </asp:DropDownList>
                     &nbsp; 排序号：<asp:TextBox ID="tbTax" runat="server" Width="60" ToolTip="数值越小越靠前，可以为负值"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:CheckBox ID="cbIsUse" runat="server" Text="是否使用" ToolTip="禁用后，不会被随机抽出" Checked="True">
                    </asp:CheckBox></td>
            </tr>
            <tr>
                <td class="right">
                   所属章节： </td>
                <td>
                
<asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate>
                     <uc1:SortSelect ID="SortSelect1" runat="server" />
                      </ContentTemplate>
   
</asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class="right">
                   
                    知识关联： </td>
                <td>
                <cc1:DropDownTree ID="ddlKnlSort" runat="server" Width="150" IdKeyName="Kns_ID" ParentIdKeyName="Kns_PID"
                        TaxKeyName="Kns_Tax">
                    </cc1:DropDownTree>
                     <asp:TextBox ID="tbKnsName" runat="server" Width="80" CssClass="tbKnsName" state="noFocus" EnableTheming="false"></asp:TextBox>

                     <span id="knTitle" class="knTitle" runat="server">关联：无</span> <span style="display: none">
                        <asp:TextBox ID="tbKnID" runat="server"></asp:TextBox>
                        <asp:TextBox ID="tbKnTit" runat="server"></asp:TextBox>
                    </span>
                </td>
                </tr>
            <tr>
                <td class="right">
                    题干：</td>
                <td>
                    （字数：<span class="count"></span>） <span id="errorInfo" style="color:Red" visible=false runat=server>错误：<asp:Literal ID="ltErrorInfo" runat="server"></asp:Literal></span>
                     &nbsp;
                         <span id="wrongInfo" runat=server visible=false class="wrongInfo" style="color: Red">报错信息<asp:CheckBox ID="cbWrong" runat="server" Text="完成报错处理" /></span>
                         <div id="wrongInfoBox" style="display:none"><asp:Literal ID="ltWrongInfo" runat="server"></asp:Literal></div>
                    </td>
            </tr>
        </table>
        <WebEditor:Editor ID="tbTitle" runat="server" Height="180px" ThemeType="simple" Width="99%"
            afterChange="function(){K('.count').html(this.count('text'))}"></WebEditor:Editor>
        <div class="answerBox">
            <div style="height: 25px; width: 80px; text-align: right; line-height: 25px;float:left ">
                答案：</div><div class="answerNum">（字数：<span class="anscount"></span>）</div>
                 <WebEditor:Editor ID="tbAnswer" runat="server" Height="200px" ThemeType="simple" Width="99%"
            afterChange="function(){K('.anscount').html(this.count('text'))}"></WebEditor:Editor>
        </div>
    </div>
    <div class="quesRight">
        
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
           
            <tr>
                <td class="left">
                    试题解析：</td>
                <td colspan="2">
                </td>
            </tr>
        </table>
        <WebEditor:Editor ID="tbExplan" runat="server" Height="420px" ThemeType="simple"> </WebEditor:Editor>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
    <cc1:CloseButton ID="CloseButton1" runat="server" />

    <script type="text/javascript" src="Scripts/KnowledgeAjax.js"></script>

</asp:Content>
