document.getElementById("registerForm").addEventListener("submit", async (e) => {
    e.preventDefault();
    const resultado = document.getElementById("resultado");
    const data = {
        nombre: document.getElementById("nombre").value,
        email: document.getElementById("email").value,
        password: document.getElementById("password").value,
        genero: document.getElementById("genero").value
    };

    const r = await fetch("/auth/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify(data)
    });

    if (r.ok) {
        resultado.textContent = "Usuario creado. Redirigiendo al login...";
        window.location.href = "/Home/Login";
        return;
    }

    const err = await r.json().catch(() => ({}));
    resultado.textContent = err.message || "Error al registrarse";
});
