function manejarClickAgregarPaso() {
    const indice = tareaEditarVM.pasos().findIndex(p => p.esNuevo());
    if (indice != -1) {
        return;
    }
    tareaEditarVM.pasos.push(new pasoDTO({ modoEdicion: true, realizado: false }))
    $("[name=txtPasoDescripcion]:visible").focus();
}
function manejarClickCancelarPaso(paso) {
    if (paso.esNuevo()) {
        tareaEditarVM.pasos.pop();
    } else {
        paso.modoEdicion(false);
        paso.descripcion(paso.descripcionAnterior);
    }
}
async function manejarClickSalvarPaso(paso) {
    paso.modoEdicion(false);
    const esNuevo = paso.esNuevo();
    const idTarea = tareaEditarVM.id;
    const data = obtenerCuerpoPeticionPaso(paso);
    if (!paso.descripcion()) {
        return;
    }
    if (esNuevo) {
        insertarPaso(paso, data, idTarea);
    } else {
        actualizarPaso(data, paso.id(), paso);
    }
}
function obtenerCuerpoPeticionPaso(paso) {
    return JSON.stringify({
        descripcion: paso.descripcion(),
        realizado: paso.realizado()
    })
}
async function insertarPaso(paso, data, idTarea) {
    const respuesta = await fetch(`${urlPasos}/${idTarea}`, {
        method: "POST",
        body: data,
        headers: {
            "Content-type": "application/json"
        }
    })
    if (respuesta.ok) {
        const json = await respuesta.json();
        paso.id(json.id)
    } else {
        manejarErrorApi(respuesta)
    }
}
async function actualizarPaso(data, id,paso) {
    const respuesta = await fetch(`${urlPasos}/${id}`, {
        body: data,
        method: "PUT",
        headers: {
            "Content-type": "application/json"
        }
    });
    if (respuesta.ok) {
        paso.modoEdicion(false)
    } else {
        mostrarMensajeError(respuesta)
    }
}
function manejarClickDescripcionPaso(paso) {
    paso.modoEdicion(true);
    paso.descripcionAnterior = paso.descripcion();
    $("[name=txtPasoDescripcion]:visible").focus();
}