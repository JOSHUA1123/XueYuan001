<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="Accounts.aspx.cs" Inherits="SiteShow.Manage.Sys.Accounts" Title="�˻�����" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager2.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="Accounts_Edit.aspx" AddButtonOpen="true"
            AddButtonVisible="false" WinWidth="640" WinHeight="480" GvName="GridView1" OnDelete="DeleteEvent"
            DelButtonVisible="false" OutputButtonVisible="true" />
         <asp:Panel ID="searchBox" CssClass="searchBox" runat="server">
            <asp:DropDownList ID="ddlOrgin" runat="server">
            </asp:DropDownList>
            �˺ţ�<asp:TextBox ID="tbAccname" runat="server" Width="80" MaxLength="30"></asp:TextBox>&nbsp;
            ������<asp:TextBox ID="tbName" runat="server" Width="80" MaxLength="30"></asp:TextBox>&nbsp;
            �ֻ���<asp:TextBox ID="tbMobitel" runat="server" Width="80" MaxLength="30"></asp:TextBox>&nbsp;
            <asp:Button ID="btnSear" runat="server" Width="100" Text="��ѯ" OnClick="btnsear_Click" />
        </asp:Panel>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="false">
        <EmptyDataTemplate>
            û���κ��˻�
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="���">
                <ItemStyle CssClass="center" Width="40" />
                <ItemTemplate>
                    <%# Container.DataItemIndex   + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server" Visible="true">
                    </cc1:RowDelete>
                    <cc1:RowEdit ID="btnEdit" runat="server"></cc1:RowEdit>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="50px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="�˺�">
                <ItemStyle CssClass="center"/>
                <ItemTemplate>
                    <%# Eval("Ac_Accname")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="����">
                <ItemStyle CssClass="center" Width="80px" />
                <ItemTemplate>
                    <%# Eval("Ac_name")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="�Ա�">
                <ItemStyle CssClass="center" Width="60px" />
                <ItemTemplate>
                    <%# Eval("Ac_Sex", "{0}") == "1" ? "��" : ""%>
                    <%# Eval("Ac_Sex", "{0}") == "2" ? "Ů" : ""%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="&yen; �ʽ�">
                <ItemStyle CssClass="right" />
                <HeaderStyle CssClass="right" Width="80px" />
                <ItemTemplate>
                    <a href="#" onclick="OpenWin('Accounts_Money.aspx?id=<%# Eval("Ac_id") %>','�ʽ���',400,300);return false;"><%# Eval("Ac_money", "{0:C}")%>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="�ֻ���1">
                <ItemStyle CssClass="center" Width="80px" />
                <ItemTemplate>
                    <%# Eval("Ac_MobiTel1")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="�ֻ���2">
                <ItemStyle CssClass="center" Width="80px" />
                <ItemTemplate>
                    <%# Eval("Ac_MobiTel2")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="��ʦ">
                <ItemStyle CssClass="center" Width="80px" />
                <ItemTemplate>
                    <%# Eval("Ac_IsTeacher", "{0}") == "True" ? "��ʦ" : ""%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="����">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# GetOrgin(Eval("Org_ID"))%>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="����/���">
                <ItemStyle CssClass="center" Width="120px" />
                <ItemTemplate>
                    <cc1:StateButton ID="sbUse" OnClick="sbUse_Click" runat="server" TrueText="����" FalseText="����"
                        State='<%# Eval("Ac_IsUse","{0}")=="True"%>'></cc1:StateButton>/<cc1:StateButton ID="sbPass" 
                        OnClick="sbPass_Click" runat="server" TrueText="ͨ��" FalseText="δ��"
                        State='<%# Eval("Ac_IsPass","{0}")=="True"%>'></cc1:StateButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
