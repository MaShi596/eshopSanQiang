// 验证是否是邮编
function isZip(_ID ,_Desc,strMin,strMax)
{	
   var rexTel=/^[0-9]+$/;
   var _obj = $('#'+_ID);
    if ( _obj == null ) return true;   
    var _str = _obj.val();	
	if(_str !='')
	{
	   if((_str.length<strMin) || (_str.length>strMax))
	   {
		VildateMsgShow(_Desc ,_ID);_obj.focus();return false;
	   }
	   /*else
	   {
		var rexTel=/^[0-9]+$/;
		if(!rexTel.test(_str))
		{
		 VildateMsgShow(_Desc ,_ID);_obj.focus();return false;
		}
	   }*/
	}
	return true;
}

/*正则表达式验证*/
function V_Expression(_ID,_patn,_Desc){
	var _obj = $('#'+_ID);
	var _strValue = _obj.val();		
	var matchArray=_strValue.match(_patn);
	if (matchArray==null) { VildateMsgShow(_Desc,_ID); _obj.focus();return false;}	
	return true;
}

//是否为空
function V_Empty(_ID ,_Desc,strMin,strMax)
{
	var _obj = $('#'+_ID);
    if ( _obj == null ) return true;   
    var _str = _obj.val();
    if ( _str ==''){VildateMsgShow(_Desc ,_ID);_obj.focus();return false;}
	else if((_str.length<strMin)||(_str.length>strMax)){VildateMsgShow(_Desc ,_ID);_obj.focus();return false;}
   return true;
}

//是否已选择下拉列表框的项
function V_zero( _ID ,_Desc )
{
    var _obj = $('#'+_ID);
    var _str = _obj.val();
    if ( _str =='' ) {VildateMsgShow(_Desc,_ID);_obj.focus();return false;}
    if ( _str =='0' ) {VildateMsgShow(_Desc,_ID);_obj.focus();return false;}
    return true;
}
//是否为money型
function V_Price( _ID ,_Desc )
{			
	var _obj = $('#'+_ID);
	if ( _obj == null ) return true;
	if ( _obj.val() == '' ) { VildateMsgShow("请输入"+_Desc+"！",_ID); _obj.focus(); return false;}
	if( !isNumber( _obj.val() ) ){VildateMsgShow("提示："+_Desc+"格式错误！必须数数字" ,_ID);_obj.focus(); return false;}
	
	return true;
}
//是否为email格式
function V_Email(_ID,_Desc)
{
	var _obj = $('#'+_ID);
	var _strValue = $.trim(_obj.val());
	if (_strValue==""){ VildateMsgShow(_Desc,_ID); _obj.focus();return false;}
	var patn = /^[_a-zA-Z0-9\-]+(\.[_a-zA-Z0-9\-]*)*@[a-zA-Z0-9\-]+([\.][a-zA-Z0-9\-]+)+$/;
	var matchArray=_strValue.match(patn);
	if (matchArray==null){ VildateMsgShow(_Desc,_ID); _obj.focus();return false;}
	return true;
}
/*电话号码区号*/
function V_TelQH(_TelID,_Desc){
	var _obj = $('#'+_TelID);
	var _strValue = _obj.val();
	if ( _strValue == "" ) { VildateMsgShow("请输入"+_Desc+"！",_ID); _obj.focus();return false;}
	var patn = /^\d{3,4}$/;
	var matchArray=_strValue.match(patn);
	if (matchArray==null) { VildateMsgShow("您输入的"+_Desc+"格式不正确，请重新输入。",_ID); _obj.focus();return false;}	
	return true;
} 
/*电话号码区号*/
function V_Tel(_TelID,_Desc){
	var _obj = $('#'+_TelID);
	var _strValue = _obj.val();
	if ( _strValue == "" ) { VildateMsgShow("请输入"+_Desc+"！",_ID); _obj.focus();return false;}
	var patn = /^\d{6,13}$/;
	var matchArray=_strValue.match(patn);
	if (matchArray==null) { VildateMsgShow("您输入的"+_Desc+"格式不正确，请重新输入。",_ID); _obj.focus();return false;}	
	return true;
} 
var __Win__IsAutoClose = true;
var __Win__IsFilterClose = true;
var __Win__CloseWaitTime = 3000;
var __Win__BlockFlag = false; 
//关闭气泡提示
function HideWinErrMsgTips(elementid)
{
    var ua = navigator.userAgent.toLowerCase();
    var isOpera = (ua.indexOf('opera') != -1);
    var isIE = (ua.indexOf('msie') != -1 && !isOpera);
    var objWinDiv = document.getElementById(elementid);
    if(isIE&&typeof(__Win__IsFilterClose)!="undefined"&&__Win__IsFilterClose)
    {
       __Win__BlockFlag = false;
       HideIEWinErrMsgTips(objWinDiv.id);
    }
    else
    {
	    objWinDiv.style.visibility = "hidden";
	}
}
function HideIEWinErrMsgTips(elementid)
{
     var obj___ = document.getElementById(elementid+"____")
     var  opacty=obj___.filters.alpha.opacity;   
     obj___.filters.alpha.opacity-=9;
     if(obj___.filters.alpha.opacity>0)
     {
        if(!__Win__BlockFlag)
            setTimeout("HideIEWinErrMsgTips('"+elementid+"')",100);
     }
     else
     {
        document.getElementById(elementid).style.visibility = "hidden";
     }   
}

