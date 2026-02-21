(() => {
  const root = document.documentElement;
  const key = "shorturl-theme";
  const current = localStorage.getItem(key);

  if (current) {
    root.setAttribute("data-theme", current);
  }

  const toggle = document.getElementById("themeToggle");
  if (!toggle) return;

  toggle.addEventListener("click", () => {
    const next = root.getAttribute("data-theme") === "dark" ? "light" : "dark";
    root.setAttribute("data-theme", next);
    localStorage.setItem(key, next);
  });
})();
