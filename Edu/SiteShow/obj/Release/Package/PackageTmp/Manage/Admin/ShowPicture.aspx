<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="ShowPicture.aspx.cs" Inherits="SiteShow.Manage.Admin.ShowPicture"
    Title="�ޱ���ҳ" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="ShowPicture_Edit.aspx" GvName="GridView1"
            WinWidth="800" WinHeight="600" IsWinOpen="true" OnDelete="DeleteEvent" />
        <div class="searchBox">
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True">
        <EmptyDataTemplate>
            ��ǰ����û�д����ֻ�ͼƬ
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="����">
                <ItemTemplate><div style="margin-left:5px">
                    <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                    <cc1:RowEdit ID="btnEdit" runat="server"></cc1:RowEdit> </div>
                    <br />
                    <asp:LinkButton ID="lbUp" OnClick="lbUp_Click" runat="server" Enabled='<%# Container.DataItemIndex!=0 %>'>����</asp:LinkButton>
                    <asp:LinkButton ID="lbDown" OnClick="lbDown_Click" runat="server" Enabled='<%# Container.DataItemIndex+1 < ((EntitiesInfo.ShowPicture[])GridView1.DataSource).Length %>'>����</asp:LinkButton>
                    <br />
                    <cc1:StateButton ID="sbShow" OnClick="sbShow_Click" runat="server" TrueText="��ʾ"
                        FalseText="����" State='<%# Eval("Shp_IsShow","{0}")=="True"%>'></cc1:StateButton>
                    
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="60px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ͼƬ">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <img src="<%# Eval("Shp_File")%>" style="max-height: 100px;" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
</asp:Content>
