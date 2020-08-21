<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="packConfig.aspx.cs" Inherits="JCore.Vue.packConfig" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet nofollow" type="text/css" href="http://nms.tongbu.com/static/res/bootstrap/3.3.5/css/bootstrap.min.css" />
    <script type="text/javascript" src="http://cdn.bootcss.com/jquery/1.11.2/jquery.js"> </script>
    <script type="text/javascript" src="http://nms.tongbu.com/static/res/bootstrap/3.3.5/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container-fluid portlet">
        <div>
            <div>

                <div class="row-fluid" style="margin-top: 10px;">
                    <div style="position: absolute; width: 250px;">
                        <div class="form-group">
                            <label for="txtPackId">ID</label>
                            <input runat="server" type="text" id="txtPackId" class="form-control" readonly="readonly" style="text-align: center;" />
                        </div>

                        <div class="form-group">
                            <label for="txtPackName">名称</label>
                            <input runat="server" type="text" id="txtPackName" class="form-control" />
                        </div>

                        <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="添 加" OnClientClick="return onSave();" OnClick="btnSave_OnClick"/>
                    </div>
                    <div style="padding-left: 250px; min-width: 500px;">
                        <ul id="tabBar" class="nav nav-tabs">
                            <li class="active"><a href="#divSubitem" data-toggle="tab">包括的报表</a></li>
                        </ul>

                        <div class="tab-content" style="width: auto;" id="divSubitem" runat="server" Visible="False">
                            <div class="tab-pane in active">
                                <div id="divSubitemView" class="container" style="margin-top: 10px;margin-left: 10px">
                                    <div class="row-fluid">
                                        <table id="tbSubitem" class="table table-condensed table-hover">
                                            <thead>
                                                <tr>
                                                    <th>ID</th>
                                                    <th>NAME</th>
                                                    <th>GROUPING</th>
                                                    <th>TYPE</th>
                                                    <th>DB</th>
                                                    <th>ITEMNO</th>
                                                    <th>OPTION</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater runat="server" ID="rptSubitem" OnItemCommand="rptSubitem_ItemCommand">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td><%# Eval("Id") %></td>
                                                            <td><%# Eval("Name") %></td>
                                                            <td><%# Eval("Grouping") %></td>
                                                            <td><i class="glyphicon  <%# Convert.ToInt32(Eval("ShowType")) == 1 ? 
                                                                "glyphicon-th-large" : "glyphicon-stats" %>"></i>
                                                            </td>
                                                            <td><%# Eval("DbName") %></td>
                                                            <td><%# Eval("ItemNo") %></td>
                                                            <td>
                                                                <asp:Button ID="btnDel" runat="server" Text="删除" CssClass="btn btn-default" 
                                                                    CommandName="DEL" CommandArgument='<%# Eval("ListId") %>' />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>
                                        
                                        <asp:Button runat="server" ID="btnSearch" CssClass="btn btn-primary" Text="查 询" OnClick="btnSearch_OnClick"/>
                                        <asp:TextBox runat="server" ID="txtSearchName"></asp:TextBox>
                                        
                                        <table id="tbSearchResult" class="table table-condensed table-hover;">
                                            <thead>
                                                <tr>
                                                    <th>ID</th>
                                                    <th>NAME</th>
                                                    <th>GROUPING</th>
                                                    <th>TYPE</th>
                                                    <th>DB</th>
                                                    <th>ITEMNO</th>
                                                    <th>OPTION</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater runat="server" ID="rptSearchResult" OnItemCommand="rptSearchResult_OnItemCommand">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td><%# Eval("Id") %></td>
                                                            <td><%# Eval("Name") %></td>
                                                            <td><%# Eval("Grouping") %></td>
                                                            <td><i class="glyphicon  <%# Convert.ToInt32(Eval("ShowType")) == 1 ? 
                                                                "glyphicon-th-large" : "glyphicon-stats" %>"></i>
                                                            </td>
                                                            <td><%# Eval("DbName") %></td>
                                                            <td><%# Eval("ItemNo") %></td>
                                                            <td>
                                                                <asp:Button ID="btnDel" runat="server" Text="添加" CssClass="btn btn-default"
                                                                    CommandName="ADD" CommandArgument='<%# Eval("Id") %>' />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
    <script>
        function onSave() {
            var name = $("#txtPackName").val().trim();
            if (name === "") {
                alert('请填写名称！');
                return false;
            }
            return true;
        }
    </script>
</body>
</html>