$(function() {
  setTimeout(() => {
    let errMsgs = document.querySelectorAll('[name=errMsg]')
    errMsgs.forEach(err => {
      err.children[0].click()
    })
  }, 1000)
})
