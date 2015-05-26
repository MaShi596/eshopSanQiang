// JavaScript Document
    function showImage(source) {
        var img = document.getElementById("ProductDetails_img_Common_ViewProduct_Image");
        var url = source.attributes["src"].nodeValue;
        
        if (img.src == url) {
            return;
        }

        img.src = url;
        var img1 = new Image();
        img1.src = url;

        if (img1.width > 305 || img1.height > 402) {
            _w = img1.width / 305;
            _h = img1.height / 402;

            if (_h > _w) {
                _w = _h;
            }
            img1.width = img1.width / _w;
            img1.height = img1.height / _w;
        }
        if (img1.width > 0 && img1.height > 0) {
            img.style.width = img1.width + "px";
            img.style.height = img1.height + "px";
        }
    }
	//��Ʒ��ϸҳ��ͼ
    function ProductSwapImages(thisObject) {
        var upperID = thisObject.parentNode.parentNode.parentNode.parentNode.getElementsByTagName("div");
        for (var i = 0; i < upperID.length; i++) {
            upperID[i].className = "Product_WareSmall_1";
        }
        thisObject.className = "Product_WareSmall";
    }
    //��ע��Э�鴰��
    function openDialog(url) {
        window.open(url, '', 'width=1000,height=630,top=60,left=300,resizable=1,scrollbars=1,status=no,toolbar=no,location=no,menu=no');
    }
    //�����˿���֤
    function Validate() {
        var amount = parseFloat(document.getElementById("RefundOrders_txtRefundBanlance").value);
        if (isNaN(amount)) {
            alert("�˿����Ϊ�ջ��߱���������");
            document.getElementById("RefundOrders_txtRefundBanlance").value = "";
            return false;
        }
        if (amount <= 0) {
            alert("�˿����0�͸���");
            document.getElementById("RefundOrders_txtRefundBanlance").value = "";
            return false;
        }
        var orderTotal = parseFloat(document.getElementById("orderToatl").innerHTML.substring(1));
        if (amount > orderTotal) {
            alert("�˿������С�ڻ���ڶ������");
            document.getElementById("RefundOrders_txtRefundBanlance").value = "";
            return false;
        }
    }