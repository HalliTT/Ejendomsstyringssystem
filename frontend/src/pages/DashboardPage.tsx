import "@/pages/DashboardPage.css";
import { AppShell } from "@/components/layout/AppShell";
import { Outlet } from "react-router";
import { useModalState } from "@/hooks/useModalState";
import { useTheme } from "@/hooks/useTheme";
import { PropertyModal } from "@/components/dashboard/PropertyModal";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { createProperty } from "@/api/properties";
import type { PropertyInput } from "@/types";
import { useAuth } from "@/hooks/useAuth";
import { startLogin, logout } from "@/lib/auth/login";

export function DashboardPage() {
  const queryClient = useQueryClient();
  const addPropertyModal = useModalState(false);
  const profileMenu = useModalState(false);
  const { theme, toggleTheme } = useTheme();

  const createPropertyMutation = useMutation({
    mutationFn: createProperty,

    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["properties"],
      });
    },
  });

  const submit = async (propertyData: PropertyInput) => {
    await createPropertyMutation.mutate(propertyData);
  };

  const handleOpenAddProperty = () => {
    createPropertyMutation.reset();
    addPropertyModal.open();
  };

  const { isLoggedIn } = useAuth();

  return (
    <>
      <AppShell
        pageTitle="tak"
        theme={theme}
        onToggleTheme={toggleTheme}
        onAddClick={handleOpenAddProperty}
        isProfileMenuOpen={profileMenu.isOpen}
        onToggleProfileMenu={profileMenu.toggle}
        onCloseProfileMenu={profileMenu.close}
        onLogout={logout}
      >
        {!isLoggedIn ? (
          <div className="dashboard-login">
            <div className="dashboard-login-backdrop">
              <div className="fake-card fake-card-large" />
              <div className="fake-card" />
              <div className="fake-card" />
              <div className="fake-card" />
              <div className="fake-card" />
              <div className="fake-card" />
            </div>

            <div className="dashboard-login-card">
              <h2>Welcome to your dashboard</h2>
              <p>Log in to view your properties and manage your account.</p>

              <button onClick={startLogin}>Login</button>
            </div>
          </div>
        ) : (
          <Outlet />
        )}
      </AppShell>
      <PropertyModal
        isOpen={isLoggedIn && addPropertyModal.isOpen}
        onClose={addPropertyModal.close}
        onSubmit={submit}
        isSubmitting={createPropertyMutation.isPending}
        isSuccess={createPropertyMutation.isSuccess}
      />
    </>
  );
}
