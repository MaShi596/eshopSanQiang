
//***********会员批量购买商品，对商品数量进行验证***********************//
function CheckProductNum(obj){
    var stocks=$(obj).parent().parent()[0].childNodes[5].innerText;
    var pronum=obj.value.replace(/\s/g,"");
    if(pronum==""||parseInt(pronum)<=0){
        obj.value="1";
        return false;
    }
    if(parseInt(pronum)>parseInt(stocks)){
        obj.value=stocks;
        return false;
    }
}