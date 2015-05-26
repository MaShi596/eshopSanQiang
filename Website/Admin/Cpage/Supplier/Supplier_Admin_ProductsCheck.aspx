<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="Supplier_Admin_ProductsCheck.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier_Admin_ProductsCheck" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <!--选项卡-->
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../../images/01.gif" width="32" height="32" /></em>
            <h1 class="title_line">
                供应商商品审核</h1>
            <span class="font">商品审核后，默认在仓库中</span>
        </div>
        <div class="areacolumn clearfix">
            <div class="columnright">
                <div class="formtab Pg_45" runat="server" id="htmlDivNoCheck">
                    <ul>
                        <li><a href="Supplier_Admin_Products.aspx">已审核（<asp:Literal runat="server" ID="litlChecked"></asp:Literal>）</a></li>
                        <li class="visited">待审核（<asp:Literal runat="server" ID="litlNoCheckNum"></asp:Literal>）</li>
                        <li><a href="Supplier_Admin_ProductsErrorRefer.aspx">申请撤销（<asp:Literal runat="server"
                            ID="litlErrorReferNum"></asp:Literal>）</a></li>
                    </ul>
                </div>
                <!--搜索-->
                <div class="searcharea clearfix">
                    <ul>
                        <li><span>商品名称/供应商名称：</span> <span>
                            <asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput" /></span>
                        </li>
                        <li>
                            <abbr class="formselect">
                                <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--请选择店铺分类--" />
                            </abbr>
                        </li>
                        <li>
                            <abbr class="formselect">
                                <Hi:ProductLineDropDownList ID="dropLines" runat="server" NullToDisplay="--请选择产品线--" />
                            </abbr>
                        </li>
                        <li>
                            <abbr class="formselect">
                                <Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" NullToDisplay="--请选择商品品牌--" >
                                </Hi:BrandCategoriesDropDownList>
                            </abbr>
                        </li>
                        <li>
                            <abbr class="formselect">
                                <Hi:ProductTagsDropDownList runat="server" ID="dropTagList" NullToDisplay="--请选择商品标签--" >
                                </Hi:ProductTagsDropDownList>
                            </abbr>
                        </li>
                        <li>
                            <abbr class="formselect">
                                <Hi:ProductTypeDownList ID="dropType" runat="server" NullToDisplay="--请选择商品类型--" />
                            </abbr>
                        </li>
                    </ul>
                </div>
                <div class="searcharea clearfix">
                    <ul>
                        <li><span>商家编码：</span> <span>
                            <asp:TextBox ID="txtSKU" Width="110" runat="server" CssClass="forminput"></asp:TextBox></span></li>
                        <li><span>添加时间：</span></li>
                        <li>
                            <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" />
                            <span class="Pg_1010">至</span>
                            <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" />
                        </li>
                        <li>
                            <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton" /></li>
                    </ul>
                </div>
                <div class="advanceSearchArea clearfix">
                    <!--预留显示高级搜索项区域-->
                </div>
                <!--结束-->
                <div class="functionHandleArea clearfix">
                    <!--分页功能-->
                    <div class="pageHandleArea">
                        <ul>
                            <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" />
                            </li>
                        </ul>
                    </div>
                    <div class="pageNumber">
                        <div class="pagination">
                            <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
                        </div>
                    </div>
                    <!--结束-->
                    <div class="blank8 clearfix">
                    </div>
                    <div class="batchHandleArea">
                        <ul>
                            <li class="batchHandleButton"><span class="signicon"></span><span class="allSelect">
                                <a href="javascript:void(0)" onclick="SelectAll()">全选</a></span> <span class="reverseSelect">
                                    <a href="javascript:void(0)" onclick="ReverseSelect()">反选</a></span> <span class="delete"
                                        id="htmlSpanbtnDelete" runat="server">
                                        <Hi:ImageLinkButton ID="btnDelete" runat="server" Text="删除" IsShow="true" DeleteMsg="确定要把商品移入回收站吗？" /></span>
                                <span class="downproduct" style="display: none;">
                                    <asp:LinkButton runat="server" ID="btnUpShelf" Text="下架" /></span> <span class="downproduct"
                                        id="htmlSpanbtnCheck" runat="server"><a href="javascript:void(0)" onclick="CheckPass()">
                                            审核通过</a></span> <span class="downproduct"><a href="javascript:void(0)" onclick="CheckError()">
                                                审核失败</a></span> <span class="submit_btnxiajia" style="display: none;"><a href="javascript:void(0)"
                                                    onclick="javascript:PenetrationStatus();">下架</a></span> <span class="downproduct"
                                                        style="display: none;"><a href="javascript:void(0)" onclick="EditBaseInfo()">调整基本信息</a></span>
                                <span class="printorder" style="display: none;"><a href="javascript:void(0)" onclick="EditSaleCounts()">
                                    调整显示销售数量</a></span> <span class="sendproducts" style="display: none;"><a href="javascript:void(0)"
                                        onclick="EditStocks()">调整库存</a></span> <span class="printorder" style="display: none;">
                                            <a href="javascript:void(0)" onclick="EditMemberPrices()">调整会员零售价</a></span>
                                <span class="printorder" style="display: none;"><a href="javascript:void(0)" onclick="EditDistributorPrices()">
                                    调整分销商采购价</a></span> <span class="printorder" style="display: none;"><a href="javascript:void(0)"
                                        onclick="EditProdcutTag()">调整商品标签</a></span> <span class="printorder" style="display: ;">
                                            <a href="javascript:void(0)" onclick="RemarkOrder();">调整价格模板</a></span>
                            </li>
                        </ul>
                    </div>
                </div>
                <!--数据列表区域-->
                <div class="datalist" style="margin: 5px 0px 0px 0px;">
                    <UI:Grid runat="server" ID="grdProducts" Width="100%" AllowSorting="true" ShowOrderIcons="true"
                        GridLines="None" DataKeyNames="ProductId" SortOrder="Desc" AutoGenerateColumns="false"
                        HeaderStyle-CssClass="table_title">
                        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="选择" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ProductId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="排序" DataField="DisplaySequence" ItemStyle-Width="40px"
                                HeaderStyle-CssClass="td_right td_left" />
                            <asp:TemplateField HeaderText="供应商" ItemStyle-Width="120px" HeaderStyle-CssClass=" td_left td_right_fff">
                                <ItemTemplate>
                                    <%#Eval("SupplierName") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作" ItemStyle-Width="12%" HeaderStyle-CssClass=" td_left td_right_fff">
                                <ItemTemplate>
                                    <span class="submit_bianji"><a onclick='<%# "showWindow_Page(" + Eval("ProductId") + ")" %>'
                                        style="cursor: pointer;">查看</a></span> <span class="submit_shanchu">
                                            <Hi:ImageLinkButton ID="btnDelete" CommandName="Delete" runat="server" Text="删除"
                                                IsShow="true" DeleteMsg="确定要把商品移入回收站吗？" /></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="商品" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <div style="float: left; margin-right: 10px;">
                                        <a href='<%#"../../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                            <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl40" />
                                        </a>
                                    </div>
                                    <div style="float: left;">
                                        <span class="Name"><a href='<%#"../../../ProductDetails.aspx?productId="+Eval("ProductId")%>'
                                            target="_blank">
                                            <%# Eval("ProductName") %></a></span> <span class="colorC">商家编码：<%# Eval("ProductCode") %></span>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="库存" ItemStyle-Width="100" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Label ID="lblStock" runat="server" Text='<%# Eval("Stock") %>' Width="25"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <Hi:MoneyColumnForAdmin HeaderText=" 供货价" ItemStyle-Width="80" DataField="costPrice"
                                HeaderStyle-CssClass="td_right td_left" />
                            <Hi:MoneyColumnForAdmin HeaderText=" 市场价" ItemStyle-Width="80" DataField="MarketPrice"
                                HeaderStyle-CssClass="td_right td_left" />
                            <Hi:MoneyColumnForAdmin HeaderText="一口价" ItemStyle-Width="80" DataField="SalePrice"
                                HeaderStyle-CssClass="td_right td_left" />
                            <Hi:MoneyColumnForAdmin HeaderText="采购价" ItemStyle-Width="80" DataField="PurchasePrice"
                                HeaderStyle-CssClass="td_right td_left" />
                            <asp:TemplateField HeaderText="状态" ItemStyle-Width="12%" HeaderStyle-CssClass=" td_left td_right_fff">
                                <ItemTemplate>
                                    <span style='<%# Eval("CheckStatus").ToString()=="2"?"color:red;": "" %>'>
                                        <%# Eval("CheckRemark") %></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </UI:Grid>
                    <div class="blank12 clearfix">
                    </div>
                </div>
                <div class="bottomPageNumber clearfix">
                    <div class="pageNumber">
                        <div class="pagination">
                            <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%-- 下架商品--%>
    <div id="UnSaleProduct" style="display: none;">
        <div class="frame-content">
            同时撤销铺货：
            <asp:CheckBox ID="chkDeleteImage" Text="撤销铺货" Checked="true" runat="server" onclick="javascript:SetPenetrationStatus(this)" />
            <p>
                <em>当选择撤销铺货时，所有子站关于此商品信息以及促销活动信息都将被删除</em></p>
        </div>
    </div>
    <%-- 商品标签--%>
    <div id="TagsProduct" style="display: none;">
        <div class="frame-content">
            <Hi:ProductTagsLiteral ID="litralProductTag" runat="server"></Hi:ProductTagsLiteral>
        </div>
    </div>
    <%-- 取消通过--%>
    <div id="Window_CheckError" style="display: none;">
        <div class="frame-content">
            <asp:TextBox runat="server" ID="txtCheckError" TextMode="MultiLine" Height="50" Width="200"></asp:TextBox>
        </div>
    </div>
    <%-- 审核通过--%>
    <div id="Window_CheckPass" style="display: none;">
        <div class="frame-content">
            <div style="color: Red;">
                注:默认是在仓库中</div>
            <input type="checkbox" onclick="CheckPassChecked(this)">上架出售
        </div>
    </div>
    <div style="display: none">
        <input type="hidden" id="hdPenetrationStatus" value="1" runat="server" />
        <Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>
        <asp:Button ID="btnUpdateProductTags" runat="server" Text="商品标签" CssClass="submit_DAqueding" />
        <asp:Button ID="btnOK" runat="server" Text="下架商品" CssClass="submit_DAqueding" />
        <asp:Button ID="btnCheckError" runat="server" Text="审核失败" CssClass="submit_DAqueding" />
        <Hi:TrimTextBox runat="server" ID="txtCheckPass" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>
        <asp:Button ID="btnCheck" runat="server" Text="审核通过" CssClass="submit_DAqueding" />
    </div>

    <!--供应商价格模板--->
