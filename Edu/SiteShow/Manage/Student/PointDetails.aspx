﻿<%@ Page Language="C#" MasterPageFile="~/Manage/Student/Parents.Master" AutoEventWireup="true"
    CodeBehind="PointDetails.aspx.cs" Inherits="SiteShow.Manage.Student.PointDetails" %>

<%@ MasterType VirtualPath="~/Manage/Student/Parents.Master" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager2.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    <div id="header">
    <span class="toolsBar">当前积分：<asp:Literal ID="ltPointsum" runat="server"></asp:Literal></span>
        <asp:Panel ID="searchBox" CssClass="searchBox" runat="server">
            时间：<asp:TextBox ID="tbStartTime" runat="server" Width="90" onfocus="WdatePicker()"
                CssClass="Wdate" EnableTheming="false"></asp:TextBox>
            -
            <asp:TextBox ID="tbEndTime" runat="server" Width="90" onfocus="WdatePicker()" CssClass="Wdate"
                EnableTheming="false"></asp:TextBox>
            &nbsp;
            <asp:DropDownList ID="ddlType" runat="server" Width="90">
                <asp:ListItem Value="-1">--所有操作--</asp:ListItem>
                <asp:ListItem Value="1">支出</asp:ListItem>
                <asp:ListItem Value="2">增加</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnSear" runat="server" Width="60" Text="查询" OnClick="btnsear_Click" />
        </asp:Panel>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="false">
        <EmptyDataTemplate>
            没有任何满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemTemplate>
                    <%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="40px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="详情">
                <ItemTemplate>
                    <%# Eval("Pa_Type", "{0}") == "1" ? "<span class='type1'>支出</span>" : "<span class='type2'>增加</span>"%>
                    <span>>> </span>
                    <%# Eval("Pa_Value", "{0}")%>
                </ItemTemplate>
                <ItemStyle CssClass="left" Width="120px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="积分">
                <ItemTemplate>
                    <%# Eval("Pa_Total", "{0}")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="60px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="学员">
                <ItemTemplate>
                    <%# GetStudent(Eval("Ac_ID", "{0}"))%>
                </ItemTemplate>
                <ItemStyle CssClass="center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="说明">
                <ItemTemplate>
                    <%# Eval("Pa_Info", "{0}")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="时间">
                <ItemTemplate>
                    <%# Eval("Pa_CrtTime", "{0}")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="160px" />
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="20" OnPageChanged="BindData" />
</asp:Content>
