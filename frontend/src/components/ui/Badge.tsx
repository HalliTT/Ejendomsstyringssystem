import type { ReactNode } from "react";
import "@/components/ui/Badge.css";

const styles = {
  badge: "badge",
  dot: "badge-dot",
  neutral: "badge-neutral",
  success: "badge-success",
  warning: "badge-warning",
  danger: "badge-danger",
  accent: "badge-accent",
  purple: "badge-purple",
};

export type BadgeVariant =
  | "neutral"
  | "success"
  | "warning"
  | "danger"
  | "accent"
  | "purple";

interface BadgeProps {
  children: ReactNode;
  variant?: BadgeVariant;
  dot?: boolean;
}

export function Badge({
  children,
  variant = "neutral",
  dot = false,
}: BadgeProps) {
  return (
    <span className={`${styles.badge} ${styles[variant]}`}>
      {dot && <span className={styles.dot} />}
      {children}
    </span>
  );
}
