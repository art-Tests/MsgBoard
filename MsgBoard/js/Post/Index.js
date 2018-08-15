;(function() {
  let p = {
    replyBtns: document.querySelectorAll('a.reply-link'),
    replyHideBtns: document.querySelectorAll('a.reply-close'),
    delBtns: document.querySelectorAll('a.delete-link'),
    delCnfmBtns: document.querySelectorAll('.delete-confirm'),
    tmpDivSelector: 'div[name=tmpl-div]',
    delLinkSelector: '.delete-link',
    delCnfmSelector: '.delete-confirm',
    replyAreaSelector: '.reply-area',
    replyLinkSelector: '.reply-link',
    replyCloseSelector: '.reply-close',
    userIdSelector: '#userId',
    uriPostDel: '/Post/Delete',
    uriReplyDel: '/Reply/Delete',
    templateId: '#tmpl'
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
    form.action = p.uriPostDel

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
    form.action = p.uriReplyDel

    idDom.value = replyId
    idDom.name = 'id'
    form.appendChild(idDom)

    document.body.appendChild(form)
    form.submit()
  }

  const delCnfmHandler = function() {
    $(this)
      .hide()
      .siblings(p.delCnfmSelector)
      .show()
  }

  const getReplyHandler = function() {
    let area = $(this)
      .hide()
      .siblings(p.ReCloseSelector)
      .show()
      .siblings(p.replyAreaSelector)
      .show()

    area[0].innerHTML = ''

    let postId = $(this).data('postid')
    let userId = $(p.userIdSelector).val()
    $.ajax({
      type: 'Get',
      url: `/API/Reply/${postId}?user=${userId}`,
      success: function(res) {
        $(p.templateId)
          .tmpl(res)
          .appendTo(area)
        $(p.tmpDivSelector).delegate(p.delLinkSelector, 'click', delCnfmHandler)
        $(p.tmpDivSelector).delegate(
          p.delCnfmSelector,
          'click',
          delReplyHandler
        )
      }
    })
  }

  const hideReplyArea = function() {
    $(this)
      .hide()
      .siblings(p.replyLinkSelector)
      .show()
      .siblings(p.replyAreaSelector)
      .hide()
  }
  class App {
    static Init() {
      p.delBtns.forEach(btn => btn.addEventListener('click', delCnfmHandler))
      p.delCnfmBtns.forEach(btn =>
        btn.addEventListener('click', delPostHandler)
      )
      p.replyBtns.forEach(link =>
        link.addEventListener('click', getReplyHandler)
      )
      p.replyHideBtns.forEach(link =>
        link.addEventListener('click', hideReplyArea)
      )
    }
  }
  App.Init()
})()
