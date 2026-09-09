import "@/components/layout/Sidebar.css";
import { NavLink } from "react-router";

export function Sidebar() {
  return (
    <div className="sidebar">
      <h2>Sidebar</h2>
      <NavLink
        to="/dashboard"
        className={({ isActive }) => (isActive ? "active" : "")}
      >
        Dashboard
      </NavLink>
      <NavLink
        to="/dashboard/properties"
        className={({ isActive }) => (isActive ? "active" : "")}
      >
        Properties
      </NavLink>
    </div>
  );
}
