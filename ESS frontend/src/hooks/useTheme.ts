import { useCallback, useEffect, useState } from "react";

export type Theme = "light" | "dark";

const storage_key = "ejendom-theme"

function getInitialTheme(): Theme {
    if (typeof window === "undefined") return "light";
    
    const key  = window.localStorage.getItem(storage_key);
    if (key === "light" || key === "dark") return key;
    
    return window.matchMedia("(prefers-color-scheme: dark)").matches ? "dark" : "light"
}

export function useTheme(): { theme: Theme; toggleTheme: () => void }{
    const [theme, setTheme] = useState<Theme>(getInitialTheme);

    useEffect(() => {
        document.documentElement.setAttribute("data-theme", theme);
        window.localStorage.setItem(storage_key, theme);
    }, []);

    const toggleTheme = useCallback(() => {
        setTheme(prev => (prev === "light" ? "dark" : "light"));
    }, []);

    return { theme, toggleTheme };
}