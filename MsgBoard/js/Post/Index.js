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

/**
 * 詢問是否確認刪除文章？
 * 1. 隱藏刪除按鈕
 * 2. 顯示確認按鈕
 */
const ConfirmDelete = function() {
  $(this)
    .hide()
    .siblings(page.DeleteConfirmClassName)
    .show()
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
const GetReplyData = function() {
  let area = $(this)
    .hide()
    .siblings(page.ReplyCloseClassName)
    .show()
    .siblings(page.ReplyAreaClassName)
    .show()
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
  $(this)
    .hide()
    .siblings(page.ReplyLinkClassName)
    .show()
    .siblings(page.ReplyAreaClassName)
    .hide()
}
class App {
  static Init() {
    page.ConfirmBtns.forEach(btn =>
      btn.addEventListener('click', ConfirmDelete)
    )

    page.DeleteConfirm.forEach(btn => btn.addEventListener('click', deletePost))

    page.ReplyLinks.forEach(link =>
      link.addEventListener('click', GetReplyData)
    )
    page.ReplyCloses.forEach(link =>
      link.addEventListener('click', hideReplyArea)
    )
  }
}

App.Init()

let delConfirm = function(obj) {
  $(obj)
    .hide()
    .siblings(page.DeleteConfirmClassName)
    .show()
}
