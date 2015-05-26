// JavaScript Document
// 推荐商品与热销商品 的替换
function setHotTab(showId, oldId) {
    var show = document.getElementById("con_" + showId);
    show.style.display = "block";    
    var old = document.getElementById("con_" + oldId);
    old.style.display = "none";

    var showCss = document.getElementById(showId);
    showCss.className = "off";
    var oldCss = document.getElementById(oldId);
    oldCss.className = "";

    var showmore = document.getElementById("more_" + showId);
    showmore.className = "con_more";
    var oldmore = document.getElementById("more_" + oldId);
    oldmore.className = "con_more displayUn";
}
// 商城公告与商城资讯的替换
function setAfficheTab(showId, oldId) {    
    var show = document.getElementById("con_" + showId);
    show.style.display = "block";
    var old = document.getElementById("con_" + oldId);
    old.style.display = "none";

    var showCss = document.getElementById(showId);
    showCss.className = "hover";
    var oldCss = document.getElementById(oldId);
    oldCss.className = "";
}

// 商品详情与简单介绍的替换
function setProductDecTab(showId, oldId) {
    var show = document.getElementById("con_" + showId);
    show.className = "aOverFlow displayB";
    var old = document.getElementById("con_" + oldId);
    old.className = "aOverFlow displayUn";

    var showCss = document.getElementById(showId);
    showCss.className = "Product_intro_hover";
    var oldCss = document.getElementById(oldId);
    oldCss.className = "Product_intro_tab";
}