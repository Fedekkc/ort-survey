// Operación de red encapsulada y protegida
async function solicitarDatosAlServidorCorporativo(urlCompletaDelRecurso, tokenDeSeguridad) {
    let datosRecuperadosDelBackend = null;

    try {
        const configuracionDeLaPeticionHttp = {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + tokenDeSeguridad
            }
        };

        const respuestaDelControlador = await fetch(urlCompletaDelRecurso, configuracionDeLaPeticionHttp);

        if (respuestaDelControlador.ok) {
            datosRecuperadosDelBackend = await respuestaDelControlador.json();
        } else {
            console.error("El servidor C# rechazó la solicitud autorizada.");
        }
    } catch (errorDeRedDetectado) {
        console.error("Fallo crítico al intentar contactar la infraestructura backend.", errorDeRedDetectado);
    }

    return datosRecuperadosDelBackend;
}

// Validación de la bóveda de credenciales
function verificarAutenticacionActivaCorporativa() {
    let tokenRecuperadoDeLaMemoria = "";
    const datoAlmacenadoEnElNavegador = localStorage.getItem("tokenDeSeguridadCorporativo");

    if (datoAlmacenadoEnElNavegador !== null) {
        tokenRecuperadoDeLaMemoria = datoAlmacenadoEnElNavegador;
    } else {
        console.warn("Acceso denegado: Se requiere autenticación válida en la memoria.");
    }

    return tokenRecuperadoDeLaMemoria;
}

// Extracción del ID de la URL
function obtenerIdentificadorDeEncuestaDesdeLaUrl() {
    let identificadorCapturado = "";
    const parametrosDeLaDireccionWeb = new URLSearchParams(window.location.search);
    const valorDelParametroId = parametrosDeLaDireccionWeb.get("idEncuesta");

    if (valorDelParametroId !== null) {
        identificadorCapturado = valorDelParametroId;
    }

    return identificadorCapturado;
}

