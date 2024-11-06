$(document).on('keydown', function (e) {
    var key = e.key;
    if (key === 'Enter') {
        $('#submitBtn').trigger('click');
    }
});

$('#navbar a').click(function (e) {
    e.preventDefault();
    $('#navbar a,#home').removeClass('navbar-active');
    $(this).addClass('navbar-active');
});