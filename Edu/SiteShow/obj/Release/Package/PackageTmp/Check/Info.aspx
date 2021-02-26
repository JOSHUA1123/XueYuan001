<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Info.aspx.cs" Inherits="SiteShow.Check.Info" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lbOrgName" runat="server" Text=""></asp:Label>
    </div>
    <hr />
    <div class="infoBox">
      
            <div class="infoItem">专业 <asp:Label ID="zy" runat="server"></asp:Label> 个</div>
            <div class="infoItem">课程<asp:Label ID="kc" runat="server"></asp:Label>个</div>
            <div class="infoItem">试题<asp:Label ID="st" runat="server"></asp:Label>道</div>
            <div class="infoItem">考试<asp:Label ID="ks" runat="server"></asp:Label>场</div>
            <div class="infoItem">学员<asp:Label ID="sy" runat="server"></asp:Label>个</div>
            <div class="infoItem">资讯<asp:Label ID="zx" runat="server"></asp:Label>条</div>
            </div>
    </form>
</body>
</html>
