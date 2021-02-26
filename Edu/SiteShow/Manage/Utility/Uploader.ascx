<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Uploader.ascx.cs" Inherits="SiteShow.Manage.Utility.Uploader" %>
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

 
<div class="selectFileBtn" style="height: 30px;  line-height: 30px; float: left;">
      <%--  <button type="button" class="layui-btn" id="test5"><i class="layui-icon"></i>上传视频</button>--%>
        <div id="uploader" class="wu-example">
            <!--用来存放文件信息-->
            <div class="filename"></div>
           <%-- <div class="state"></div>--%>
           <%-- <div class="btns">--%>
                <div id="picker">选择文件</div>
               <%-- <button id="ctlBtn" class="layui-btn">开始上传</button>--%>
            <%--</div>--%>
            </div>
    </div>
    <div  id="uploadLoadingDiv" style="margin-top: 100px;display:none;">
        <span id="txt">分片上传：</span>
	<div class="layui-progress layui-progress-big" lay-showpercent="true" lay-filter="demo" lay-showPercent="true">
	    <div class="layui-progress-bar  layui-bg-green" lay-percent="0%"></div>
        </div>
    </div>
    
<div class="uploadbar" style="height: 50px;margin-top: 100px;">
   
    <div class="selectFileBtn" style="height: 30px; width: 160px; line-height: 20px;
        float: left;">
        
        <a
                class="layui-btn"
                hg="640" href="<%=uploaderPath %>FileExplorer.aspx?path=<%=UploadPath %>" http:localhost="" mmid="572#" panel.ashx="" sharekeyid=""
                target="_blank"
                teacher="" wd="">选择服务器端文件</a></div>
    <div class="selectFileBtn" style="height: 30px; width: 160px; line-height: 20px;
        float: left;">
        <a href="<%=uploaderPath %>OuterLink.aspx?path=<%=UploadPath %>&uid=<%=UID %>" hg="600"
            wd="300" target="_blank" class="layui-btn">站外视频链接</a></div>
    <div class="selectFileBtn" style="height: 30px; width: 160px; line-height: 20px;
        float: left;">
        <a href="<%=uploaderPath %>OtherLink.aspx?path=<%=UploadPath %>&uid=<%=UID %>" hg="600"
            wd="300" target="_blank" class="layui-btn">视频平台链接</a></div>
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
    layui.use('upload', function () {
        var upload = layui.upload;
        var host = location.host;
        upload_url = location.protocol + "//"+host+"<%=uploaderPath %>Uploading.ashx";
       
       
        var count = "<%= LimitCount %>";
        upload_url = upload_url+"?path="+path+"&uid="+uid+"&count="+count
        console.log(upload_url);
        upload.render({
            elem: '#test5'
            , url: upload_url
            , accept: 'video' //视频
            , xhr: xhrOnProgress
            , progress: function (value) {//上传进度回调 value进度值
                element.progress('demo', parseInt(value) + '%')//设置页面进度条
                console.log(value);
            }
            , done: function (res) {
                console.log(res)
                uploadCG(res.path);
            }
        });
    });


    var element,layer;
    layui.use(['element','layer'], function () {
        element = layui.element;
        layer=layui.layer;
    });
    
    //var upload_url2 = location.protocol + "//" + host + "<%=uploaderPath %>GetChunkFiles.ashx?path="+path;
    var counts=0;
    var numChunk=0;
    var laryIndex,loadsindex;

    $(function () {
        var GUID = WebUploader.Base.guid();//一个GUID  GetChunkFiles
     var host = location.host;
     var upload_url = location.protocol + "//" + host + "<%=uploaderPath %>FileUpLoad.ashx?path="+path;
     var upload_url1 = location.protocol + "//" + host + "<%=uploaderPath %>MergeFiles.ashx?path="+path;
        $("#uploadLoadingDiv").css("display", "none");
        uploadereditsVideo = WebUploader.create({
            // swf文件路径
            swf: '<%=uploaderPath %>webuploader-0.1.5//Uploader.swf',
            // 文件接收服务端。
            server: upload_url, 
            pick: {
                id: '#picker',
                label: '上传',
                innerHTML: '上传',
                multiple: false
            },
            accept: {
                title: 'Video',
                extensions: 'mp4',
                mimeTypes: 'video/*'
            },
            //fileNumLimit: 1,
            //fileSingleSizeLimit: 1024 * 1024 * 1000,
            //chunked: true,//开始分片上传
            //chunkSize: 1024 * 1024 * 20,//每一片的大小
            resize: false,
            chunked: true,//开始分片上传
            threads: 1,//并发上传数量
            chunkRetry: 3,//如果某个分片由于网络问题出错，允许自动重传3次
            chunkSize: 204800*3,//每一片的大小600k
            formData: {
                guid: GUID //自定义参数
            }
        });

        uploadereditsVideo.on('fileQueued', function (file) {
            $("#uploader .filename").html("文件名：" + file.name);
            $("#uploadLoadingDiv").css("display", "block");
            uploadereditsVideo.upload();
            //uploadereditsVideo.md5File(file)

            //    // 及时显示进度
            //    .progress(function (percentage) {
            //        console.log('Percentage:', percentage);
            //        element.progress('demo', (parseInt(percentage * 100)-1) + '%');
            //    })

            //    // 完成
            //    .then(function (val) {
            //        console.log('md5 result:', val);
            //    });
        });
        uploadereditsVideo.on('uploadProgress', function (file, percentage) {
            console.log('Percentage:', percentage);
            element.progress('demo', (parseInt(percentage * 100) - 1) + '%');
            //if (parseInt(percentage * 100)==100) {
                //$("#txt").innerText = "文件合并中："
                //element.progress('demo','0%');
                //GetChunkFiles();
           // }
            if (parseInt(percentage * 100)==100) {
                laryIndex = layer.msg('视频合并中，可能耗时较长,请您耐心等待！！！', { icon: 1, time: -1 });
                loadsindex = layer.load(0, { shade: false, time: -1 });
            }
            
        });

        // 文件上传成功
        uploadereditsVideo.on('uploadSuccess', function (file, response) {
            //合并文件
            // var uid = "<%=UID %>";
            //GetChunkFilesTime();
            console.log(file.name+"   "+ response.chunked);
            $.post(upload_url1, { guid: GUID, fileName: file.name, uid: uid }, function (data) {
                //alert(data)
                console.log(data);
                if (data.r == 1) {
                    element.progress('demo', '100%');
                    layer.close(laryIndex);
                    layer.close(loadsindex);
                    $("#uploadLoadingDiv").css("display", "none");
                    uploadCG(data.path);
                    //alert("上传完成" + data.path);
                    console.log(data.path);
                }
                else {
                    
                    layer.alert(data.err, {
                        skin: 'layui-layer-molv' //样式类名
  ,closeBtn: 0
                    }, function(){
                        location.reload();
                    });
                    layer.close(laryIndex);
                    layer.close(loadsindex);
                    $("#uploadLoadingDiv").css("display", "none");
                    
                }
            },'json');
        });
        $("#ctlBtn").click(function () {
            uploadereditsVideo.upload();
            $("#ctlBtn").text("上传");
            $('#ctlBtn').attr('disabled', 'disabled');
            $("#uploader .state").html("上传中...");
        });
    });
   

    //function GetChunkFilesTime(){
    //    $.post(upload_url2, { guid: GUID }, function (data) {
    //        //alert(data)
    //        console.log(data);
    //        if (data.r == 1) {
    //            numChunk=data.counts;

    //            element.progress('demo', parseInt(((counts-numChunk)/counts)*100)+'%');
    //            console.log(numChunk);
    //        }
    //        else {
    //            alert(data.err);
    //            //setTimeout(GetChunkFilesTime(),1000);
    //        }
    //    },'json');

    //    setTimeout(GetChunkFilesTime(),1000);

    //}

    //function GetChunkFiles(){
    //    $.post(upload_url2, { guid: GUID }, function (data) {
    //        //alert(data)
    //        console.log(data);
    //        if (data.r == 1) {
    //            counts=data.counts;
    //            console.log(data.counts);
    //        }
    //        else {
    //            alert(data.err);
    //        }
    //    },'json');
    //}
    var swfu;
    (function () {
        var settings = {
            //flash_url: "<%=uploaderPath %>swfupload/swfupload.swf",
            upload_url: "<%=uploaderPath %>Uploading.ashx?path=<%=UploadPath %>&uid=<%=UID %>&count=<%= LimitCount %>",
            file_size_limit: "0",
            file_types: "*.mp4",
            file_types_description: "视频文件",
            file_upload_limit: <%= LimitCount %>,
            file_queue_limit: 0,
            custom_settings: {

                progressTarget: "divprogresscontainer",
                progressGroupTarget: "divprogressGroup",

                //progress object
                container_css: "progressobj",
                icoNormal_css: "IcoNormal",
                icoWaiting_css: "IcoWaiting",
                icoUpload_css: "IcoUpload",
                fname_css: "fle ftt",
                state_div_css: "statebarSmallDiv",
                state_bar_css: "statebar",
                percent_css: "ftt",
                href_delete_css: "ftt",

                //sum object
                /*
                页面中不应出现以"cnt_"开头声明的元素
                */
                s_cnt_progress: "cnt_progress",
                s_cnt_span_text: "fle",
                s_cnt_progress_statebar: "cnt_progress_statebar",
                s_cnt_progress_percent: "cnt_progress_percent",
                s_cnt_progress_uploaded: "cnt_progress_uploaded",
                s_cnt_progress_size: "cnt_progress_size"
            },
            debug: false,

            // Button settings
            //button_image_url: "<%=uploaderPath %>images/TestImageNoText_65x29.png",
            button_width: "200",
            button_height: "29",
            button_placeholder_id: "spanButtonPlaceHolder",
            button_text: '<span class="theFont">点击此处上传文件</span>',
            button_text_style: ".theFont { font-size: 12;color:#0068B7; }",
            button_text_left_padding: 12,
            button_text_top_padding: 3,
            button_cursor : SWFUpload.CURSOR.HAND,
            button_window_mode : SWFUpload.WINDOW_MODE.TRANSPARENT,
            // The event handler functions are defined in handlers.js
            file_queued_handler: fileQueued,
            file_queue_error_handler: fileQueueError,
            upload_start_handler: uploadStart,
            upload_progress_handler: uploadProgress,
            upload_error_handler: uploadError,
            upload_success_handler: uploadSuccess,
            upload_complete_handler: uploadComplete,
            file_dialog_complete_handler: fileDialogComplete
        };
        //swfu = new SWFUpload(settings);
    })();
    function fileQueueError(fileobject, errorcode, message) {
        alert(errorcode);
    }

    function uploadCG(file) {
        var width = 500;
        var height = 400;
        var str = "<video controls='' width=" + width + " height=" + height + " class='ke - media'><source src='" + file + "'></video>";
        $("#divPlayer").html(str);
    }
    function uploadSuccess(file, serverData) {
        var btn = $(".swfPlace");
        btn.remove();
        //上传后的文件
        var file = serverData;
        var width=500;
        var height=400;
        <%--var str = "<object wmode='transparent' classid='clsid:D27CDB6E-AE6D-11cf-96B8-4445535411111'  codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0'";
        str += "  width=" + width + " height=" + height + " >";
          str += "<param name='movie' value='<%=uploaderPath %>flvplayer.swf?vcastr_file=" + file + "' />";
          str += "<param name='quality' value='high' />";
          str += "<param name='wmode' value='transparent' />";
          str += "<param name='allowFullScreen' value='true' />";
          str += "<param name='FlashVars' value='vcastr_file=" + file + "&IsAutoPlay=1&IsContinue=1' />";
          str += "<embed src='<%=uploaderPath %>flvplayer.swf?vcastr_file="+file+"' allowfullscreen='true'";
          str += " flashvars='vcastr_file=" + file + "&IsAutoPlay=1&IsContinue=1' quality='high' pluginspage='http://www.macromedia.com/go/getflashplayer' ";
          str += " type='application/x-shockwave-flash'   wmode='transparent' width=" + width + " height=" + height + " />";
        str += "</object>";--%>
        var str = "<video controls='' width=" + width + " height=" + height + " class='ke - media'><source src='"+file+"'></video>";
          $("#divPlayer").html(str);
    }
