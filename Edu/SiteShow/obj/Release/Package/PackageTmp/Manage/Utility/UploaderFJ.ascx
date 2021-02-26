<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploaderFJ.ascx.cs" Inherits="SiteShow.Manage.Utility.Uploader" %>
<script type="text/javascript" src="<%=uploaderPath %>swfupload/swfupload.js"></script>
<script type="text/javascript" src="<%=uploaderPath %>js/swfupload.queue.js"></script>
<script type="text/javascript" src="<%=uploaderPath %>js/fileprogress.js"></script>
<script type="text/javascript" src="<%=uploaderPath %>js/filegroupprogress.js"></script>
<script type="text/javascript" src="<%=uploaderPath %>js/handlers.js"></script>
<link href="<%=uploaderPath %>layui/css/layui.css" rel="stylesheet" />
<link href="<%=uploaderPath %>webuploader-0.1.5/webuploader.css" rel="stylesheet" />
<script src="<%=uploaderPath %>webuploader-0.1.5/jquery-1.10.2.min.js"></script>
<script src="<%=uploaderPath %>layui/layui.js"></script>
<script src="<%=uploaderPath %>webuploader-0.1.5/webuploader.js"></script>


<%--<div class="selectFileBtn" style="height: 30px;  line-height: 30px; float: left;">
      <%--  <button type="button" class="layui-btn" id="test5"><i class="layui-icon"></i>上传视频</button>--%>
<%--<div id="uploader" class="wu-example">--%>
<!--用来存放文件信息-->
<%-- <div class="filename"></div>--%>
<%-- <div class="state"></div>--%>
<%-- <div class="btns">--%>
<%--  <div id="picker">选择文件</div>--%>
<%-- <button id="ctlBtn" class="layui-btn">开始上传</button>--%>
<%--</div>--%>
<%-- </div>
    </div>--%>

<div class="layui-upload-drag" id="test10">
    <i class="layui-icon"></i>
    <p>点击上传，或将文件拖拽到此处</p>
    <div class="layui-hide" id="uploadDemoView">
        <hr>
        <%-- <img src="" alt="上传成功后渲染" style="max-width: 196px">--%>
    </div>
</div>

<div style="margin-top: 24px;">
    <font color="red">*PPT/Word文档转为PDF格式，请下载转换工具，点击下载工具下载 ======》》  </font>
    <a href="../../Upload/Tools/PDF转换器.msi" style="color: #0068b7;">下载工具</a>
</div>


<div id="uploadLoadingDiv" style="margin-top: 100px; display: none;">
    <div class="layui-progress layui-progress-big" lay-filter="js_upload_progress" lay-showpercent="true">
        <div class="layui-progress-bar  layui-bg-green" lay-percent="0%"></div>
    </div>
</div>





<script type="text/javascript">
    //创建监听函数
    var xhrOnProgress = function (fun) {
        xhrOnProgress.onprogress = fun; //绑定监听
        //使用闭包实现监听绑
        return function () {
            //通过$.ajaxSettings.xhr();获得XMLHttpRequest对象
            var xhr = $.ajaxSettings.xhr();
            //判断监听函数是否为函数
            if (typeof xhrOnProgress.onprogress !== 'function')
                return xhr;
            //如果有监听函数并且xhr对象支持绑定时就把监听函数绑定上去
            if (xhrOnProgress.onprogress && xhr.upload) {
                xhr.upload.onprogress = xhrOnProgress.onprogress;
            }
            return xhr;
        }
    }

    var uploaderPath = "<%=uploaderPath %>";
    var uid = "<%=UID %>";
    var path = "<%=UploadPath %>";
    layui.use(['upload', 'element', 'layer'], function () {
        var upload = layui.upload;
        var element = layui.element;
        var layer = layui.layer;
        var host = location.host;
        var upload_url = location.protocol + "//" + host + "<%=uploaderPath %>FlieUpLoadNew.ashx?path=" + path + "&uid=" + uid;

        console.log(upload_url);


        var indes;
        var laryIndex, loadsindex
        //拖拽上传
        upload.render({
            elem: '#test10'
            , url: upload_url //改成您自己的上传接口
             , before: function () {
                 element.progress('js_upload_progress', '0%');//设置页面进度条
                 indes = layer.open({
                     type: 1,
                     title: '上传进度',
                     closeBtn: 1, //不显示关闭按钮
                     area: ['300px', '170px'],
                     shadeClose: false, //开启遮罩关闭
                     content: $("#uploadLoadingDiv").html(),
                     offset: '100px'
                 });
             }
            , xhr: xhrOnProgress
            , progress: function (value) {//上传进度回调 value进度值
                element.progress('js_upload_progress', (parseInt(value) - 1) + '%');//设置页面进度条
                if (parseInt(value) == 100) {
                    laryIndex = layer.msg('可能耗时较长,请您耐心等待！！！', { icon: 1, time: -1 });
                    loadsindex = layer.load(0, { shade: false, time: -1 });
                }
            }
            //,field: 'file'
            , accept: 'file'
            , done: function (res) {
                if (res.code == 1) {
                    layer.close(laryIndex);
                    layer.close(loadsindex);
                    layer.msg('上传成功');
                    element.progress('demo', '100%');
                    //$("#uploadLoadingDiv").css("display", "none");
                    //layui.$('#uploadDemoView').removeClass('layui-hide').find('img').attr('src', res.path);
                    layer.close(indes);
                    location.reload();
                    console.log(res)
                }

            }
        });

    });


</script>
