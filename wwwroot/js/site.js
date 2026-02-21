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



  const generatedResult = document.querySelector(".generated-result");
  if (generatedResult) {
    const toggleButton = generatedResult.querySelector("[data-generated-toggle]");
    const content = generatedResult.querySelector("[data-generated-content]");

    if (toggleButton && content) {
      toggleButton.addEventListener("click", () => {
        const isExpanded = toggleButton.getAttribute("aria-expanded") === "true";
        const nextExpanded = !isExpanded;

        content.hidden = !nextExpanded;
        toggleButton.setAttribute("aria-expanded", String(nextExpanded));
        toggleButton.textContent = nextExpanded ? "Unexpand" : "Expand";
      });
    }
  }

  document.querySelectorAll(".copy-url-btn").forEach((button) => {
    button.addEventListener("click", async () => {
      const url = button.dataset.copyUrl;
      if (!url) {
        return;
      }

      const initialText = button.textContent;

      try {
        await navigator.clipboard.writeText(url);
        button.textContent = "Copied";
      } catch {
        button.textContent = "Copy failed";
      }

      setTimeout(() => {
        button.textContent = initialText;
      }, 1100);
    });
  });


  const recentUrlsTable = document.getElementById("recentUrlsTable");
  if (recentUrlsTable) {
    const tableBody = recentUrlsTable.querySelector("tbody");
    const rows = tableBody ? Array.from(tableBody.querySelectorAll("tr")) : [];
    const pageSize = Number.parseInt(recentUrlsTable.dataset.pageSize ?? "5", 10);
    const pagination = document.getElementById("recentUrlsPagination");

    if (pagination && rows.length > pageSize && pageSize > 0) {
      const status = pagination.querySelector("[data-pagination-status]");
      const prevButton = pagination.querySelector("[data-pagination-action='prev']");
      const nextButton = pagination.querySelector("[data-pagination-action='next']");
      const totalPages = Math.ceil(rows.length / pageSize);
      let currentPage = 1;

      const renderPage = () => {
        const start = (currentPage - 1) * pageSize;
        const end = start + pageSize;

        rows.forEach((row, index) => {
          row.hidden = index < start || index >= end;
        });

        if (status) {
          status.textContent = `Page ${currentPage} of ${totalPages}`;
        }

        if (prevButton) {
          prevButton.disabled = currentPage === 1;
        }

        if (nextButton) {
          nextButton.disabled = currentPage === totalPages;
        }
      };

      prevButton?.addEventListener("click", () => {
        if (currentPage > 1) {
          currentPage -= 1;
          renderPage();
        }
      });

      nextButton?.addEventListener("click", () => {
        if (currentPage < totalPages) {
          currentPage += 1;
          renderPage();
        }
      });

      pagination.hidden = false;
      renderPage();
    }
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
