<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Supplier_Products.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier_Products" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	<!--选项卡-->
	<div class="dataarea mainwidth td_top_ccc">
        <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="formtab Pg_45" runat="server" id="htmlDivChecked">
                <ul>
                    <li class="visited">已审核（<asp:Literal runat="server" ID="litlChecked"></asp:Literal>）</li>                                      
                    <li><a href="Supplier_Products.aspx?checkedstatus=1">仓库中（<asp:Literal runat="server" ID="litlNoCheckNum"></asp:Literal>）</a></li>
                </ul>
            </div>
            <div class="formtab Pg_45" runat="server" id="htmlDivNoCheck">
                <ul>
                    <li ><a href="Supplier_Products.aspx?checkedstatus=0">已审核（<asp:Literal runat="server" ID="litlChecked2"></asp:Literal>）</a></li>                                      
                    <li class="visited">仓库中（<asp:Literal runat="server" ID="litlNoCheckNum2"></asp:Literal>）</li>
                </ul>
            </div>
		<!--搜索-->
		<div class="searcharea clearfix">
			<ul>
				<li><span>商品名称：</span>
				    <span><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput"  /></span>
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
						<Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" NullToDisplay="--请选择商品品牌--" ></Hi:BrandCategoriesDropDownList>
					</abbr>
				</li>
				          <li style="display:none;"><abbr class="formselect">
						<Hi:ProductTagsDropDownList runat="server" ID="dropTagList" NullToDisplay="--请选择商品标签--" ></Hi:ProductTagsDropDownList>
					</abbr></li>
                 <li>
					<abbr class="formselect">
						<Hi:ProductTypeDownList ID="dropType" runat="server" NullToDisplay="--请选择商品类型--" />
					</abbr>
				</li>    
				</ul>
		</div>
		<div class="searcharea clearfix">
		    <ul>
		     <li><span>商家编码：</span>
                    <span><asp:TextBox ID="txtSKU" Width="110" runat="server" CssClass="forminput"></asp:TextBox></span></li>
		        <li><span>添加时间：</span></li>
		        <li>
		            <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" cssclass="forminput" />
		            <span class="Pg_1010">至</span>
		            <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" cssclass="forminput" />
		        </li>
		        <li><asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton"/></li>
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
					<li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
				</ul>
			</div>
			<div class="pageNumber">
				<div class="pagination">
                <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
            </div>
			</div>
			<!--结束-->

			<div class="blank8 clearfix"></div>
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
					    <span class="signicon"></span>
					    <span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">全选</a></span>
					    <span class="reverseSelect"><a href="javascript:void(0)" onclick="ReverseSelect()">反选</a></span>
                        <span class="delete" id="htmlSpanbtnDelete" runat="server"><Hi:ImageLinkButton ID="btnDelete" runat="server" Text="删除" IsShow="true" DeleteMsg="确定要把商品移入回收站吗？" /></span>
                        <span class="downproduct" style="display:none;"><asp:LinkButton runat="server" ID="btnUpShelf" Text="提交审核" /></span>
                        <span class="downproduct" id="htmlSpanbtnError" runat="server"><Hi:ImageLinkButton ID="btnError" IsShow="true" DeleteMsg="确定要申请撤销？" runat="server" Text="申请撤销" /></span>
                        <span class="downproduct" id="htmlSpanbtnCheck" runat="server"><Hi:ImageLinkButton ID="btnCheck" runat="server" Text="提交审核" /></span>
                        <span class="submit_btnxiajia" style="display:none;"><a href="javascript:void(0)" onclick="javascript:PenetrationStatus();">下架</a></span>
                        <span class="downproduct" id="htmlSpanEditBaseInfo" runat="server"><a href="javascript:void(0)" onclick="EditBaseInfo()">调整基本信息</a></span>  
                        <span class="printorder" style="display:none;"><a href="javascript:void(0)" onclick="EditSaleCounts()">调整显示销售数量</a></span>
                        <span class="sendproducts" ><a href="javascript:void(0)" onclick="EditStocks()">调整库存</a></span>                              
                        <span class="printorder" id="htmlSpanEditMemberPrices" runat="server" style="display:none"><a href="javascript:void(0)" onclick="EditMemberPrices()">调整会员零售价</a></span>    
                        <span class="printorder" id="htmlSpanEditDistributorPrices" runat="server" style="display:none"><a href="javascript:void(0)" onclick="EditDistributorPrices()">调整分销商采购价</a></span> 
                        <span class="printorder" style="display:none;"><a href="javascript:void(0)" onclick="EditProdcutTag()">调整商品标签</a></span>  
                    </li>
				</ul>
			</div>
		</div>		
		<!--数据列表区域-->
	    <div class="datalist">
	    <UI:Grid runat="server" ID="grdProducts" Width="100%" AllowSorting="true" ShowOrderIcons="true" GridLines="None" DataKeyNames="ProductId"
                    SortOrder="Desc"  AutoGenerateColumns="false" HeaderStyle-CssClass="table_title">
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="30px" HeaderText="选择" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ProductId") %>' />
                            </itemtemplate>
                        </asp:TemplateField>     
                        <asp:TemplateField HeaderText="审核状态" ItemStyle-Width="150px" HeaderStyle-CssClass=" td_left td_right_fff">
                            <ItemTemplate>
                                <span style='<%# Eval("CheckStatus").ToString()=="2"?"color:red;":"" %>'><%# Eval("CheckRemark") %></span>
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="操作" ItemStyle-Width="12%" HeaderStyle-CssClass=" td_left td_right_fff" >
                            <ItemTemplate>
                                <div style='<%# Eval("CheckStatus").ToString()=="3"?"display:none":"" %>'>
                                    <span class="submit_bianji"><a onclick='<%# "showWindow_Page(" + Eval("ProductId") + ")" %>' style="cursor:pointer;" >编辑</a></span>
			                        <span class="submit_shanchu"><Hi:ImageLinkButton ID="btnDelete" CommandName="Delete" runat="server" Text="删除" IsShow="true" DeleteMsg="确定要把商品移入回收站吗？" /></span>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>                         
                        <asp:BoundField HeaderText="排序" DataField="DisplaySequence" ItemStyle-Width="40px" HeaderStyle-CssClass="td_right td_left" />
                        <asp:TemplateField ItemStyle-Width="35%" HeaderText="商品" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                            <div style="float:left; margin-right:10px;">
                                <a href='<%#  "../../../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                        <Hi:ListImage ID="ListImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                 </a> 
                                 </div>
                                 <div style="float:left;">
                                 <span class="Name"> <a href='<%#"../../../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank"><%# Eval("ProductName") %></a></span>
                                  <span class="colorC">商家编码：<%# Eval("ProductCode") %></span>
                                 </div>
                         </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="库存"  ItemStyle-Width="100" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                             <asp:Label ID="lblStock" runat="server" Text='<%# Eval("Stock") %>' Width="25"></asp:Label>
                          </itemtemplate>
                        </asp:TemplateField>
                        <Hi:MoneyColumnForAdmin HeaderText=" 市场价" ItemStyle-Width="80"  DataField="MarketPrice" HeaderStyle-CssClass="td_right td_left"  />      
                        <Hi:MoneyColumnForAdmin HeaderText="供货价" ItemStyle-Width="80"  DataField="CostPrice" HeaderStyle-CssClass="td_right td_left"  />                        
                    </Columns>
                </UI:Grid>
		  <div class="blank12 clearfix"></div>
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
    同时撤销铺货：  <asp:CheckBox ID="chkDeleteImage" Text="撤销铺货" Checked="true" runat="server" onclick="javascript:SetPenetrationStatus(this)" />
    <p><em>当选择撤销铺货时，所有子站关于此商品信息以及促销活动信息都将被删除</em></p>
    </div>
