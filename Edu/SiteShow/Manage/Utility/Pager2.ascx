<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pager2.ascx.cs" Inherits="SiteShow.Manage.Utility.Pager2" %>
<div class="pager2 noprint">

    <%= First %>
    <%= Prev %>
        <%= NumberNav(this.Index,"span")%>
    <%= Next %>
    <%= Last %>
        <span class="info">
        ��ǰ<%= Size >= RecordAmount ? RecordAmount : Size%>��/��<%= RecordAmount %>��</span>
         <span class="info">
          ��<%= PageAmount%>ҳ
         </span>
         <span class="info">
         ����<asp:TextBox ID="tbGoPagenum" runat="server" Width="40" nullable="false" star="false" datatype="uint" place="top" group="GoPagenum"></asp:TextBox>ҳ 
<asp:Button ID="btnGoPagenum" runat="server" Text="��ת" verify="true" 
        group="GoPagenum" onclick="btnGoPagenum_Click"/>
         </span>
    
    

</div>
