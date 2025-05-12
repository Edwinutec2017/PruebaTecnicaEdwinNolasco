function Clientes(codCliente)
{
    $('#clienteload' ).modal('show');
    if (codCliente > 0) {
        $('#clienteload').modal('hide');

        const clienteInput = { CodCliente: codCliente, };

        $.ajax({
            url: urlObtenerTransacciones,
            type: 'POST',
            data: {clienteInput },
            success: function (response) {
                $("#container-partial").html(response);
                $('#clienteTransaccion').modal('show');
            },
            error: function (response) {
                console.log(response);
            },
            complete: function (response) {
                $('#clienteload').modal('hide');
            }
        });

    } else
    {
        $('#clienteload').modal('hide');

    }

}

function CloseModal()
{
    $('#clienteTransaccion').modal('hide');
}

