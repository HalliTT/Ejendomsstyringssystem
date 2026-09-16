import "@/components/layout/Topbar.css";
import { MoonIcon, PlusIcon, SunIcon } from "../ui/Icons";
import { IconButton } from "../ui/IconButton";
import type { Theme } from "@/hooks/useTheme";

interface TopbarProps {
  pageTitle: string;
  theme: Theme;
  onToggleTheme: () => void;
  onAddClick: () => void;
}

export function Topbar({
  pageTitle,
  theme,
  onToggleTheme,
  onAddClick,
}: TopbarProps) {
  return (
    <div className="topbar">
      <div className="topbar-left">
        <h1 className="topbar-page-title">{pageTitle}</h1>
      </div>

      <div className="topbar-right">
        <div className="topbar-search">Search</div>
        <button type="button" className="topbar-add-btn" onClick={onAddClick}>
          <PlusIcon width={17} height={17} />
          <span>Add property</span>
        </button>

        <div className="topbar-divider" />

        <IconButton aria-label="Toggle dark mode" onClick={onToggleTheme}>
          {theme === "dark" ? (
            <SunIcon width={19} height={19} />
          ) : (
            <MoonIcon width={19} height={19} />
          )}
        </IconButton>
      </div>
    </div>
  );
}
