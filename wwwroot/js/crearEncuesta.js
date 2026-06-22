function agregarPregunta() {
    const div = document.createElement("div");
    div.classList.add("pregunta");
    div.innerHTML =
        '<div class="pregunta-cabecera">' +
        '<input class="preguntaTexto" placeholder="Texto de la pregunta" minlength="5" required>' +
        '<button type="button" class="eliminarPregunta">Eliminar</button>' +
        '</div>' +
        '<div class="opcionesContainer"></div>' +
        '<button type="button" class="addOpcion">+ Opcion</button>';

    div.querySelector(".eliminarPregunta").onclick = () => div.remove();
    div.querySelector(".addOpcion").onclick = () => {
        const opt = document.createElement("input");
        opt.classList.add("opcionTexto");
        opt.placeholder = "Texto de opcion";
        div.querySelector(".opcionesContainer").appendChild(opt);
    };
    document.getElementById("preguntasContainer").appendChild(div);
}

document.getElementById("addPregunta").onclick = agregarPregunta;

document.getElementById("encuestaForm").addEventListener("submit", async (e) => {
    e.preventDefault();
    const resultado = document.getElementById("resultado");
    const preguntas = [];

    document.querySelectorAll(".pregunta").forEach((div) => {
        const texto = div.querySelector(".preguntaTexto").value.trim();
        if (texto.length < 5) {
            return;
        }
        const opciones = [...div.querySelectorAll(".opcionTexto")]
            .filter((o) => o.value.trim() !== "")
            .map((o) => ({ texto: o.value.trim() }));
        preguntas.push({ texto, opciones });
    });

    if (preguntas.length === 0) {
        resultado.textContent = "Agrega al menos una pregunta (minimo 5 caracteres).";
        return;
    }

    const titulo = document.getElementById("titulo").value.trim();
    if (titulo.length < 5) {
        resultado.textContent = "El titulo debe tener al menos 5 caracteres.";
        return;
    }

    const data = {
        titulo,
        descripcion: document.getElementById("descripcion").value,
        es_publica: document.getElementById("es_publica").checked,
        preguntas
    };

    const r = await fetch("/encuestas", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify(data)
    });

    if (r.ok) {
        resultado.textContent = "Encuesta creada";
        window.location.href = "/Home/MisEncuestas";
        return;
    }

    const err = await r.json().catch(() => ({}));
    if (err.message) {
        resultado.textContent = err.message;
    } else if (err.errors) {
        const msgs = Object.values(err.errors).flat();
        resultado.textContent = msgs.length ? msgs.join(" ") : "Error al crear encuesta";
    } else {
        resultado.textContent = "Error al crear encuesta";
    }
});

agregarPregunta();
