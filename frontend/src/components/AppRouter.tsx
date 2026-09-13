import { BrowserRouter, Navigate, Route, Routes } from "react-router";

import { DashboardPage } from "@/components/pages/DashboardPage";
import { PropertyPage } from "@/components/pages/PropertyPage";

import { OverviewSection } from "@/components/sections/OverviewSection";
import { PropertiesSection } from "@/components/sections/PropertiesSection";

export function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/dashboard" element={<DashboardPage />}>
          <Route index element={<OverviewSection />} />
          <Route path="properties" element={<PropertiesSection />} />
          <Route path="properties/:propertyId" element={<PropertyPage />} />
        </Route>
        <Route path="*" element={<Navigate to="/dashboard" replace />} />
      </Routes>
    </BrowserRouter>
  );
}
