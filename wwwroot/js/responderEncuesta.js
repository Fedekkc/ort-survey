async function cargarEncuesta(codigo) {
    const r = await fetch("/encuestas/p/" + encodeURIComponent(codigo));
    if (!r.ok) {
        document.getElementById("errorMsg").textContent = "Encuesta no encontrada.";
        return null;
    }
    return r.json();
}

function renderFormulario(encuesta) {
    const form = document.getElementById("respuestaForm");
    const contenedor = document.getElementById("preguntasRespuesta");
    contenedor.innerHTML = "";

    document.getElementById("encuestaId").value = encuesta.id_encuesta;
    document.getElementById("tituloEncuesta").textContent = encuesta.titulo;

    encuesta.preguntas.forEach((p) => {
        const fieldset = document.createElement("fieldset");
        fieldset.dataset.preguntaId = p.id_pregunta;
        const legend = document.createElement("legend");
        legend.textContent = p.texto;
        fieldset.appendChild(legend);

        if (p.opciones && p.opciones.length > 0) {
            p.opciones.forEach((op) => {
                const label = document.createElement("label");
                const radio = document.createElement("input");
                radio.type = "radio";
                radio.name = "pregunta_" + p.id_pregunta;
                radio.value = op.id_opcion;
                label.appendChild(radio);
                label.appendChild(document.createTextNode(" " + op.texto));
                fieldset.appendChild(label);
            });
        } else {
            const input = document.createElement("input");
            input.type = "text";
            input.className = "respuesta-texto";
            input.placeholder = "Tu respuesta";
            fieldset.appendChild(input);
        }

        contenedor.appendChild(fieldset);
    });

    form.classList.remove("oculto");
}

document.getElementById("respuestaForm")?.addEventListener("submit", async (e) => {
    e.preventDefault();
    const encuestaId = parseInt(document.getElementById("encuestaId").value, 10);
    const respuestas = [];

    document.querySelectorAll("#preguntasRespuesta fieldset").forEach((fieldset) => {
        const idPregunta = parseInt(fieldset.dataset.preguntaId, 10);
        const radio = fieldset.querySelector('input[type="radio"]:checked');
        const texto = fieldset.querySelector(".respuesta-texto");

        if (radio) {
            respuestas.push({ id_pregunta: idPregunta, id_opcion: parseInt(radio.value, 10) });
        } else if (texto && texto.value.trim()) {
            respuestas.push({ id_pregunta: idPregunta, valor_texto: texto.value.trim() });
        }
    });

    if (respuestas.length === 0) {
        document.getElementById("errorMsg").textContent = "Responde al menos una pregunta.";
        return;
    }

    const r = await fetch("/respuestas", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ encuestaId, respuestas })
    });

    const msg = document.getElementById("resultadoMsg");
    if (r.ok) {
        msg.textContent = "Respuestas enviadas. Gracias.";
        document.getElementById("respuestaForm").classList.add("oculto");
    } else {
        const err = await r.json().catch(() => ({}));
        document.getElementById("errorMsg").textContent = err.message || "Error al enviar respuestas";
    }
});

window.addEventListener("DOMContentLoaded", async () => {
    const encuestaCodigo = document.getElementById("encuestaCodigo")?.value;
    if (!encuestaCodigo) {
        document.getElementById("errorMsg").textContent = "Link de encuesta invalido.";
        return;
    }
    const encuesta = await cargarEncuesta(encuestaCodigo);
    if (encuesta) renderFormulario(encuesta);
});
