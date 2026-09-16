import "@/components/layout/AppShell.css";
import { Topbar } from "@/components/layout/Topbar";
import { Sidebar } from "@/components/layout/Sidebar";
import type { Theme } from "@/hooks/useTheme";

interface AppShellProps {
  pageTitle: string;
  children: React.ReactNode;
  theme: Theme;
  onToggleTheme: () => void;
  onAddClick: () => void;
}

export function AppShell({
  pageTitle,
  children,
  theme,
  onToggleTheme,
  onAddClick,
}: AppShellProps) {
  return (
    <div className="shell">
      <Sidebar />
      <div className="main">
        <Topbar
          pageTitle={pageTitle}
          theme={theme}
          onToggleTheme={onToggleTheme}
          onAddClick={onAddClick}
        />
        <div className="content">{children}</div>
      </div>
    </div>
  );
}