//得到某obj的x,y坐标,兼容大部分的浏览器
function getWinElementPos(obj)
{

 var ua = navigator.userAgent.toLowerCase();
 var isOpera = (ua.indexOf('opera') != -1);
 var isIE = (ua.indexOf('msie') != -1 && !isOpera); // not opera spoof

 var el = obj;

 if(el.parentNode === null || el.style.display == 'none') 
 {
  return false;
 }

 var parent = null;
 var pos = [];
 var box;

 if(el.getBoundingClientRect) //IE
 {
  box = el.getBoundingClientRect();
  var scrollTop = Math.max(document.documentElement.scrollTop, document.body.scrollTop);
  var scrollLeft = Math.max(document.documentElement.scrollLeft, document.body.scrollLeft);

  return {x:box.left + scrollLeft, y:box.top + scrollTop};
 }
 else if(document.getBoxObjectFor) // gecko
 {
  box = document.getBoxObjectFor(el);
     
  var borderLeft = (el.style.borderLeftWidth)?parseInt(el.style.borderLeftWidth):0;
  var borderTop = (el.style.borderTopWidth)?parseInt(el.style.borderTopWidth):0;

  pos = [box.x - borderLeft, box.y - borderTop];
 }
 else // safari & opera
 {
  pos = [el.offsetLeft, el.offsetTop];
  parent = el.offsetParent;
  if (parent != el) {
   while (parent) {
    pos[0] += parent.offsetLeft;
    pos[1] += parent.offsetTop;
    parent = parent.offsetParent;
   }
  }
  if (ua.indexOf('opera') != -1 
   || ( ua.indexOf('safari') != -1 && el.style.position == 'absolute' )) 
  {
    pos[0] -= document.body.offsetLeft;
    pos[1] -= document.body.offsetTop;
  } 
 }
  
 if (el.parentNode) { parent = el.parentNode; }
 else { parent = null; }
  
 while (parent && parent.tagName != 'BODY' && parent.tagName != 'HTML') 
 { // account for any scrolled ancestors
  pos[0] -= parent.scrollLeft;
  pos[1] -= parent.scrollTop;
  
  if (parent.parentNode) { parent = parent.parentNode; } 
  else { parent = null; }
 }
 return {x:pos[0], y:pos[1]};
}

//验证控件显示设置
//_strmsg :显示的提示信息
function VildateMsgShow( _strmsg ,_ValidateControlID )
{   
   var valobj = document.getElementById('SpanVildateMsg');
   if (typeof(valobj.display) == "string")
     {
        if (valobj.display == "None")
        {
            return;
        }
    }
    if ((navigator.userAgent.indexOf("Mac") > -1) &&(navigator.userAgent.indexOf("MSIE") > -1))
    {
        valobj.style.visibility = "visible";
    }
	
    valobj.style.position = "absolute";
    valobj.style.className = "";    
    valobj.innerHTML="<span style=\"z-index:998;filter:alpha(opacity=100)\" class=\"category-pop\" id='"+valobj.id+"____'><label class=\"pop-l\"></label><a>"+_strmsg+"</a><label class=\"pop-r\" title=\"关闭提示\" onclick=\"javascript:HideWinErrMsgTips('"+valobj.id+"')\">[关闭]</label></span>";

    var obj = document.getElementById(_ValidateControlID)    
    var WinElementPos = getWinElementPos(obj)
	valobj.style.left = (parseInt(WinElementPos.x+obj.offsetWidth)).toString() + "px";
	valobj.style.top = (parseInt(WinElementPos.y)).toString() + "px";   
     valobj.style.visibility ="visible";
    __Win__BlockFlag = true;
    if(typeof(__Win__IsAutoClose)!="undefined"&&__Win__IsAutoClose&&typeof(__Win__CloseWaitTime)!="undefined")
        setTimeout("HideWinErrMsgTips('"+valobj.id+"')",__Win__CloseWaitTime);
        
    return false;
}