<div id="RemarkOrder"  style="display:none;">
        <div class="datalist">
    <UI:Grid ID="grdMemberRankList" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="Auto" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
              <Columns>
              <asp:TemplateField HeaderText="选择" HeaderStyle-Width="100px" HeaderStyle-CssClass="td_right td_left">
                        <ItemStyle CssClass="spanD spanN" />
                        <ItemTemplate>
                             <span class="submit_bianji">
                                 <input type="radio" name="rad_ht" value='<%#DataBinder.Eval(Container.DataItem,"Auto")%>' onclick="SelPtemplate(this)"/></span>                             
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="模板名称" ItemStyle-Width="200px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <%#Eval("Name") %>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="商品一口价" ItemStyle-Width="120px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           供货价*<%# ((decimal)Eval("SalePrice")).ToString("f2") %>%
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="商品分销商采购价" ItemStyle-Width="120px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           供货价*<%# ((decimal)Eval("PurchasePrice")).ToString("f2")%>%
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="分销商最低零售价" ItemStyle-Width="120px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           供货价*<%# ((decimal)Eval("LowestSalePrice")).ToString("f2")%>%
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="备注" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <%#Eval("remark") %>
                        </ItemTemplate>
                  </asp:TemplateField>
              </Columns>
            </UI:Grid>	   
    </div>
   
