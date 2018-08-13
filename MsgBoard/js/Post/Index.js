$(function() {
  let confirmBtns = document.querySelectorAll('a.delete-link')
  let delBtns = document.querySelectorAll('.delete-confirm')
  const ConfirmDelete = function() {
    var deleteLink = $(this)
    deleteLink.hide()
    var confirmButton = deleteLink.siblings('.delete-confirm')
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

  confirmBtns.forEach(btn => {
    btn.addEventListener('click', ConfirmDelete)
  })

  delBtns.forEach(btn => {
    btn.addEventListener('click', deletePost)
  })
})
