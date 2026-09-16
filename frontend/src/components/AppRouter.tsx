import { BrowserRouter, Navigate, Route, Routes } from "react-router";
import { DashboardPage } from "@/pages/DashboardPage";

import { OverviewSection } from "@/sections/OverviewSection";
import { PropertiesSection } from "@/sections/PropertiesSection";

import { PropertyPage } from "@/pages/PropertyPage";
import CallBackPage from "@/pages/Callback";

export function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/callback" element={<CallBackPage />} />
        <Route path="/dashboard" element={<DashboardPage />}>
          <Route index element={<OverviewSection />} />
          <Route path="properties" element={<PropertiesSection />} />
          <Route
            path="properties/:propertyId"
            element={<PropertyPage onBack={() => {}} />}
          />
        </Route>
        <Route path="*" element={<Navigate to="/dashboard" replace />} />
      </Routes>
    </BrowserRouter>
  );
}
