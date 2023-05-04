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
        actualizarPaso(data, paso.id());
        paso.modoEdicion(false)
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
        const tarea = obtenerTareaEnEdicion();
        tarea.pasosTotales(tarea.pasosTotales() + 1);
        if (paso.realizado()) {
            tarea.pasosRealizados(tarea.pasosRealizados() + 1)
        }
    } else {
        manejarErrorApi(respuesta)
    }
}
async function actualizarPaso(data, id) {
    const respuesta = await fetch(`${urlPasos}/${id}`, {
        body: data,
        method: "PUT",
        headers: {
            "Content-type": "application/json"
        }
    });
    if (respuesta.ok) {

    } else {
        mostrarMensajeError(respuesta)
    }
}
function manejarClickDescripcionPaso(paso) {
    paso.modoEdicion(true);
    paso.descripcionAnterior = paso.descripcion();
    $("[name=txtPasoDescripcion]:visible").focus();
}
function manejarClickCheckPaso(paso) {
    if (paso.esNuevo()) {
        return true;
    }
    const data = obtenerCuerpoPeticionPaso(paso);
    actualizarPaso(data, paso.id());
    const tarea = obtenerTareaEnEdicion();
    if (paso.realizado()) {
        tarea.pasosRealizados(tarea.pasosRealizados() + 1)
    } else {
        tarea.pasosRealizados(tarea.pasosRealizados() - 1)
    }
    return true;
}
function manejarClickBorrarPaso(paso) {
    confirmarAccion({
        callBackAceptar: () => {
            borrarPaso(paso);

        }, callBackCancel: () => {

        }, titulo: "¿Esta seguro de querer borrar el paso?"
    })
}
async function borrarPaso(paso) {
    const respuesta = await fetch(`${urlPasos}/${paso.id()}`, {
        method: "DELETE"
    });
    if (!respuesta.ok) {
        manejarErrorApi(respuesta);

        return;
    }
    tareaEditarVM.pasos.remove((item) => { return item.id() == paso.id() })
    const tarea = obtenerTareaEnEdicion();
    tarea.pasosTotales(tarea.pasosTotales() - 1);
    if (paso.realizado()) {
        tarea.pasosRealizados(tarea.pasosRealizados() - 1)
    }
}
async function acutializarOrdenPasos() {
    const ids = obtenerIdPasos();
    await enviarIdsPasos(ids);
    const arregloOrganizado = tareaEditarVM.pasos.sorted(function (a, b) {
        return ids.indexOf(a.id().toString()) - ids.indexOf(b.id().toString());
    })
    tareaEditarVM.pasos(arregloOrganizado);
}
async function enviarIdsPasos(ids) {
    var data = JSON.stringify(ids);
    await fetch(`${urlPasos}/ordenar/${tareaEditarVM.id}`, {
        method: "POST",
        body: data,
        headers: {
            "Content-type":"application/json"
        }
    })
}
function obtenerIdPasos() {
    const ids = $("[name=chbPaso]").map(function () {
        return $(this).attr("data-id")
    }).get();
    return ids;
}