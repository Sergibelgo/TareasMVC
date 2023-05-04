function agregarNuevaTareaAlListado() {
    tareaListadoViewModel.tareas.push(new tareaElementoListadoDTO({ id: 0, titulo: "" }));
    $("[name=titulo-tarea]").last().focus();
}
async function manejarFocusoutTituloTarea(tarea) {
    const titulo = tarea.titulo();
    if (!titulo) {
        tareaListadoViewModel.tareas.pop();
        return;
    }
    const data = JSON.stringify(titulo);
    const respuesta = await fetch(urlTareas, {
        method: "POST",
        body: data,
        headers: {
            "Content-type": "application/json"
        }
    })
    if (respuesta.ok) {
        const json = await respuesta.json();
        tarea.id(json.id);
    } else {
        manejarErrorApi(respuesta);
    }
}
async function obtenerTareas() {
    tareaListadoViewModel.cargando(true);
    const respuesta = await fetch(urlTareas);
    if (!respuesta.ok) {
        manejarErrorApi(respuesta);
    } else {
        const json = await respuesta.json();
        tareaListadoViewModel.tareas([]);
        json.forEach(valor => {
            tareaListadoViewModel.tareas.push(new tareaElementoListadoDTO(valor))
        });
        tareaListadoViewModel.cargando(false);
    }
}