</div>

<%-- 商品标签--%>
<div  id="TagsProduct" style="display: none;">
    <div class="frame-content">
    <Hi:ProductTagsLiteral ID="litralProductTag" runat="server"></Hi:ProductTagsLiteral>
    </div>
</div>

<div style="display:none">
    <input type="hidden" id="hdPenetrationStatus" value="1" runat="server" />
     <Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine" style="display:none;"></Hi:TrimTextBox>
    <asp:Button ID="btnUpdateProductTags" runat="server" Text="商品标签" CssClass="submit_DAqueding" />

     <asp:Button ID="btnOK" runat="server" Text="下架商品" CssClass="submit_DAqueding"/>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" >
    var formtype = "";
    function EditStocks(){    
        var productIds = GetProductId();
        if (productIds.length > 0)
            DialogFrame("../admin/Cpage/Supplier/SupplierAdmin/Supplier_ProductStockEdit.aspx?ProductIds=" + productIds, "调整库存", null, null);
    }

    function EditBaseInfo() {
        var productIds = GetProductId();
        if(productIds.length > 0)
            DialogFrame("../admin/Cpage/Supplier/SupplierAdmin/Supplier_ProductBaseInfoEdit.aspx?ProductIds=" + productIds, "调整商品基本信息", null, null);
    }

    function EditSaleCounts() {
        var productIds = GetProductId();
        if (productIds.length > 0)
            DialogFrame("product/EditSaleCounts.aspx?ProductIds=" + productIds, "调整前台显示的销售数量", null, null);
    }    
    
    function EditMemberPrices(){
        var productIds = GetProductId();
        if(productIds.length > 0)
            DialogFrame("../admin/Cpage/Supplier/SupplierAdmin/Supplier_ProductMemberPricesEdit.aspx?ProductIds=" + productIds, "调整会员零售价", null, null);
    }    
    
    function EditDistributorPrices(){
        var productIds = GetProductId();
        if(productIds.length > 0)
            DialogFrame("../admin/Cpage/Supplier/SupplierAdmin/Supplier_ProductDistributorPricesEdit.aspx?ProductIds=" + productIds, "调整分销商采购价", null, null);
    }
    
    function GetProductId(){
        var v_str = "";

        $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function(rowIndex, rowItem){
            v_str += $(rowItem).attr("value") + ",";
        });
        
        if(v_str.length == 0){
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
        };
        return true;
    }

    function showWindow_Page(auto) {
        var currentDate = new Date();
        DialogFrame("../admin/Cpage/Supplier/SupplierAdmin/Supplier_ProductEdit.aspx?productId=" + auto + "&t=" + currentDate.getTime(), "编辑商品", 1000, 600);
        window.parent.FramLinkToSet(location.href);
    }

</script>
</asp:Content>