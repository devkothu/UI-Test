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

  const popup = document.getElementById("qrPopup");
  const popupImage = document.getElementById("qrPopupImage");
  const popupText = document.getElementById("qrPopupText");

  const showQr = (url, anchor) => {
    if (!popup || !popupImage || !popupText || !url) {
      return;
    }

    const qrSrc = `https://api.qrserver.com/v1/create-qr-code/?size=220x220&data=${encodeURIComponent(url)}`;
    popupImage.src = qrSrc;
    popupText.textContent = url;
    popup.setAttribute("aria-hidden", "false");

    const rect = anchor.getBoundingClientRect();
    const top = rect.bottom + window.scrollY + 8;
    const left = Math.max(12, rect.left + window.scrollX - 40);
    popup.style.top = `${top}px`;
    popup.style.left = `${left}px`;
  };

  const hideQr = () => {
    if (!popup) {
      return;
    }

    popup.setAttribute("aria-hidden", "true");
  };

  document.querySelectorAll(".qr-trigger").forEach((button) => {
    button.addEventListener("mouseenter", () => showQr(button.dataset.qrUrl, button));
    button.addEventListener("focus", () => showQr(button.dataset.qrUrl, button));
    button.addEventListener("click", () => showQr(button.dataset.qrUrl, button));
    button.addEventListener("mouseleave", hideQr);
    button.addEventListener("blur", hideQr);
  });

  document.addEventListener("click", (event) => {
    if (!popup || popup.contains(event.target)) {
      return;
    }

    if (!event.target.closest(".qr-trigger")) {
      hideQr();
    }
  });

  document.querySelectorAll(".copy-btn").forEach((button) => {
    button.addEventListener("click", async () => {
      const value = button.dataset.copyText;
      if (!value) {
        return;
      }

      try {
        await navigator.clipboard.writeText(value);
        const oldText = button.textContent;
        button.textContent = "Copied";
        setTimeout(() => {
          button.textContent = oldText;
        }, 1200);
      } catch {
        button.textContent = "Copy failed";
      }
    });
  });
})();
