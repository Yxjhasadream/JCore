<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfigTemp.aspx.cs" Inherits="JCore.Vue.ConfigTemp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="text/html; charset=utf-8" http-equiv="content-type" />
    <title>查询配置</title>

    <link href="css/bootstrap-modal-carousel.min.css" rel="stylesheet" />
    <script type="text/javascript" src="http://nms.tongbu.com/static/local/nms.moduler.js"></script>

    <script>
        $$include("_default, jquery.pagination, lodash, clipboard,difflib");
    </script>
    <style type="text/css">
        textarea {
            width: 100% !important;
        }

        div.tab-content {
            width: 100% !important;
        }

        body textarea {
            font-family: "Consolas,PCMyungjo", monospace !important;
        }
    </style>
</head>
<body>
    <form id="form" runat="server">
        <input type="hidden" runat="server" id="txtTabIndex" />

        <div class="container-fluid portlet">
            <div>
                <div>
                    <div class="row-fluid">
                        <div class="pull-right">
                            <span style="color: red;"><strong>管理后台菜单使用此链接地址：</strong></span>
                            <input type="text" id="Url" onclick="select();" readonly="readonly"
                                style="width: 440px; cursor: default; background-color: white; border: none; margin-top: 5px"
                                value='<%=Request.Url.ToString().Substring(0,Request.Url.ToString().LastIndexOf('/')+1)+"CustomQuery.aspx?queryId="+QueryId %>' />
                            <a href="javascript:void(0)" style="margin-right: 15px; margin-left: 15px;" onclick="viewQuery();" class="btn btn-default">开发人员专用预览</a>
                            <asp:Button runat="server" ID="btnQuerySave" Text="保 存" Style="margin-right: 15px;"
                                CssClass="btn btn-primary" OnClick="btnQuerySave_OnClick" OnClientClick="return validate()" />
                        </div>
                    </div>

                    <div class="row-fluid" style="margin-top: 10px;">
                        <div style="position: absolute; width: 220px;">
                            <div class="form-group">
                                <label for="txtQueryId">ID</label>
                                <input runat="server" type="text" id="txtQueryId" class="form-control" readonly="readonly" style="text-align: center;" />
                            </div>

                            <div class="form-group">
                                <label for="txtQueryName">查询名称</label>
                                <input runat="server" type="text" id="txtQueryName" class="form-control" />
                            </div>

                            <div class="form-group">
                                <label for="txtAlias">查询别名 <a title="(如果没有使用到，可以忽略此项，默认与ID值一致，但不能与其他报表的别名重复,否则在保存时会抛出异常。">（说明）</a></label>
                                <input runat="server" type="text" id="txtAlias" class="form-control" />
                            </div>

                            <div class="form-group">
                                <label for="txtQueryName">分组(点击展开)</label>
                                <input runat="server" type="text" id="txtGrouping" class="form-control" list="datalistGroupings" />
                                <datalist id="datalistGroupings">
                                    <asp:Repeater runat="server" ID="rptGroupings">
                                        <ItemTemplate>
                                            <option value="<%# Container.DataItem.ToString() %>" />
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </datalist>
                            </div>

                            <div class="form-group">
                                <label for="selectShowType">报表类型</label>
                                <select runat="server" id="selectShowType" class="form-control">
                                    <option value="1">网格（Grid）</option>
                                    <option value="2">图表（Chart）</option>
                                </select>
                            </div>

                            <div class="form-group">
                                <label for="selectDbType">数据库类型</label>
                                <select runat="server" id="selectDbType" class="form-control">
                                    <option value="1">SQLServer</option>
                                    <option value="2">MySql</option>
                                    <option value="21">MySql(从库)</option>
                                </select>
                            </div>

                            <div class="form-group">
                                <label for="txtDbName">数据库名称</label>
                                <input runat="server" type="text" class="form-control" id="txtDbName" onkeyup="itmeNoChange()" />
                            </div>

                            <div class="form-group">
                                <label for="txtItemNo">权限控制串</label>
                                <input runat="server" type="text" class="form-control" id="txtItemNo" />
                            </div>

                            <div class="form-group">
                                <label for="selectAutoRun">页面载入时自动执行一次查询</label>
                                <select runat="server" id="selectAutoRun" class="form-control">
                                    <option value="1">是</option>
                                    <option value="0">否</option>
                                </select>
                            </div>

                            <div class="form-group" id="selectHasParamAutoRunGroup" style="display: none">
                                <label for="selectHasParamAutoRun">给定参数时自动执行查询</label>
                                <select runat="server" id="selectHasParamAutoRun" class="form-control">
                                    <option value="1">是</option>
                                    <option value="0">否</option>
                                </select>
                            </div>

                            <div class="form-group">
                                <label for="selectEnableExport">启用导出</label>
                                <select runat="server" id="selectEnableExport" class="form-control">
                                    <option value="0">否</option>
                                    <option value="1">是</option>
                                </select>
                            </div>

                            <div class="form-group">
                                <label for="selectShowControl">是否隐藏查询条件栏</label>
                                <select runat="server" id="selectShowControl" class="form-control">
                                    <option value="0">否</option>
                                    <option value="1">是</option>
                                </select>
                            </div>
                        </div>

                        <div style="padding-left: 250px; min-width: 500px;">
                            <ul id="tabBar" class="nav nav-tabs">
                                <li class="active"><a href="#divQuerySql" data-toggle="tab">主查询</a></li>
                                <li><a href="#divTotalSql" data-toggle="tab">总记录数/横坐标</a></li>
                                <li><a href="#divSqlParam" data-toggle="tab">参数</a></li>
                                <li><a href="#divSettings" data-toggle="tab">配置</a></li>
                                <li><a href="#divHelp" data-toggle="tab">帮助</a></li>
                            </ul>

                            <div class="tab-content" style="width: auto;">
                                <div class="tab-pane in active" id="divQuerySql">
                                    <h4 class="pull-left">配置报表的主查询</h4>
                                    <asp:TextBox runat="server" ID="txtQuerySql" TextMode="MultiLine" Rows="32" CssClass="" Style="padding: 5px" />
                                </div>

                                <div class="tab-pane" id="divTotalSql">
                                    <h4 class="pull-left">查询分页记录总数/图表横坐标值</h4>
                                    <asp:TextBox runat="server" ID="txtTotalSql" TextMode="MultiLine" Rows="32" CssClass="" Style="padding: 5px" />
                                </div>

                                <div class="tab-pane" id="divSqlParam">
                                    <div id="divParamControl" class="container" style="margin-top: 10px; margin-left: 0px; width: 100%; display: none;">
                                        <div class="row-fluid">
                                            <h4 class="pull-left">配置SQL参数</h4>
                                            <input type="button" class="btn btn-primary pull-right" id="btnCreateParam" value="新 增" />
                                            <input type="button" class="btn btn-info pull-right" id="btnImportParam" style="margin-right: 10px;" value="导入参数" />
                                            <input type="button" class="btn btn-info pull-right" id="generateAllParamToJSON" style="margin-right: 10px;" value="生成整张报表的参数" />
                                            <input type="button" class="btn btn-info pull-right" id="btnCreateDeclare" style="margin-right: 10px;" value="生成Declare" />
                                        </div>
                                        <div class="row-fluid">
                                            <table id="tbParams" class="table table-condensed table-hover">
                                                <thead>
                                                    <tr>
                                                        <th>ID</th>
                                                        <th>参数名称</th>
                                                        <th>展示名称</th>
                                                        <th>补充内容</th>
                                                        <th>类型</th>
                                                        <th>用户未输入时套用默认值</th>
                                                        <th>用户可见</th>
                                                        <th>日期快捷切换</th>
                                                        <th>启用数组查询</th>
                                                        <th>默认值</th>
                                                        <th>查询下拉列表</th>
                                                        <th>操作</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater runat="server" ID="rptParams">
                                                        <ItemTemplate>
                                                            <tr data-id='<%# Eval("Id") %>'>
                                                                <td class="col-param-id"><%# Eval("Id") %></td>
                                                                <td class="col-param-paramname"><%# Eval("ParamName") %></td>
                                                                <td class="col-param-showname"><%# Eval("ShowName") %></td>
                                                                <td class="col-param-remarks"><%# Eval("Remarks") %></td>
                                                                <td class="col-param-datatype"><%# Eval("DataType").ToString() == "string"? "AnsiString":Eval("DataType") %></td>
                                                                <td class="col-param-allowempty" data-allowempty='<%# Convert.ToBoolean(Eval("AllowEmpty")) ? 1 : 0 %>'>
                                                                    <%# Convert.ToBoolean(Eval("AllowEmpty")) ? "是" : "否" %>
                                                                </td>
                                                                <td class="col-param-visible" data-visible='<%# Convert.ToBoolean(Eval("Visible")) ? 1 : 0 %>'>
                                                                    <%# Convert.ToBoolean(Eval("Visible")) ? "是" : "否" %>
                                                                </td>
                                                                <td class="col-param-enabledayswitch" data-enabledayswitch='<%# Convert.ToBoolean(Eval("EnableDaySwitch")) ? 1 : 0 %>'>
                                                                    <%# Convert.ToBoolean(Eval("EnableDaySwitch")) ? "是" : "否" %>
                                                                </td>
                                                                <td class="col-param-enablearraysearch" data-enablearraysearch='<%# Convert.ToBoolean(Eval("EnableArraySearch")) ? 1 : 0 %>'>
                                                                    <%# Convert.ToBoolean(Eval("EnableArraySearch")) ? "是" : "否" %>
                                                                </td>
                                                                <td class="col-param-defaultvalue"><%# Eval("DefaultValue") %></td>
                                                                <td class="col-param-searchoptions"><%# Eval("SearchOptions") %></td>
                                                                <td style="min-width: 250px">
                                                                    <a href="javascript:void(0)" data-id='<%# Eval("Id") %>' onclick="generateJsonParam(this)" class="btn btn-info btn-xs">生成JSON</a>
                                                                    <a href="javascript:void(0)" data-id='<%# Eval("Id") %>' onclick="onParamEdit(this)" class="btn btn-info btn-xs">编辑</a>
                                                                    <asp:LinkButton runat="server" ID="linkParamMoveUp" Text="上移" class="btn btn-info btn-xs"
                                                                        CommandArgument='<%# Eval("Id") %>' CommandName="moveup" OnCommand="linkParamMoveUp_OnCommand" />
                                                                    <asp:LinkButton runat="server" ID="linkParamMoveDown" Text="下移" class="btn btn-info btn-xs"
                                                                        CommandArgument='<%# Eval("Id") %>' CommandName="movedown" OnCommand="linkParamMoveDown_OnCommand" />
                                                                    <asp:LinkButton runat="server" ID="linkParamDelete" Text="删除" class="btn btn-danger btn-xs"
                                                                        Style="margin-left: 10px;" OnClientClick="return confirm('确认删除？');"
                                                                        CommandArgument='<%# Eval("Id") %>' CommandName="delete" OnCommand="linkParamDelete_OnCommand" />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </div>
                                        <div class="row-fluid">
                                            <span>默认值支持SQL语句查值： <i>${SQL}</i>，如 <i>${SELECT GETDATE()}</i> 为获取当前时间
                                            </span>
                                        </div>
                                    </div>
                                </div>

                                <div class="tab-pane" id="divSettings">
                                    <h4 class="pull-left">通过JS定制页面</h4>
                                    <asp:TextBox runat="server" ID="txtSettings" TextMode="MultiLine" Rows="32" CssClass="" Style="padding: 5px" />
                                </div>

                                <div class="tab-pane" id="divHelp">
                                    <pre>
