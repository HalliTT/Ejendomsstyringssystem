import "@/pages/DashboardPage.css";
import { AppShell } from "@/components/layout/AppShell";
import { Outlet, useLocation } from "react-router";
import { useModalState } from "@/hooks/useModalState";
import { useTheme } from "@/hooks/useTheme";
import { PropertyModal } from "@/components/dashboard/PropertyModal";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { createProperty } from "@/api/properties";
import type { PropertyInput } from "@/types";
import { useAuth } from "@/contexts/AuthContext";
import { LoginOverlay } from "@/components/layout/LoginOverlay";

function getPageTitle(pathname: string): string {
  if (pathname.startsWith("/dashboard/properties")) return "Properties";
  if (pathname.startsWith("/dashboard/bookings")) return "Bookings";
  return "Dashboard";
}

export function DashboardPage() {
  const queryClient = useQueryClient();
  const addPropertyModal = useModalState(false);
  const profileMenu = useModalState(false);
  const { theme, toggleTheme } = useTheme();
  const location = useLocation();
  const pageTitle = getPageTitle(location.pathname);

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

  const { isLoggedIn, logout } = useAuth();

  return (
    <>
      <AppShell
        pageTitle={pageTitle}
        theme={theme}
        onToggleTheme={toggleTheme}
        onAddClick={handleOpenAddProperty}
        isProfileMenuOpen={profileMenu.isOpen}
        onToggleProfileMenu={profileMenu.toggle}
        onCloseProfileMenu={profileMenu.close}
        onLogout={logout}
      >
        {isLoggedIn && <Outlet />}
      </AppShell>
      {!isLoggedIn && <LoginOverlay />}
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
