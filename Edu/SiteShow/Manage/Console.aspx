﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Console.aspx.cs"  Inherits="SiteShow.Manage.Console" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>系统管理-</title>
<style type="text/css">
<!--
body {
	padding: 0px !important;
}
html {
 overflow-y:hidden;
 overflow-x:hidden;
 padding: 0px !important;
}

-->
</style>
<meta http-equiv="Pragma" content="no-cache" />
<meta http-equiv="Cache-Control" content="no-cache" />
<meta http-equiv="Expires" content="0" />
</head>
<body class="consPageBody">
<div id="consFullMask" style="position: absolute; z-index:50000; width:100%; height:10000px;"></div>
<script type="text/javascript">
//初始化一些参数
//当前登录的员工id
var emplyeeId=<%= id %>;
//当前登录员工的姓名
var emplyeeName= "<%= name %>" ;
//当前登录员工是否为超管
var emplyeeIsAdmin= <%= isAdmin %> ;
//当前应用程序的状态
var appState="<%= appState %>";
</script>
<script type="text/javascript" src="/Utility/CoreScripts/jquery.js"></script>
<script type="text/javascript" src="/Utility/CoreScripts/easydrag.js"></script>
<script type="text/javascript" src="CoreScripts/Extend.js"></script>
<script type="text/javascript" src="Panel/Scripts/PagePanel.js"></script>
<script type="text/javascript" src="/Utility/CoreScripts/PageBox.js"></script>
<script type="text/javascript" src="Panel/Scripts/TreePanel.js"></script>
<script type="text/javascript" src="Panel/Scripts/DropMenu.js"></script>
<script type="text/javascript" src="Panel/Scripts/Loyout.js"></script>
<script type="text/javascript" src="Scripts/Console.js"></script>
<div class="logoBox" style="background-image: url(<%= copyright["logo"]%>)">
<span class="consInfo">欢迎
        <asp:HyperLink ID="lbName" NavigateUrl="Personal/View.aspx" runat="server" ForeColor="Red">[lbName]</asp:HyperLink>
        登录</span>
</div>
<div class="consAppName"><%= copyright["product"]%>-<span id="consVersionName" runat="server"></span></div>
<form id="Console" runat="server">
  <!--顶部区域 -->
  <div id="consPageTop">
    <div id="consTop">
    
      <div id="consDropMenu">
        <!-- 下拉菜单区域 -->
      </div>
    </div>
    <!--顶部区域下方横条 -->
    <div id="consNeck">
      <div id="consNeckLeft"> <a id="messageMarker" href="#" style="display:none">短消息&nbsp;<img src="Images/ICO/message1.gif" /></a> </div>
      <div id="consNeckRight">
        <asp:Label ID="lbTime" runat="server"></asp:Label>
      </div>
    </div>
  </div>
  <!-- 主要内容区 -->
  <div id="consBody">
    <div id="consTreePanel" bg="<%= copyright["weixinqr"]%>">&nbsp;
      <!-- 左侧树形菜单区域 -->
    </div>
    <dl id="consBoxTitleBar">
      <!-- 页面区域的标签栏 -->
    </dl>
    <div id="consContextPanel">
      <!-- 内容区编辑区 -->
    </div>
  </div>
</form>
</body>
</html>
