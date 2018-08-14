let page = {
  ReplyLinks: document.querySelectorAll('a.reply-link'),
  ReplyCloses: document.querySelectorAll('a.reply-close'),
  ConfirmBtns: document.querySelectorAll('a.delete-link'),
  DeleteConfirm: document.querySelectorAll('.delete-confirm'),
  DeleteConfirmClassName: '.delete-confirm',
  ReplyAreaClassName: '.reply-area',
  ReplyLinkClassName: '.reply-link',
  ReplyCloseClassName: '.reply-close',
  DeletePostAction: '/Post/Delete',
  UserIdSelector: '#userId',
  JQueryTemplate: '#tmpl'
}

class App {
  static Init() {
    const hideTarget = target => target.hide('fast')
    const showTarget = target => target.show('fast')
    const findDom = (target, className) =>
      className ? target.siblings(className) : target

    let replyLinks = page.ReplyLinks
    let replyCloses = page.ReplyCloses

    /**
     * 詢問是否確認刪除文章？
     * 1. 隱藏刪除按鈕
     * 2. 顯示確認按鈕
     */
    const ConfirmDelete = function() {
      hideTarget($(this))
      showTarget(findDom($(this), page.DeleteConfirmClassName))
    }

    /**
     * 確認刪除文章事件處理
     * 1. 取得文章Id
     * 2. 透過js建立表單以Post送出刪除請求
     */
    const deletePost = function() {
      let postId = $(this).data('id')
      let form = document.createElement('form')
      let idDom = document.createElement('input')

      form.method = 'POST'
      form.action = page.DeletePostAction

      idDom.value = postId
      idDom.name = 'id'
      form.appendChild(idDom)

      document.body.appendChild(form)
      form.submit()
    }

    let confirmBtns = page.ConfirmBtns
    confirmBtns.forEach(btn => btn.addEventListener('click', ConfirmDelete))

    let delBtns = page.DeleteConfirm
    delBtns.forEach(btn => btn.addEventListener('click', deletePost))

    const GetReplyData = function() {
      hideTarget($(this))
      showTarget(findDom($(this), page.ReplyCloseClassName))
      let area = showTarget(findDom($(this), page.ReplyAreaClassName))
      if (area[0].innerHTML !== '') return

      let postId = $(this).data('postid')
      let userId = $(page.UserIdSelector).val()
      $.ajax({
        type: 'Get',
        url: `/API/Reply/${postId}?user=${userId}`,
        success: function(res) {
          $(page.JQueryTemplate)
            .tmpl(res)
            .appendTo(area)
        }
      })
    }

    const hideReplyArea = function() {
      hideTarget($(this))
      showTarget(findDom($(this), page.ReplyLinkClassName))
      hideTarget(findDom($(this), page.ReplyAreaClassName))
    }

    replyLinks.forEach(link => link.addEventListener('click', GetReplyData))
    replyCloses.forEach(link => link.addEventListener('click', hideReplyArea))
  }
}

App.Init()
