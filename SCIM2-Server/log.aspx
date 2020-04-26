<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="log.aspx.cs" Inherits="SCIM2_Server.log" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Literal runat="server" ID="llog" />
            <br />
            <asp:Button runat="server" ID="bclear" Text="Clear" OnClick="bclear_Click" />
        </div>
    </form>
</body>
</html>
