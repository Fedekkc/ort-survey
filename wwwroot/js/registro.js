document.getElementById("registerForm").addEventListener("submit", async e => {
    e.preventDefault();
    const data = {
        nombre: nombre.value,
        email: email.value,
        password: password.value,
        genero: genero.value
    };
    const r = await fetch("/auth/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });
    resultado.textContent = r.ok ? "Usuario creado" : "Error";
});