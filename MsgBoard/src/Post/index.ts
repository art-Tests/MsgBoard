;(function() {
  const delCnfmHandler = function() {
    $(this)
      .hide()
      .siblings('.delete-confirm')
      .show()
  }
  const hideReplyArea = function() {
    $(this)
      .hide()
      .siblings('.reply-link')
      .show()
      .siblings('.reply-area')
      .hide()
  }
  /**
   * 確認刪除文章事件處理
   * 1. 取得文章Id
   * 2. 透過js建立表單以Post送出刪除請求
   */
  const delPostHandler = function() {
    let postId = $(this).data('id')
    let form = document.createElement('form')
    let idDom = document.createElement('input')

    form.method = 'POST'
    form.action = '/Post/Delete'

    idDom.value = postId
    idDom.name = 'id'
    form.appendChild(idDom)

    document.body.appendChild(form)
    form.submit()
  }
  const delReplyHandler = function() {
    let replyId = $(this).data('id')
    let form = document.createElement('form')
    let idDom = document.createElement('input')

    form.method = 'POST'
    form.action = '/Reply/Delete'

    idDom.value = replyId
    idDom.name = 'id'
    form.appendChild(idDom)

    document.body.appendChild(form)
    form.submit()
  }

  const getReplyHandler = function() {
    let area = $(this)
      .hide()
      .siblings('.reply-close')
      .show()
      .siblings('.reply-area')
      .show()

    area[0].innerHTML = ''

    let postId = $(this).data('postid')
    let userId = $('#userId').val()
    $.ajax({
      type: 'Get',
      url: `/API/Reply/${postId}?user=${userId}`,
      success: function(res) {
        $('#tmpl')
          .tmpl(res)
          .appendTo(area)
        $('div[name=tmpl-div]').delegate(
          '.delete-link',
          'click',
          delCnfmHandler
        )
        $('div[name=tmpl-div]').delegate(
          '.delete-confirm',
          'click',
          delReplyHandler
        )
      }
    })
  }

  class PostIndex {
    Init(document: any) {
      console.log('this is init()')
      document
        .querySelectorAll('a.delete-link')
        .forEach(btn => btn.addEventListener('click', delCnfmHandler))

      document
        .querySelectorAll('.delete-confirm')
        .forEach(btn => btn.addEventListener('click', delPostHandler))

      document
        .querySelectorAll('a.reply-link')
        .forEach(link => link.addEventListener('click', getReplyHandler))
      document
        .querySelectorAll('a.reply-close')
        .forEach(link => link.addEventListener('click', hideReplyArea))
    }
  }
  var app = new PostIndex()
  app.Init(document)
})()
