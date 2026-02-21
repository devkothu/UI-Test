(() => {
  const root = document.documentElement;
  const key = "shorturl-theme";
  const current = localStorage.getItem(key);

  if (current) {
    root.setAttribute("data-theme", current);
  }

  const toggle = document.getElementById("themeToggle");
  if (toggle) {
    toggle.addEventListener("click", () => {
      const next = root.getAttribute("data-theme") === "dark" ? "light" : "dark";
      root.setAttribute("data-theme", next);
      localStorage.setItem(key, next);
    });
  }

  document.querySelectorAll(".download-qr-btn").forEach((button) => {
    button.addEventListener("click", async () => {
      const url = button.dataset.qrUrl;
      if (!url) {
        return;
      }

      const qrSrc = `https://api.qrserver.com/v1/create-qr-code/?size=512x512&data=${encodeURIComponent(url)}`;

      try {
        const response = await fetch(qrSrc);
        if (!response.ok) {
          throw new Error("Failed to fetch QR image");
        }

        const blob = await response.blob();
        const downloadUrl = URL.createObjectURL(blob);
        const link = document.createElement("a");
        const safeName = url.replace(/[^a-z0-9]/gi, "-").toLowerCase().replace(/-+/g, "-").replace(/^-|-$/g, "");

        link.href = downloadUrl;
        link.download = `qr-${safeName || "code"}.png`;
        document.body.appendChild(link);
        link.click();
        link.remove();
        URL.revokeObjectURL(downloadUrl);

        const oldText = button.textContent;
        button.textContent = "Downloaded";
        setTimeout(() => {
          button.textContent = oldText;
        }, 1200);
      } catch {
        button.textContent = "Download failed";
      }
    });
  });
})();
