/// <reference path="jquery.tmpl.d.ts" />
;
(function (w, $) {
    var delCnfmHandler = function () {
        $(this)
            .hide()
            .siblings('.delete-confirm')
            .show();
    };
    var hideReplyArea = function () {
        $(this)
            .hide()
            .siblings('.reply-link')
            .show()
            .siblings('.reply-area')
            .hide();
    };
    /**
     * 確認刪除文章事件處理
     * 1. 取得文章Id
     * 2. 透過js建立表單以Post送出刪除請求
     */
    var delPostHandler = function () {
        var postId = $(this).data('id');
        var form = document.createElement('form');
        var idDom = document.createElement('input');
        form.method = 'POST';
        form.action = '/Post/Delete';
        idDom.value = postId;
        idDom.name = 'id';
        form.appendChild(idDom);
        document.body.appendChild(form);
        form.submit();
    };
    var delReplyHandler = function () {
        var replyId = $(this).data('id');
        var form = document.createElement('form');
        var idDom = document.createElement('input');
        form.method = 'POST';
        form.action = '/Reply/Delete';
        idDom.value = replyId;
        idDom.name = 'id';
        form.appendChild(idDom);
        document.body.appendChild(form);
        form.submit();
    };
    var getReplyHandler = function () {
        var area = $(this)
            .hide()
            .siblings('.reply-close')
            .show()
            .siblings('.reply-area')
            .show();
        area[0].innerHTML = '';
        var postId = $(this).data('postid');
        var userId = $('#userId').val();
        $.ajax({
            type: 'Get',
            url: "/API/Reply/" + postId + "?user=" + userId,
            success: function (res) {
                $('#tmpl')
                    .tmpl(res)
                    .appendTo(area);
                $('div[name=tmpl-div]').delegate('.delete-link', 'click', delCnfmHandler);
                $('div[name=tmpl-div]').delegate('.delete-confirm', 'click', delReplyHandler);
            }
        });
    };
    var PostIndex = /** @class */ (function () {
        function PostIndex() {
        }
        PostIndex.prototype.Init = function (document) {
            console.log('this is init()');
            document
                .querySelectorAll('a.delete-link')
                .forEach(function (btn) { return btn.addEventListener('click', delCnfmHandler); });
            document
                .querySelectorAll('.delete-confirm')
                .forEach(function (btn) { return btn.addEventListener('click', delPostHandler); });
            document
                .querySelectorAll('a.reply-link')
                .forEach(function (link) { return link.addEventListener('click', getReplyHandler); });
            document
                .querySelectorAll('a.reply-close')
                .forEach(function (link) { return link.addEventListener('click', hideReplyArea); });
        };
        return PostIndex;
    }());
    var app = new PostIndex();
    app.Init(document);
})(window, jQuery);