</div>

    <div style="display: none">
        <asp:Button runat="server" ID="btnRemark" Text="确定" CssClass="submit_DAqueding" />
    </div>
    <input type="hidden" id="hid_Auto" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

<script type="text/javascript">
    //价格模板信息
    var formtype = "";
    function RemarkOrder() {
        arrytext = null;
        formtype = "transfer";
        setArryText('ctl00_contentHolder_ddl_UserIdList', "");
        DialogShow("选择供应商价格模板", 'uptransfer', 'RemarkOrder', 'ctl00_contentHolder_btnRemark');
    }
    //验证
    function validatorForm() {
        switch (formtype) {
            case "transfer":
                break;
        };
        return true;
    }
    function SelPtemplate(selobj) {
        if (selobj.value) {
            var s_auto = selobj.value;
            $("#ctl00_contentHolder_hid_Auto").val(s_auto);
        }
    }

    //循环更新价格
    function EditP_pricetemplate() {
        var intGridviewRowCount = $(".SpecificationTr").length;
        alert(intGridviewRowCount);
        var salPrice = document.getElementById("ctl00_contentHolder_txtSalePrice");
        var PurchasePrice = document.getElementById("ctl00_contentHolder_txtPurchasePrice");

        if (intGridviewRowCount <= 0) { return; }

        for (var i = 1; i < intGridviewRowCount + 1; i++) {
            document.getElementById("salePrice_" + i).value = salPrice.value;
            document.getElementById("purchasePrice_" + i).value = PurchasePrice.value;
        }
    }
