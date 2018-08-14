$(function() {
  let replyLinks = document.querySelectorAll('a.reply-link')
  let replyCloses = document.querySelectorAll('a.reply-close')

  let confirmBtns = document.querySelectorAll('a.delete-link')
  let delBtns = document.querySelectorAll('.delete-confirm')
  const ConfirmDelete = function() {
    let deleteLink = $(this)
    deleteLink.hide()
    let confirmButton = deleteLink.siblings('.delete-confirm')
    confirmButton.show()
  }

  const deletePost = function() {
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

  confirmBtns.forEach(btn => btn.addEventListener('click', ConfirmDelete))
  delBtns.forEach(btn => btn.addEventListener('click', deletePost))

  const hideTarget = target => target.hide('fast')
  const showTarget = target => target.show('fast')
  const findDom = (target, className) =>
    className ? target.siblings(className) : target

  const GetReplyData = function() {
    hideTarget($(this))
    showTarget(findDom($(this), '.reply-close'))
    let area = showTarget(findDom($(this), '.reply-area'))
    if (area[0].innerHTML !== '') return

    let postId = $(this).data('postid')
    let userId = $('#userId').val()
    $.ajax({
      type: 'Get',
      url: `/API/Reply/${postId}?user=${userId}`,
      success: function(res) {
        $(`#tmpl`)
          .tmpl(res)
          .appendTo(area)
      }
    })
  }

  const hideReplyArea = function() {
    hideTarget($(this))
    showTarget(findDom($(this), '.reply-link'))
    hideTarget(findDom($(this), '.reply-area'))
  }

  replyLinks.forEach(link => link.addEventListener('click', GetReplyData))
  replyCloses.forEach(link => link.addEventListener('click', hideReplyArea))
})
