document.getElementById("addPregunta").onclick = () => {
    const div = document.createElement("div");
    div.classList.add("pregunta");
    div.innerHTML = `
        <input class="preguntaTexto" placeholder="Texto de la pregunta"><br>
        <div class="opcionesContainer"></div>
        <button type="button" class="addOpcion">+ Opcion</button><br><br>
    `;
    div.querySelector(".addOpcion").onclick = () => {
        const opt = document.createElement("input");
        opt.classList.add("opcionTexto");
        opt.placeholder = "Texto de opcion";
        div.querySelector(".opcionesContainer").appendChild(opt);
    };
    preguntasContainer.appendChild(div);
};

document.getElementById("encuestaForm").addEventListener("submit", async e => {
    e.preventDefault();
    const preguntas = [];
    document.querySelectorAll(".pregunta").forEach(div => {
        const texto = div.querySelector(".preguntaTexto").value;
        const opciones = [...div.querySelectorAll(".opcionTexto")]
            .filter(o => o.value.trim() !== "")
            .map(o => ({ texto: o.value }));
        preguntas.push({ texto, opciones });
    });

    const data = {
        titulo: titulo.value,
        descripcion: descripcion.value,
        es_publica: es_publica.checked,
        estado: estado.value,
        preguntas
    };

    const r = await fetch("/encuestas", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });

    resultado.textContent = r.ok ? "Encuesta creada" : "Error al crear";
});