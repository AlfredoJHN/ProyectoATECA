$(document).ready(function () {
    // Enable Live Search.  
    $('#UsuariosList').attr('data-live-search', true);

    $('.selectUsuarios').selectpicker(
        {
            width: 'auto',
            title: 'Seleccione cedula de usuario',
            
            size: 6
        });
}); 