// Proceso principal de ensamblaje visual y analítico
async function iniciarMapeoAutomaticoDeDatos() {
    let estadoDeLaOperacionAutomatica = false;

    const tokenCorporativoActivo = verificarAutenticacionActivaCorporativa();
    const identificadorDeLaEncuesta = obtenerIdentificadorDeEncuestaDesdeLaUrl();

    if (tokenCorporativoActivo !== "" && identificadorDeLaEncuesta !== "") {
        const rutaEndpointResumenGlobal = 'https://localhost:7001/metricas/' + identificadorDeLaEncuesta;
        const rutaEndpointDistribucionGrafica = 'https://localhost:7001/metricas/' + identificadorDeLaEncuesta + '/distribucion';
        const rutaEndpointEvolucionTemporal = 'https://localhost:7001/metricas/' + identificadorDeLaEncuesta + '/timeline';

        const resultadoResumenBaseDatos = await solicitarDatosAlServidorCorporativo(rutaEndpointResumenGlobal, tokenCorporativoActivo);
        const resultadoDistribucionBaseDatos = await solicitarDatosAlServidorCorporativo(rutaEndpointDistribucionGrafica, tokenCorporativoActivo);
        const resultadoEvolucionTemporalBaseDatos = await solicitarDatosAlServidorCorporativo(rutaEndpointEvolucionTemporal, tokenCorporativoActivo);

        if (resultadoResumenBaseDatos !== null) {
            document.getElementById('cantidadTotalDeRespuestas').textContent = resultadoResumenBaseDatos.totalRespuestasEncuesta || "0";
            document.getElementById('cantidadTotalDePreguntas').textContent = resultadoResumenBaseDatos.totalPreguntas || "0";
            document.getElementById('estadoActualDeLaEncuesta').textContent = "Activa";
        }

        if (resultadoDistribucionBaseDatos !== null) {
            const contenedorListaOpcionesVisual = document.getElementById('listaDeOpcionesYPorcentajes');
            contenedorListaOpcionesVisual.innerHTML = "";

            let indicePreguntaIterada = 0;

            while (indicePreguntaIterada < resultadoDistribucionBaseDatos.length) {
                const preguntaActualDeLaBaseDatos = resultadoDistribucionBaseDatos[indicePreguntaIterada];
                const arregloDeOpcionesDeLaPregunta = preguntaActualDeLaBaseDatos.opciones;

                const elementoTituloPreguntaVisual = document.createElement('li');
                elementoTituloPreguntaVisual.innerHTML = "<strong style='color:#0056b3; margin-top:10px;'>" + preguntaActualDeLaBaseDatos.textoPregunta + "</strong>";
                contenedorListaOpcionesVisual.appendChild(elementoTituloPreguntaVisual);

                let indiceOpcionIterada = 0;

                while (indiceOpcionIterada < arregloDeOpcionesDeLaPregunta.length) {
                    const datosOpcionActualRecuperada = arregloDeOpcionesDeLaPregunta[indiceOpcionIterada];

                    const elementoFilaDeLaLista = document.createElement('li');

                    const elementoTextoOpcionDescriptivo = document.createElement('span');
                    elementoTextoOpcionDescriptivo.className = "etiqueta-opcion";
                    elementoTextoOpcionDescriptivo.textContent = datosOpcionActualRecuperada.textoOpcion;

                    const elementoBarraDeProgresoNativa = document.createElement('progress');
                    elementoBarraDeProgresoNativa.className = "barra-distribucion";
                    elementoBarraDeProgresoNativa.value = datosOpcionActualRecuperada.porcentaje;
                    elementoBarraDeProgresoNativa.max = 100;

                    const elementoPorcentajeNumericoTexto = document.createElement('span');
                    elementoPorcentajeNumericoTexto.className = "etiqueta-porcentaje";
                    elementoPorcentajeNumericoTexto.textContent = datosOpcionActualRecuperada.porcentaje + "%";

                    elementoFilaDeLaLista.appendChild(elementoTextoOpcionDescriptivo);
                    elementoFilaDeLaLista.appendChild(elementoBarraDeProgresoNativa);
                    elementoFilaDeLaLista.appendChild(elementoPorcentajeNumericoTexto);

                    contenedorListaOpcionesVisual.appendChild(elementoFilaDeLaLista);

                    indiceOpcionIterada = indiceOpcionIterada + 1;
                }

                indicePreguntaIterada = indicePreguntaIterada + 1;
            }
        }

        if (resultadoEvolucionTemporalBaseDatos !== null) {
            let arregloDeEtiquetasParaElEjeHorizontal = [];
            let arregloDeValoresParaElEjeVertical = [];
            let indiceDeIteracionTemporal = 0;

            while (indiceDeIteracionTemporal < resultadoEvolucionTemporalBaseDatos.length) {
                const registroTemporalActual = resultadoEvolucionTemporalBaseDatos[indiceDeIteracionTemporal];
                arregloDeEtiquetasParaElEjeHorizontal.push(registroTemporalActual.fecha);
                arregloDeValoresParaElEjeVertical.push(registroTemporalActual.totalRespuestas);

                indiceDeIteracionTemporal = indiceDeIteracionTemporal + 1;
            }

            const elementoLienzoParaElGrafico = document.getElementById('lienzoGraficoEvolucionTemporal');
            const contextoDeDibujoDelLienzo = elementoLienzoParaElGrafico.getContext('2d');

            const configuracionDelGraficoDeLineas = {
                type: 'line',
                data: {
                    labels: arregloDeEtiquetasParaElEjeHorizontal,
                    datasets: [{
                        label: 'Volumen de Respuestas Diarias',
                        data: arregloDeValoresParaElEjeVertical,
                        borderColor: '#0056b3',
                        backgroundColor: 'rgba(0, 86, 179, 0.1)',
                        borderWidth: 2,
                        fill: true,
                        tension: 0.3
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false
                }
            };

            const instanciaDelGraficoVisual = new Chart(contextoDeDibujoDelLienzo, configuracionDelGraficoDeLineas);
        }

        estadoDeLaOperacionAutomatica = true;
    }

    return estadoDeLaOperacionAutomatica;
}

// Disparador de la arquitectura reactiva
window.addEventListener('DOMContentLoaded', iniciarMapeoAutomaticoDeDatos);