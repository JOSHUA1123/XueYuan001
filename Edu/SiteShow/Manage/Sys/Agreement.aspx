<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true" CodeBehind="Agreement.aspx.cs" Inherits="SiteShow.Manage.Sys.Agreement" Title="学员注册协议" %>
<%@ Register Assembly="WebEditor" Namespace="WebEditor" TagPrefix="WebEditor" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">

<p class="title">注：{platform}平台名称，{org}机构简称，{domain}为域名</p>
  <p>
    <WebEditor:Editor ID="tbAgreement" runat="server" Height="400px" Width="99%">
    </WebEditor:Editor>
  </p>
  <p>&nbsp;</p>
  <p> 　　　　　
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
  </p>
</asp:Content>
