const botonLogOut = document.getElementById("botonLogOut");

botonLogOut.onclick = async () => {
    const r = await fetch("/auth/logout", { method: "POST" });
    r.ok ? location.href = "/home/index" : alert("Error al cerrar sesión");
};