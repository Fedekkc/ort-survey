async function cargarMisEncuestas() {
    const contenedor = document.getElementById("misEncuestasList");
    const r = await fetch("/usuarios/mis-encuestas", { credentials: "include" });

    if (r.status === 401) {
        window.location.href = "/Home/Login";
        return;
    }

    if (!r.ok) {
        contenedor.textContent = "Error al cargar encuestas";
        return;
    }

    const encuestas = await r.json();
    contenedor.innerHTML = "";

    if (encuestas.length === 0) {
        contenedor.textContent = "No tenes encuestas.";
        return;
    }

    encuestas.forEach((enc) => {
        const codigo = enc.codigo_publico;
        const linkPublico = "/Home/ResponderEncuesta/" + encodeURIComponent(codigo);
        const div = document.createElement("div");
        div.className = "tarjeta-encuesta";
        div.innerHTML =
            "<strong>" + enc.titulo + "</strong><br>" +
            "Link: <a href=\"" + linkPublico + "\" target=\"_blank\">" + linkPublico + "</a><br>" +
            "<a href=\"" + linkPublico + "\">Responder</a> | " +
            "<a href=\"/Home/Metricas?idEncuesta=" + enc.id_encuesta + "\">Metricas</a> | " +
            "<a href=\"/qr/" + enc.id_encuesta + "/imagen\" target=\"_blank\">QR</a><br><br>";
        contenedor.appendChild(div);
    });
}

cargarMisEncuestas();