</script>
<div id="divprogresscontainer">
</div>
<div id="divprogressGroup">
</div>
<div id="divPlayer" style="margin-top:10px;">
</div>
<script language="javascript" type="text/javascript">
    //弹出文件选择的窗口
    $(".selectFileBtn a").click(function () {
        var src = $(this).attr("href");
        var title = $(this).text();
        var height = Number($(this).attr("hg"));
        var width = Number($(this).attr("wd"));
        var box = new top.PageBox(title, src, height, width, null, window.name);
        box.Open();
        return false;
    });
    //设置选择的文件
    function setSelectFile(file, path, winname) {
        new top.PageBox().Close(winname);
        //alert(path + file);
        uploadSuccess(file, path + file);
        //
        var select_url = "<%=uploaderPath %>SelectFile.ashx?path=<%=UploadPath %>&uid=<%=UID %>&count=<%= LimitCount %>&file=" + file;
        //alert(select_url);
        $.get(select_url, function () {
            alert("设置成功");
        });
    }
    //设置外部链接
    function setOuterLink(file, path, winname) {
        new top.PageBox().Close(winname);
        var video = $("a.video");
        if (video.size() > 1) {
            $("a.video").html(file + " <span style='color:red'>(外部链接)</span>");
            $("a.video").attr("href", path);
            alert("设置外部链接成功！");
        } else {
            window.location.reload();
        }
    }
</script>
