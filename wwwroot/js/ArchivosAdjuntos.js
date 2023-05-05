let inputArchivoTarea = document.getElementById("archivoATarea");
function manejarClickAgregarArchivoAdjunto() {
    inputArchivoTarea.click();
}
async function manejarSeleccionArchivoTarea(event) {
    const archivos = event.target.files;
    const archivosArreglo = Array.from(archivos);
    const idTarea = tareaEditarVM.id;
    const formData = new FormData();
    for (var i = 0; i < archivosArreglo.length; i++) {
        formData.append("archivos", archivosArreglo[i])
    }

    const respuesta = await fetch(`${urlArchivos}/${idTarea}`,
        {
            method: "POST",
            body: formData,
        })
    if (!respuesta.ok) {
        manejarErrorApi(respuesta);
        return;
    }
    const json = await respuesta.json();
    prepararArchivosAdjuntos(json);
    inputArchivoTarea.value = null;

}
function prepararArchivosAdjuntos(archivosAdjuntos) {
    archivosAdjuntos.forEach(archivo => {
        let fechaCreacion = archivo.fechaCreacion
        if (fechaCreacion.indexOf("Z") == -1) {
            fechaCreacion += "Z";
        }
        const fechaCreationDT = new Date(fechaCreacion);
        archivo.publicado = fechaCreationDT.toLocaleString();
        let archivoGuardar = new archivoAdjuntoViewModel({ ...archivo, modoEdicion: false });
        tareaEditarVM.archivosAdjuntos.push(archivoGuardar)
    })
}
let tituloArchivoAnterior;
function manejarClickTituloArchivoAdjunto(archivo) {
    archivo.modoEdicion(true);
    tituloArchivoAnterior = archivo.titulo();
    $("[name='txtArchivoAdjuntoTitulo']:visible").focus();
}
async function manejarFocusoutTituloArchivoAdjunto(archivo) {
    archivo.modoEdicion(false);
    const idArchivo = archivo.id;
    if (archivo.titulo() == tituloArchivoAnterior || !archivo.titulo()) {
        archivo.titulo(tituloArchivoAnterior);
        return;
    }
    const data = JSON.stringify(archivo.titulo());
    const respuesta = await fetch(`${urlArchivos}/${idArchivo}`, {
        method: "PUT",
        body: data,
        headers: {
            "Content-type":"application/json"
        }
    })
    if (!respuesta.ok) {
        manejarErrorApi(respuesta)
        return;
    }

}