SQL语句拼接参数：
<b><i>{pageIndex}</i></b> - 当前分页索引，从0开始。
<b><i>{pageSize:N}</i></b> - 分页大小，如 <i>{pageSize:15}</i> 表示一页15条数据，若不指定N，<i>{pageSize}</i> 表示使用内置的默认大小。
<b><i>{pageStart}</i></b> - 分页开始位置，值即为 <i>{pageIndex} * {pageSize}</i>。
** 当查询中使用分页参数时，分页功能将自动启用。

特殊变量取值：
<b><i>{staffName}</i></b> - 打开此报表的当前用户名 <i>{管理后台帐号体系中当前帐号对应的用户名称}</i>。
<b><i>{staffNo}</i></b> - 打开此报表的当前用户工号 <i>{管理后台帐号体系中当前帐号对应的工号。}</i>。

参数调试按钮：
<b><i>Declare</i></b> - 快速生成所有参数的Declare语句。 <i>{对mysql中的数组查询生成的临时表变量暂不支持。}</i>。

SQL扩展特殊语法:
<b><i>${SQL}</i></b> - 引用一个SQL标量查询的值。可用于SQL语句配置中，也可用作SQL参数默认值。
** Mysql查询使用的参数并非真正的数据库参数，不支持Mysql存储过程样式的配置。 
常用demo：
 -SQLSERVER
   -获取日期
     -30天前，(120或20代表格式)格式为 yyyy-mm-dd hh:mi:ss(24h) ：${ SELECT CONVERT(varchar(10), GETDATE() - 30, 120) }
     -7天前，(120或20代表格式)格式为 yyyy-mm-dd hh:mi:ss(24h) ：${ SELECT CONVERT(varchar(10), GETDATE() - 7, 120) }
 -MYSQL
   -获取日期 (MYSQL的日期获取和SQLSERVER不太一样，如果要得到几天后的日期，则把数字改为负的值，关于修改小时数等请自行查阅相关时间函数)
     -30天前：${ select DATE_SUB(now(),INTERVAL 30 DAY) ; }
     -7天前 ：${ select DATE_SUB(now(),INTERVAL 7 DAY) ; }

