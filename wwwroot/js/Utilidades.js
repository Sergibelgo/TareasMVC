const urlTareas = "/api/tareas";
const urlPasos = "/api/pasos";
const urlArchivos = "/api/archivos";
async function manejarErrorApi(respuesta) {
    let mensajeError = "";
    if (respuesta.status == 400) {
        mensajeError = await respuesta.text();
    } else if (respuesta.status == 404) {
        mensajeError = errorRecursoNoEncontrado;
    } else {
        mensajeError = errorInesperado;
    }
    mostrarMensajeError(mensajeError);
}
function mostrarMensajeError(mensaje) {
    Swal.fire({
        text: mensaje,
        icon: "error",
        title:"Error..."
    })
}
function confirmarAccion({ callBackAceptar,callBackCancel,titulo }) {
    Swal.fire({
        title: titulo || "¿Realmente deseas hacer esto?",
        showCancelButton: true,
        confirmButtonText: "Si",
        focusCancel:true
    }).then(resultado => {
        if (resultado.isConfirmed) {
            callBackAceptar();
        } else if (callBackCancel) {
            callBackCancel();
        }
    })
}
function descargarArchivo(url, nombre) {
    var link = document.createElement('a');
    link.download = nombre;
    link.target = "_blank";
    link.href = url;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    delete link;
}