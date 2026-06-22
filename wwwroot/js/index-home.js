const botonLogOut = document.getElementById("botonLogOut");

if (botonLogOut) {
    botonLogOut.onclick = async () => {
        const r = await fetch("/auth/logout", {
            method: "POST",
            credentials: "include"
        });
        if (r.ok) {
            window.location.href = "/Home/Index";
        } else {
            alert("Error al cerrar sesion");
        }
    };
}