<b><i>${!~dbtype,dbname~!SQL}</i></b>
在报表系统的参数可以支持跨库进行查询了。
过去的参数查询只能支持在主查询中的数据库中执行SQL语句，
现在通过扩展语法 可以支持指定数据库名称以及数据库的类型

- dbtype 指定数据库的类型，枚举值，和系统内定义的数据库类型一致，可使用索引或名称（忽略大小写）：
    - 1/sqlserver
    - 2/mysql
    - 21/mysqlread
- dbname 数据库名称
以上扩展语法主要针对某些特殊报表的参数需要从非当前主查询数据库中获取的情况。

扩展语法例子：
${ !~ 1 , tbServerWeb ~! SELECT CONVERT(varchar(10), OperationTime, 120)  FROM tb_SiteHistory WHERE Id =2475}

上述查询指定了从sqlsever的tbServerWeb库中执行SQL语句，获取tb_SiteHistory表中的数据。

若使用旧的SQL语法 
${SQL}
${SELECT 1 AS value,'值为1' AS text UNION SELECT 2,'值为2'}
则会被默认在左侧栏中配置的数据库内执行。
                                    </pre>
                                </div>
                            </div>
                            <div style="float: right; display: <%=QueryId==0?"none":"block" %>">
                                <p runat="server" id="createInfo" style="float: left; margin-right: 10px"></p>
                                <a href="CustomQueryHistory.aspx?queryId=<%=QueryId %>" target="_blank">查看历史</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalParamDialog" class="modal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 id="paramDiagTitle" class="modal-title">新增参数</h4>
                    </div>

                    <div class="modal-body">
                        <input type="hidden" runat="server" id="txtParamId" class="form-control" />

                        <div>
                            <label>参数名称（不含@:等字符）</label>
                            <input type="text" runat="server" id="txtParamName" class="form-control" />
                        </div>
                        <div>
                            <label>界面展示名称</label>
                            <input type="text" runat="server" id="txtParamShowName" class="form-control" />
                        </div>
                        <div>
                            <label>补充说明
                                <a title="(此字段有值，则会在参数的界面展示名称后面多出一个角标，鼠标悬停则展示此字段的内容，请用\n作为换行符)">
                                    <i class="glyphicon glyphicon-info-sign"></i>
                                </a>
                            </label>
                            <input type="text" runat="server" id="txtParamRemarks" class="form-control" />
                        </div>
                        <div>
                            <label>类型</label><a href="https://lowleveldesign.org/2013/05/16/be-careful-with-varchars-in-dapper/" target="_blank">AnsiString？</a>
                            <select runat="server" id="selectParamDataType" class="form-control">
                                <option value="string">字符串(AnsiString)</option>
                                <option value="unicodestring">字符串(unicode)</option>
                                <option value="number">数字</option>
                                <option value="datetime">日期与时间</option>
                                <option value="date">日期</option>
                                <option value="month">年月</option>
                            </select>
                        </div>
                        <div class="checkbox">
                            <label>
                                <input type="checkbox" runat="server" id="ckParamAllowEmpty" />用户未输入时套用默认值
                            </label>
                        </div>
                        <div class="checkbox">
                            <label>
                                <input type="checkbox" runat="server" id="ckParamVisible" />用户可见
                            </label>
                        </div>
                        <div class="checkbox">
                            <label>
                                <input type="checkbox" runat="server" id="ckParamEnableArraySearch" />启用数组查询
                            </label>
                        </div>

                        <div class="checkbox">
                            <label>
                                <input type="checkbox" runat="server" id="ckParamEnableDaySwitch" />启用【日期类型】or【年月类型】的快捷切换（如 上一天、下一天、上一月、下一月）
                            </label>
                        </div>

                        <div>
                            <label>默认值 </label>
                            (对于数值和日期类型，空字符串将被转化为DBNull)
                            <textarea id="txtParamDefaultValue" runat="server" cols="15" rows="2" class="form-control"></textarea>
                        </div>

                        <div>
                            <label>查询下拉列表</label>
                            <textarea id="txtSearchOptions" runat="server" cols="15" rows="5" class="form-control"></textarea>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnParamSave" Text="保 存" class="btn btn-primary" OnClick="btnParamSave_OnClick" />
                        <button id="btnParamClose" type="button" class="btn btn-default" data-dismiss="modal">关 闭</button>
                    </div>
                </div>
            </div>
        </div>

        <div id="queryViewDialog" class="modal fade modal-fullscreen" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title">预览</h4>
                    </div>

                    <div class="modal-body" style="height: 85%;">
                        <iframe id="queryViewer" style="border: none; width: 100%; height: 100%;"></iframe>
                    </div>

                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关 闭</button>
                    </div>
                </div>
            </div>
        </div>
        <!-- 生成declare模态窗 -->
        <div id="modalDeclareDialog" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title">Declare</h4>
                    </div>

                    <div class="modal-body" style="height: 50%;">
                        <pre id="declareView"></pre>
                    </div>

                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关 闭</button>
                    </div>
                </div>
            </div>
        </div>
        <!-- 生成参数JSON模态窗 -->
        <div id="modalParamJsonDialog" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title">ParamJson</h4>
                    </div>

                    <div class="modal-body" style="height: 50%;">
                        <pre id="ParamJsonView" style="height: 500px;"></pre>
                    </div>

                    <div class="modal-footer">
                        <button id="copyParam" type="button" class="btn btn-default" data-clipboard-action="copy" data-clipboard-target="#ParamJsonView">复制到剪贴板</button>
                        <button type="button" class="btn btn-default" data-dismiss="modal">关 闭</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- 导入参数JSON模态窗 -->
        <div id="modalImportParamDialog" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title">导入参数Json格式参数</h4>
                    </div>

                    <div class="modal-body" style="height: 50%;">
                        <textarea runat="server" id="txtImportParam" rows="25" cols="25"></textarea>
                    </div>

                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnParamImport" Text="保 存" class="btn btn-primary" OnClick="btnParamImport_OnClick" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关 闭</button>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript">



        var queryId = "<%= QueryId %>";
        var itemValue = $("#txtItemNo").val();

        $(function () {
            if (queryId !== "0") {
                $("#divParamControl").show();
            }

            $("#ddlQueries").val(queryId);

            // tab页切换时记录tab索引.
            $("#tabBar a").each(function (index) {
                var $this = $(this);
                $this.attr("tab-index", index);
                $this.click(function () {
                    $("#txtTabIndex").val($(this).attr("tab-index"));
                });
            });

            // 恢复之前选择的tab
            var tabIndex = $("#txtTabIndex").val();
            if (tabIndex.length > 0) {
                $("#tabBar a[tab-index=" + tabIndex + "]").tab("show");
            }

            // 初始化时，判定是否展示 "给定参数时自动执行查询" 下拉框。
            if ($("#selectAutoRun").val() === "0") {
                $("#selectHasParamAutoRunGroup").css("display", "block");
            }

            // 监听页面加载下拉框。
            $("#selectAutoRun").change(function () {
                // 当前页面下拉框的值均为字符串形式。
                if ($("#selectAutoRun").val() === "0") {
                    $("#selectHasParamAutoRunGroup").css("display", "block");
                    $("#selectHasParamAutoRun").val("1");
                } else {
                    $("#selectHasParamAutoRunGroup").css("display", "none");
                }

            });

            // 绑定生成declare。
            $("#btnCreateDeclare").click(function () {
                var dbType = $("#selectDbType").val();
                var params = { queryId: queryId, dbType: dbType };
                $.ajax({
                    type: 'POST',
                    url: "CustomQueryModules/CustomQueryHandler.ashx?GetDeclare&~format=json",
                    dataType: 'JSON',
                    data: JSON.stringify(params),
                    success: function (data) {
                        if (data.Code !== 0) {
                            alert(data.Message);
                            return;
                        }

                        //  赋值，展示。
                        $("#declareView").html(data.Data);
                        $("#modalDeclareDialog").modal("show");

                    },
                    contentType: 'application/json; charset=UTF-8'
                });
            });

            // 绑定页面上的连接菜单。
            setMenuUrl();

        });

        // 复制json格式化后的参数。
        var clipboard = new ClipboardJS("#copyParam", {
            container: document.getElementById("modalParamJsonDialog")
        });

        for (var i = 0; i < 10; i++) {
            setTimeout(() => {
                console.log(i);
            }, 0);
        }
        // 创建参数模态窗。
        $("#btnCreateParam").click(function () {
            $("#modalParamDialog input[type=text]").each(function () { $(this).val(""); });

            $("#ckParamEnableDaySwitch").prop("checked", false);
            $("#ckParamAllowEmpty").prop("checked", false);
            $("#ckParamVisible").prop("checked", true);
            $("#ckParamEnableArraySearch").prop("checked", false);

            $("#txtParamId").val("0");
            $("#paramDiagTitle").text("新增参数");

            $("#modalParamDialog").modal("show");
        });

        // 打开导入参数模态窗。
        $("#btnImportParam").click(function () {
            $("#txtImportParam").val("");
            $("#modalImportParamDialog").modal("show");
        });

        // 生成参数模态窗。
        function generateJsonParam(sender) {
            var id = $(sender).attr("data-id");
            var tr = $($("#tbParams tr[data-id=" + id + "]")[0]);
            var showName = $(tr.find("td.col-param-showname")[0]).text().trim();
            var remarks = $(tr.find("td.col-param-remarks")[0]).text().trim();
            var paramName = $(tr.find("td.col-param-paramname")[0]).text().trim();
            var defaultValue = $(tr.find("td.col-param-defaultvalue")[0]).text().trim();
            var searchoptions = $(tr.find("td.col-param-searchoptions")[0]).text().trim();
            var dataType = $(tr.find("td.col-param-datatype")[0]).text().trim();
            var allowEmpty = $(tr.find("td.col-param-allowempty")[0]).attr("data-allowempty") === "1" ? true : false;
            var visible = $(tr.find("td.col-param-visible")[0]).attr("data-visible") === "1" ? true : false;
            var enableDaySwitch = $(tr.find("td.col-param-enabledayswitch")[0]).attr("data-enabledayswitch") === "1" ? true : false;
            var enableArraySearch = $(tr.find("td.col-param-enablearraysearch")[0]).attr("data-enablearraysearch") === "1" ? true : false;
            var jsonObj =
                _.zipObject(["showName", "remarks", "paramName", "defaultValue", "searchoptions", "dataType", "allowEmpty",
                    "visible", "enableDaySwitch", "enableArraySearch",]
                    , [showName, remarks, paramName, defaultValue, searchoptions, dataType, allowEmpty, visible, enableDaySwitch, enableArraySearch, ]);
            var paramObj = [jsonObj];
            var data = JSON.stringify(paramObj, null, 2);

            //  赋值，展示。
            $("#ParamJsonView").html(data);
            $("#modalParamJsonDialog").modal("show");
        }

        // 获取整张报表的所有参数。
        $("#generateAllParamToJSON").click(function () {
            var params = { queryId: queryId };
            $.ajax({
                type: "POST",
                url: "CustomQueryModules/CustomQueryHandler.ashx?GetReportJsonParams&~format=json",
                dataType: "JSON",
                data: JSON.stringify(params),
                success: function (data) {
                    if (data.Code !== 0) {
                        alert(data.Message);
                        return;
                    }

                    //  赋值，展示。
                    $("#ParamJsonView").html(JSON.stringify(data.Data, null, 2));
                    $("#modalParamJsonDialog").modal("show");

                },
                contentType: "application/json; charset=UTF-8"
            });
        });

        // 保存导入的JSON格式的参数。
        function saveParam() {
            var param = $("#ParamJsonView").text();
            var obj = JSON.parse(param);
            console.log(obj);
        }


        // 编辑参数。
        function onParamEdit(sender) {
            var id = $(sender).attr("data-id");
            var tr = $($("#tbParams tr[data-id=" + id + "]")[0]);

            var showName = $(tr.find("td.col-param-showname")[0]).text().trim();
            var remarks = $(tr.find("td.col-param-remarks")[0]).text().trim();
            var paramName = $(tr.find("td.col-param-paramname")[0]).text().trim();
            var defaultValue = $(tr.find("td.col-param-defaultvalue")[0]).text().trim();
            var searchoptions = $(tr.find("td.col-param-searchoptions")[0]).text().trim();
            var dataType = $(tr.find("td.col-param-datatype")[0]).text().trim();
            // 这里需要做一个AnsiString的展示上的兼容。
            if (dataType === "AnsiString") {
                dataType = "string";
            }
            var allowEmpty = $(tr.find("td.col-param-allowempty")[0]).attr("data-allowempty");
            var visible = $(tr.find("td.col-param-visible")[0]).attr("data-visible");
            var enableDaySwitch = $(tr.find("td.col-param-enabledayswitch")[0]).attr("data-enabledayswitch");
            var enableArraySearch = $(tr.find("td.col-param-enablearraysearch")[0]).attr("data-enablearraysearch");

            $("#txtParamId").val(id);
            $("#txtParamName").val(paramName);
            $("#txtParamShowName").val(showName);
            $("#txtParamRemarks").val(remarks);
            $("#txtParamDefaultValue").val(defaultValue);
            $("#txtSearchOptions").val(searchoptions);
            $("#selectParamDataType").val(dataType);
            $("#ckParamAllowEmpty").prop("checked", allowEmpty === "1" ? true : false);
            $("#ckParamVisible").prop("checked", visible === "1" ? true : false);
            $("#ckParamEnableDaySwitch").prop("checked", enableDaySwitch === "1" ? true : false);
            $("#ckParamEnableArraySearch").prop("checked", enableArraySearch === "1" ? true : false);

            $("#paramDiagTitle").text("编辑参数");
            $("#modalParamDialog").modal("show");
        }

        function viewQuery() {
            var showGrid = $("#selectShowType").val() === "1";
            var url;
            if (showGrid) {
                url = "CustomQuery.aspx?query=<%= QueryId %>&force=1";
            } else {
                url = "CustomChart.aspx?query=<%= QueryId %>&force=1";
            }
            $("#queryViewDialog").modal("show");
            $("#queryViewer").attr("src", url);
            //window.open(url, "_blank");
        }

        // 绑定页面上的连接菜单。
        function setMenuUrl() {
            var menuUrl = "<%=Request.Url.ToString().Substring(0,Request.Url.ToString().LastIndexOf('/')+1)%>";
            var alias = $("#txtAlias").val();
            var id = $("#txtQueryId").val();
            var showGrid = $("#selectShowType").val() === "1";
            if (showGrid) {
                if (alias !== id) {
                    menuUrl += "CustomQuery.aspx?alias=" + alias;
                } else {
                    menuUrl +="CustomQuery.aspx?queryId=<%= QueryId %>";
                }
            } else {
                if (alias !== id) {
                    menuUrl += "CustomChart.aspx?alias=" + alias;
                } else {
                    menuUrl +="CustomChart.aspx?queryId=<%= QueryId %>";
                }
            }
            $("#Url").val(menuUrl);
        }


        function itmeNoChange() {
            if ($("#txtQueryId").val() !== "") {
                return;
            }
            var dbName = $("#txtDbName").val();
            var value = dbName + "_" + itemValue.split("_")[1] + "_" + itemValue.split("_")[2];
            $("#txtItemNo").val(value);
        }

        function validate() {
            if ($("#txtDbName").val() === "") {
                alert("数据库名称不能为空");
                return false;
            }
            if ($("#txtQueryName").val() === "") {
                alert("查询名称不能为空");
                return false;
            }

            return true;
        }

    </script>
</body>
</html>