</script>

    <script type="text/javascript">
        var formtype = "";
        function EditStocks() {
            var productIds = GetProductId();
            if (productIds.length > 0)
                DialogFrame("Cpage/Supplier/SupplierAdmin/Supplier_ProductStockEdit.aspx?ProductIds=" + productIds, "调整库存", null, null);
        }

        function EditBaseInfo() {
            var productIds = GetProductId();
            if (productIds.length > 0)
                DialogFrame("Cpage/Supplier/SupplierAdmin/Supplier_ProductBaseInfoEdit.aspx?ProductIds=" + productIds, "调整商品基本信息", null, null);
        }

        function EditSaleCounts() {
            var productIds = GetProductId();
            if (productIds.length > 0)
                DialogFrame("product/EditSaleCounts.aspx?ProductIds=" + productIds, "调整前台显示的销售数量", null, null);
        }

        function EditMemberPrices() {
            var productIds = GetProductId();
            if (productIds.length > 0)
                DialogFrame("Cpage/Supplier/SupplierAdmin/Supplier_ProductMemberPricesEdit.aspx?ProductIds=" + productIds, "调整会员零售价", null, null);
        }

        function EditDistributorPrices() {
            var productIds = GetProductId();
            if (productIds.length > 0)
                DialogFrame("Cpage/Supplier/SupplierAdmin/Supplier_ProductDistributorPricesEdit.aspx?ProductIds=" + productIds, "调整分销商采购价", null, null);
        }

        function GetProductId() {
            var v_str = "";

            $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
                v_str += $(rowItem).attr("value") + ",";
            });

            if (v_str.length == 0) {
                alert("请选择商品");
                return "";
            }
            return v_str.substring(0, v_str.length - 1);
        }

        function SetPenetrationStatus(checkobj) {
            if (checkobj.checked) {
                $("#ctl00_contentHolder_hdPenetrationStatus").val("1");
            } else {
                $("#ctl00_contentHolder_hdPenetrationStatus").val("0");
            }
        }

        //下架
        function PenetrationStatus() {
            formtype = "unsale";
            arrytext = null;
            var productIds = GetProductId();
            if (productIds.length > 0) {
                DialogShow("商品下架", "productunsale", "UnSaleProduct", "ctl00_contentHolder_btnOK");
            }
        }

        function EditProdcutTag() {
            var productIds = GetProductId();
            if (productIds.length > 0) {
                formtype = "tag";
                arrytext = null;
                setArryText('ctl00_contentHolder_txtProductTag', "");
                DialogShow("商品标签", "producttag", "TagsProduct", "ctl00_contentHolder_btnUpdateProductTags");
            }
        }


        function validatorForm() {
            switch (formtype) {
                case "tag":
                    if ($("#ctl00_contentHolder_txtProductTag").val().replace(/\s/g, "") == "") {
                        alert("请选择商品标签");
                        return false;
                    }
                    break;
                case "unsale":
                    if (!confirm("确定要下架选定的商品吗？")) {
                        return false;
                    }
                    setArryText('ctl00_contentHolder_hdPenetrationStatus', $("#ctl00_contentHolder_hdPenetrationStatus").val());

                    break;
                case "checkpass":
                    //                if (!confirm("确定要通过吗？")) {
                    //                    return false;
                    //                }

                    break;
            };
            return true;
        }

        function showWindow_Page(auto) {
            var currentDate = new Date();
            DialogFrame("Cpage/Supplier/SupplierAdmin/Supplier_Admin_EditProduct.aspx?productId=" + auto + "&t=" + currentDate.getTime(), "编辑商品", null, null);
            window.parent.FramLinkToSet(location.href);
        }

        function CheckError() {
            formtype = "checkerror";
            arrytext = null;
            var productIds = GetProductId();
            if (productIds.length > 0) {
                formtype = "checkerror";
                arrytext = null;
                setArryText('ctl00_contentHolder_txtCheckError', "");
                DialogShow("审核失败", "window_checkerror", "Window_CheckError", "ctl00_contentHolder_btnCheckError");
            }
        }

        function CheckPass() {
            formtype = "checkpass";
            arrytext = null;
            var productIds = GetProductId();
            if (productIds.length > 0) {
                formtype = "checkpass";
                arrytext = null;
                setArryText('ctl00_contentHolder_txtCheckPass', "");
                DialogShow("审核通过", "window_checkpass", "Window_CheckPass", "ctl00_contentHolder_btnCheck");
            }
        }

        function CheckPassChecked(obj) {
            $("#ctl00_contentHolder_txtCheckPass").val("0");
            if ($(obj).attr("checked") != undefined)
                $("#ctl00_contentHolder_txtCheckPass").val("1");
        }
    </script>
</asp:Content>
    