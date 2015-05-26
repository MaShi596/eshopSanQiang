 <%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Supplier_Admin_SearchBindProduct.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier_Admin_SearchBindProduct" Title="无标题页" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
<!--选项卡-->
	<!--选项卡-->

	<div class="dataarea mainwidth databody">
			  <div class="title">
  <em><img src="../../images/01.gif" width="32" height="32" /></em>
  <h1>入库选择商品</h1>
  <span>商品查询，方便添加入库商品</span>
        </div>
		<!--搜索-->

			<!--结束-->
		
		
		<!--数据列表区域-->
	  <div class="datalist">
	  		<div class="searcharea clearfix">
			    <ul>
				    <li><span>商品名称：</span><span><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput"  /></span></li>
				    <li><Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--请选择店铺分类--" /></li>
                    <li><Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" NullToDisplay="--请选择商品品牌--" CssClass="forminput" /></li>
				    <li><asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton"/></li>
			    </ul>
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
                            <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
                        </div>
                    </div>

                    <div class="blank8 clearfix"></div>
			        <div class="batchHandleArea">
				        <ul>
					        <li class="batchHandleButton">
					            <span class="signicon"></span>
					            <span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">全选</a></span>
					            <span class="reverseSelect"><a href="javascript:void(0)" onclick="ReverseSelect()">反选</a></span>
                                <span class="submit_btnxiajia"><a href="javascript:void(0)" onclick="return AddBindProduct()">添加</a></span>
                                           
                            </li>
				        </ul>
			        </div>
			  </div>

            <table>
                <tr class="table_title"><th class="td_right td_left" width="65%">商品名称</th><th class="td_right td_left" width="35%">商品规格</th></tr>
  
            <asp:Repeater ID="rp_bindproduct" runat="server">
            <ItemTemplate>
                <tr>
                    <td>          <div style="float:left; margin-right:10px;">
                                <div style="float:left; margin-right:10px;">
                                <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                        <Hi:ListImage ID="ListImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                 </a> 
                                 </div>
                                 </div>
                                 <div style="float:left;">
                                 <span class="Name"> <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank"><%# Eval("ProductName") %></a></span>
                                  <span class="colorC">商家编码：<%# Eval("ProductCode") %></span>
                                 </div></td>
                    <td>
                        <table>
                        <asp:Repeater ID="rp_sku" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><input name="CheckBoxGroup" type="checkbox" value='<%# Eval("skuid") %>|<%# GetSkuContent(Eval("skuid").ToString())%>|<%# Eval("saleprice") %>|<%# Eval("ProductId") %>|<%# Eval("ThumbnailUrl40")%>|<%# Eval("ProductCode")%>' /><%# GetSkuContent(Eval("skuid").ToString())%></td>
                                    <td>售价：<%# Eval("saleprice","{0:F2}")%></td>
                                    <td>库存：<%# Eval("stock")%></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        </table>
                    </td>
                </tr>
            </ItemTemplate>
            </asp:Repeater>
            </table>
   
      <div class="blank5 clearfix"></div>
	  </div>
	  <!--数据列表底部功能区域-->
	  <div class="bottomPageNumber clearfix">
	  <div class="pageNumber">
					<div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
            </div>
			</div>
		</div>
</div>
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" Runat="Server">
<script language="javascript" type="text/javascript">

    function AddBindProduct() {
        var chks = $("input[name='CheckBoxGroup']:checked");
        if (chks.length <= 0) {
            alert("请选择要入库的商品");
            return false;
        }
        var origin = artDialog.open.origin;
        var total = $(origin.document.getElementById("selectAllNums")).val();
        var tr = '';

        var arr = new Array();
        $(chks).each(function (i, item) {
            arr = $(item).val().split('|');
            var proname = $(item).parent().parent().parent().parent().parent().parent().find("span[class='Name']").text();
            $listparent = $(origin.document.getElementById("addlist"));
            if ($listparent.find("span[id='" + arr[0] + "']").length == 0)
                tr += String.format("<tr name='appendlist'><td><div style='float:left; margin-right:10px;'><a href='../../../ProductDetails.aspx?productId={7}' target='_blank'><img src='{5}' width='40' height='40' /></a></div><div style='float:left;'><span class='Name'><a href='../../../ProductDetails.aspx?productId={7}' target='_blank'>{0}</a></span><br /><span class='colorC'>商家编码:{6}</span></div></td><td>{2}</td><td>{3}</td><td><input onblur='CollectAllNums();' type='text' value='1' name='txtNum'/></td><td style='display:none'>{4}</td><td ><span  id='{1}' style='cursor:pointer;color:blue' onclick='Remove(this)'>删除</span></td></tr>", proname, arr[0], arr[1], arr[2], arr[3] + "|" + arr[0], arr[4], arr[5], arr[3]);
            total++;
        });
        $("#selectAllNums").val(total);
        $listparent.append(tr);
        $listparent = $(origin.document.getElementById("lab_Allcount"));
        $listparent.remove();
        $listparent = $(origin.document.getElementById("d_allnums"));
        $listparent.append("<table id='lab_Allcount' style='float:left;color:Red;'><tr><td>" + total + "</td></tr></table>");
        art.dialog.close();
        // window.parent.GetTotalPrice();
    }

</script>
</asp:Content>