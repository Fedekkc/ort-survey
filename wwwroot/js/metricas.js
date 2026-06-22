async function getJson(url) {
    const r = await fetch(url, { credentials: "include" });
    if (r.status === 401) {
        window.location.href = "/Home/Login";
        return null;
    }
    if (!r.ok) return null;
    return r.json();
}

function formatearEstado(estado) {
    if (!estado) return "-";
    const texto = estado.trim();
    if (!texto) return "-";
    return texto.charAt(0).toUpperCase() + texto.slice(1);
}

function renderDistribucion(distribucion) {
    const lista = document.getElementById("listaDeOpcionesYPorcentajes");
    lista.innerHTML = "";

    if (!distribucion || distribucion.length === 0) {
        return;
    }

    distribucion.forEach((pregunta) => {
        const titulo = document.createElement("li");
        titulo.innerHTML = "<strong style='color:#0056b3; margin-top:10px;'>" + pregunta.textoPregunta + "</strong>";
        lista.appendChild(titulo);

        (pregunta.opciones || []).forEach((op) => {
            const fila = document.createElement("li");

            const etiqueta = document.createElement("span");
            etiqueta.className = "etiqueta-opcion";
            etiqueta.textContent = op.textoOpcion;

            const barra = document.createElement("progress");
            barra.className = "barra-distribucion";
            barra.value = op.porcentaje;
            barra.max = 100;

            const porcentaje = document.createElement("span");
            porcentaje.className = "etiqueta-porcentaje";
            porcentaje.textContent = op.porcentaje + "%";

            fila.appendChild(etiqueta);
            fila.appendChild(barra);
            fila.appendChild(porcentaje);
            lista.appendChild(fila);
        });

        if (pregunta.totalRespuestasTextoLibre > 0) {
            const txt = document.createElement("li");
            txt.textContent = "Respuestas texto libre: " + pregunta.totalRespuestasTextoLibre;
            lista.appendChild(txt);
        }
    });
}

function renderTimeline(timeline) {
    if (!timeline || timeline.length === 0 || !window.Chart) {
        return;
    }

    const labels = timeline.map((t) => t.fecha);
    const values = timeline.map((t) => t.totalRespuestas);
    const canvas = document.getElementById("lienzoGraficoEvolucionTemporal");

    new Chart(canvas.getContext("2d"), {
        type: "line",
        data: {
            labels,
            datasets: [{
                label: "Volumen de Respuestas Diarias",
                data: values,
                borderColor: "#0056b3",
                backgroundColor: "rgba(0, 86, 179, 0.1)",
                borderWidth: 2,
                fill: true,
                tension: 0.3
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false
        }
    });
}

async function iniciarMetricas() {
    const params = new URLSearchParams(window.location.search);
    const idEncuesta = params.get("idEncuesta");
    const errorMsg = document.getElementById("errorMsg");

    if (!idEncuesta) {
        errorMsg.textContent = "Falta idEncuesta en la URL.";
        return;
    }

    const resumen = await getJson("/metricas/" + idEncuesta);
    const distribucion = await getJson("/metricas/" + idEncuesta + "/distribucion");
    const timeline = await getJson("/metricas/" + idEncuesta + "/timeline");
    const encuesta = await getJson("/encuestas/" + idEncuesta);

    if (!resumen) {
        errorMsg.textContent = "No se pudieron cargar las metricas.";
        return;
    }

    document.getElementById("cantidadTotalDeRespuestas").textContent = resumen.totalRespuestasEncuesta ?? 0;
    document.getElementById("cantidadTotalDePreguntas").textContent = resumen.totalPreguntas ?? 0;
    document.getElementById("estadoActualDeLaEncuesta").textContent = formatearEstado(encuesta?.estado);

    renderDistribucion(distribucion);
    renderTimeline(timeline);
}

document.getElementById("botonVolverAlListado")?.addEventListener("click", () => {
    window.location.href = "/Home/MisEncuestas";
});

window.addEventListener("DOMContentLoaded", iniciarMetricas);
