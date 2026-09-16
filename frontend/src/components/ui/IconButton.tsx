import "@/components/ui/IconButton.css";
import type { ButtonHTMLAttributes, ReactNode } from "react";

interface IconButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  children: ReactNode;
  badgeCount?: number;
  "aria-label": string;
}

export function IconButton({
  children,
  badgeCount,
  className = "",
  ...rest
}: IconButtonProps) {
  return (
    <button type="button" className={`icon-btn ${className}`} {...rest}>
      {children}
      {!!badgeCount && (
        <span className="icon-btn-badge">
          {badgeCount > 9 ? "9+" : badgeCount}
        </span>
      )}
    </button>
  );
}
