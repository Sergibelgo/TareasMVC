function manejarClickAgregarPaso() {
    const indice = tareaEditarVM.pasos().findIndex(p => p.esNuevo());
    if (indice != -1) {
        return;
    }
    tareaEditarVM.pasos.push(new pasoDTO({ modoEdicion: true, realizado: false }))
    $("[name=txtPasoDescripcion]:visible").focus();
}