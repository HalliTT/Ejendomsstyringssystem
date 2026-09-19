import "@/components/layout/Topbar.css";
import { MoonIcon, PlusIcon, SunIcon } from "../ui/Icons";
import { IconButton } from "../ui/IconButton";
import type { Theme } from "@/hooks/useTheme";
import { ProfileMenu } from "./ProfileMenu";

interface TopbarProps {
  pageTitle: string;
  theme: Theme;
  onToggleTheme: () => void;
  onAddClick: () => void;
  isProfileMenuOpen: boolean;
  onToggleProfileMenu: () => void;
  onCloseProfileMenu: () => void;
  onLogout: () => void;
}

export function Topbar({
  pageTitle,
  theme,
  onToggleTheme,
  onAddClick,
  isProfileMenuOpen,
  onToggleProfileMenu,
  onCloseProfileMenu,
  onLogout,
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

        <ProfileMenu
          name="test"
          email="test@test.dk"
          avatar="#222"
          isOpen={isProfileMenuOpen}
          onToggle={onToggleProfileMenu}
          onClose={onCloseProfileMenu}
          onLogout={onLogout}
        />
      </div>
    </div>
  );
}
