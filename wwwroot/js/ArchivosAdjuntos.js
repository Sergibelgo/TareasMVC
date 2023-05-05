﻿let inputArchivoTarea = document.getElementById("archivoATarea");
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
    console.log(json);
    inputArchivoTarea.value = null;

}