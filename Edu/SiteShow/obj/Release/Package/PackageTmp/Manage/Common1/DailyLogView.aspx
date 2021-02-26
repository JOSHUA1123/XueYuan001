﻿<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true" CodeBehind="DailyLogView.aspx.cs" Inherits="SiteShow.Manage.Common1.DailyLogView" Title="无标题页" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <div id="header">
    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>

    <uc1:toolsBar ID="ToolsBar1" runat="server" GvName="GridView1" WinWidth="600" WinHeight="450" WinPath="DailyLogView_Preview.aspx" 
    AddButtonVisible=false ModifyButtonVisible=false DelButtonVisible=false
        OnDelete="DeleteEvent"  />
    <div class="searchBox">
         类别：<asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                <asp:ListItem Selected="True" Value="1">日志</asp:ListItem>
                <asp:ListItem Value="2">周志</asp:ListItem>
                <asp:ListItem Value="3">月志</asp:ListItem>
                <asp:ListItem Value="4">季度总结</asp:ListItem>
                <asp:ListItem Value="5">年度总结</asp:ListItem>
            </asp:RadioButtonList>
            院系：<cc1:DropDownTree ID="ddlDepart" runat="server" Width="150" IdKeyName="dep_id" ParentIdKeyName="dep_PatId"
        TaxKeyName="dep_Tax"  AutoPostBack="True" OnSelectedIndexChanged="ddlDepart_SelectedIndexChanged"></cc1:DropDownTree>
            
        </asp:DropDownList>员工：<asp:DropDownList ID="ddlEmp" runat="server" Width="100">
        </asp:DropDownList>
        时间：<asp:TextBox ID="tbStart" runat="server" onfocus="WdatePicker()" Width="80"></asp:TextBox>至
        <asp:TextBox ID="tbEnd" runat="server" onfocus="WdatePicker()" Width="80"></asp:TextBox>   
        <asp:Button ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
    </div></div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        SelectBoxKeyName="SelectBox" ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足条件的日志信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <itemstyle cssclass="center" width="40" />
                <itemtemplate>
<%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="员工名称">
                <itemstyle cssclass="center" width="80" />
                <itemtemplate>
                
<%# Eval("Acc_Name", "{0}")%> 
</itemtemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="类别">
                <itemstyle cssclass="center" width="80" />
                <itemtemplate>
                
<%# Eval("Dlog_Type", "{0}")%> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="工作记录">
                <itemstyle cssclass="left"  />
                <itemtemplate>
                <div class="width:430px;overflow:hidden;white-space:nowrap;">
<%# Eval("Dlog_Note","{0}")%> </div>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="计划">
                <itemstyle cssclass="left" width="30%" />
                <itemtemplate>
                
<%# Eval("Dlog_Plan","{0}")%> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="时间">
                <itemstyle cssclass="center" width="80" />
                <itemtemplate>
                
<%# Eval("Dlog_WrtTime", "{0:yyyy-MM-dd}")%> 
</itemtemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="撰写时间">
                <itemstyle cssclass="center" width="80" />
                <itemtemplate>
                
<%# Eval("Dlog_CrtTime", "{0:yyyy-MM-dd}")%> 
</itemtemplate>
            </asp:TemplateField>
           
          
        </Columns>
    </cc1:GridView>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>