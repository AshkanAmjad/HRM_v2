//document.body.addEventListener('keydown', function (e) {
//    var key = e.key;
//    if (key === 'Enter') {
//        document.getElementById('submit').click();
//    }
//});

$(document).on('keydown', function (e) {
    var key = e.key;
    if (key === 'Enter') {
        $('#submit').trigger('click');
    }
});
