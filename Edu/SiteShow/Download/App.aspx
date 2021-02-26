<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="SiteShow.Download" %>
<%
    string file = GetExecFile.File();
    GetExecFile.Write(file); 
      
%>