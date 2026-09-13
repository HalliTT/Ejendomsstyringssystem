import "@/components/ui/Card.css";
import type { HTMLAttributes, ReactNode } from "react";

interface CardProps extends HTMLAttributes<HTMLDivElement> {
  children: ReactNode;
}

export function Card({ children, className = "" }: CardProps) {
  const classes = ["card", className].filter(Boolean).join(" ");
  return <div className={classes}>{children}</div>;
}

interface CardHeaderProps {
  title: string;
  subtitle?: string;
  action?: ReactNode;
}

export function CardHeader({ title, subtitle, action }: CardHeaderProps) {
  <div className="card-header">
    <div>
      <h3 className="card-title">{title}</h3>
      {subtitle && <p className="card-subtitle">{subtitle}</p>}
    </div>
    {action}
  </div>;
}
