﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tester.aspx.cs" Inherits="SiteShow.Manage.Tester" %>

<%@ Register Src="Utility/ExcelInput.ascx" TagName="ExcelInput" TagPrefix="uc1" %>
<%@ Register Src="Utility/Uploader.ascx" TagName="Uploader" TagPrefix="uc2" %>
<!DOCTYPE html>
<html>
<script type="text/javascript" src="CoreScripts/jquery.js"></script>
<head runat="server">
    <title>无标题页</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="initial-scale=1,maximum-scale=1,user-scalable=no" />
    <meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1.0, user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="format-detection" content="telephone=yes" />
    <meta name="format-detection" content="email=no" />
    <script type="text/javascript" src="/Utility/CoreScripts/jquery.js"></script>
    <script type="text/javascript" src="/Utility/CoreScripts/Verify.js"></script>
    <script type="text/javascript" src="/Utility/CoreScripts/Extend.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    IPad：
    <asp:Label ID="isiPad" runat="server" Text="Label"></asp:Label>
    <br />
    Mobile:
    <asp:Label ID="isMobile" runat="server" Text="Label"></asp:Label>
    <br />
    IPhone：
    <asp:Label ID="isPhone" runat="server" Text="Label"></asp:Label>
    <br />
    OS：
    <asp:Label ID="lbOs" runat="server" Text="Label"></asp:Label>
    <hr />
     <asp:Label ID="lbUseagent" runat="server" Text="Label"></asp:Label>
    </form>
</body>
</html>
