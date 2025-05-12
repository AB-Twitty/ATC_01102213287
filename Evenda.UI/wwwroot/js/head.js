class ThemeCustomizer {
    theme = "light";

    init() {
        this.html = document.getElementsByTagName("html")[0];

        var e = sessionStorage.getItem("__THEME_CONFIG__");
        if (e) {
            e = JSON.parse(e);
            this.theme = e.theme;
        }

        this.theme = this.html.getAttribute("data-bs-theme") || this.theme;

        if (this.theme === "dark") {
            document.documentElement.setAttribute("data-bs-theme", "dark");
        } else if (this.theme === "light") {
            document.documentElement.setAttribute("data-bs-theme", "light");
        } else {
            document.documentElement.removeAttribute("data-bs-theme");
        }

        const lightIcon = document.getElementById("light-theme");
        const darkIcon = document.getElementById("dark-theme");
        if (lightIcon && darkIcon) {
            if (this.theme === "light") {
                lightIcon.classList.remove("d-none");
                darkIcon.classList.add("d-none");
            } else {
                darkIcon.classList.remove("d-none");
                lightIcon.classList.add("d-none");
            }
        }

        this.onThemeChange();

        window.addEventListener("DOMContentLoaded", () => {
            this.after();
        });
    }

    onThemeChange = () => {
        if (this.theme === "dark") {
            document.documentElement.setAttribute("data-bs-theme", "dark");
        } else {
            document.documentElement.setAttribute("data-bs-theme", "light");
        }

        if (this.lightTheme && this.darkTheme) {
            if (this.theme === "light") {
                this.lightTheme.classList.remove("d-none");
                this.darkTheme.classList.add("d-none");
            } else {
                this.darkTheme.classList.remove("d-none");
                this.lightTheme.classList.add("d-none");
            }
        }

        sessionStorage.setItem("__THEME_CONFIG__", JSON.stringify({ theme: this.theme }));
    };

    after() {
        this.lightTheme = document.getElementById("light-theme");
        this.darkTheme = document.getElementById("dark-theme");

        if (this.lightTheme && this.darkTheme) {
            this.lightTheme.addEventListener("click", e => {
                this.theme = "dark";
                this.onThemeChange();
            });

            this.darkTheme.addEventListener("click", e => {
                this.theme = "light";
                this.onThemeChange();
            });
        }

        this.onThemeChange();
    }
}

(new ThemeCustomizer).init();

document.addEventListener("DOMContentLoaded", function () {
    if (window.innerWidth <= 1140) {
        document.body.classList.add("canvas-menu");
    }
});
