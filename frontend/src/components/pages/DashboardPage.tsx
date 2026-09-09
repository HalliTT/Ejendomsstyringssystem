import { AppShell } from "@/components/layout/AppShell";
import { Outlet } from "react-router";

export function DashboardPage() {
  return (
    <AppShell>
      <Outlet />
    </AppShell>
  );
}
