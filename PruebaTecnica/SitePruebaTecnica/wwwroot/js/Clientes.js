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

var  acction=""
function Compras(accion)
{
    if (accion == 1) {
        $("#frmclientes").prop("hidden", false);
        $("#description").prop("hidden", false);
        acction = "c";
    } else
    {
        $("#frmclientes").prop("hidden", true);
    }
      
}

function Pagos(accion) {
    if (accion == 1) {
        $("#frmclientes").prop("hidden", false);
        $("#description").prop("hidden", true);
        acction = "p";
    } else
    {
        $("#frmclientes").prop("hidden", true);
    }
}

function Transaciones(codCliente)
{
    if (acction == "c") {
        AddCompras(codCliente);
    } else
    {
        AddPagosCliente(codCliente);
    }

}


function AddCompras(CodCliente)
{
    var desciption = document.getElementById("Transaccion_Description");
    var monto = document.getElementById("Transaccion_Monto");
    var fecha = document.getElementById("Transaccion_FechaTransaccion");
    if (validarInputsDetallado("c"))
    {
        ModalPrincipalClose();
        $('#clienteload').modal('show');

        const compras = {
            CodCliente: CodCliente,
            Description: desciption.value,
            Monto: monto.value,
            FechaTransaccion: fecha.value
        };
        $.ajax({
            url: urlComprasCliente,
            type: 'POST',
            data: { compras },
            success: function (response) {

                if (response == "registrada") {
                    limpiarInputs();
                    ClienteEncabezado(compras.CodCliente);
                    ModalPrincipalOpen();
                } else
                {
                    ModalPrincipalOpen();
                    alert(response);
                }
            
            },
            error: function (response) {
                ModalPrincipalOpen();
                alert(response);
            },
            complete: function (response) {
                $('#clienteload').modal('hide');
            }
        });

    }
    $('#clienteload').modal('hide');
    

}


function AddPagosCliente(CodCliente) {

    var monto = document.getElementById("Transaccion_Monto");
    var fecha = document.getElementById("Transaccion_FechaTransaccion");
    if (validarInputsDetallado("p")) {
        ModalPrincipalClose();
        $('#clienteload').modal('show');

        const pagos = {
            CodCliente: CodCliente,
            Monto: monto.value,
            FechaTransaccion: fecha.value
        };

        $.ajax({
            url: urlPagosCliente,
            type: 'POST',
            data: { pagos },
            success: function (response) {
                if (response == "registrada") {
                    limpiarInputs();
                    ClienteEncabezado(pagos.CodCliente);
                    ModalPrincipalOpen();
                } else {
                    ModalPrincipalOpen();
                    alert(response);

                }
            },
            error: function (response) {
                console.log(response);
                ModalPrincipalOpen();
            },
            complete: function (response) {
                $('#clienteload').modal('hide');
            }
        });

    }
    $('#clienteload').modal('hide');


}


function limpiarInputs() {
    // Obtener los elementos del DOM
    var descripcion = document.getElementById("Transaccion_Description");
    var monto = document.getElementById("Transaccion_Monto");
    var fecha = document.getElementById("Transaccion_FechaTransaccion");

    // Limpiar los valores
    descripcion.value = "";
    monto.value = "";
    fecha.value = "";

    // Opcional: Enfocar el primer campo después de limpiar
    descripcion.focus();
}

function validarInputsDetallado(tipo) {
    var descripcion = document.getElementById("Transaccion_Description");
    var monto = document.getElementById("Transaccion_Monto");
    var fecha = document.getElementById("Transaccion_FechaTransaccion");
    var mensaje = "";


    if (tipo =="c")
    {
        if (descripcion.value.trim() === "") {
            mensaje += "La descripción no puede estar vacía.\n";
            descripcion.focus();
        }
    }

    if (monto.value.trim() === "") {
        mensaje += "El monto no puede estar vacío.\n";
        if (mensaje.split("\n").length < 3) monto.focus();
    }
    if (fecha.value.trim() === "") {
        mensaje += "La fecha no puede estar vacía.\n";
        if (mensaje.split("\n").length < 3) fecha.focus();
    }

    if (mensaje !== "") {
        alert(mensaje);
        return false;
    }

    return true;
}

function HistorialTransaccion(codCliente)
{
    $('#clienteload').modal('show');
    const clienteInput = { CodCliente: codCliente, };
    $.ajax({
        url: urlObtenerTransaccionesCliente,
        type: 'POST',
        data: { clienteInput },
        success: function (response) {
            $("#transaccion").html(response);
        },
        error: function (response) {
            console.log(response);
        },
        complete: function (response) {
            $('#clienteload').modal('hide');
        }
    });

}

function CloseModal()
{
    $('#clienteTransaccion').modal('hide');
}

function CloseModalCompras() {
    $('#addCompras').modal('hide');
}

function ClienteEncabezado(codCliente) {
    if (codCliente > 0) {
        const clienteInput = { CodCliente: codCliente, };

        $.ajax({
            url: urlCliente,
            type: 'POST',
            data: { clienteInput },
            success: function (response) {
                $("#detalleCliente").html(response);
                ClienteDetalle(clienteInput.CodCliente);
            },
            error: function (response) {
                console.log(response);
            },
            complete: function (response) {
               
            }
        });

    }

}
function ClienteDetalle(codCliente) {
    if (codCliente > 0) {
        const clienteInput = { CodCliente: codCliente, };

        $.ajax({
            url: urlClienteDetalle,
            type: 'POST',
            data: { clienteInput },
            success: function (response) {
                $("#transaccion").html(response);
            },
            error: function (response) {
                console.log(response);
            },
            complete: function (response) {
                $('#clienteload').modal('hide');
            }
        });

    } 

}


function ModalPrincipalClose()
{
    $('body').removeClass('modal-open cursor-blocked');
    $('#clienteTransaccion').modal('hide');
}


function ModalPrincipalOpen() {
    $('body').addClass('modal-open cursor-blocked');
    $('#clienteTransaccion').modal('show');
}