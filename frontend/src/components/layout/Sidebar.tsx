import "@/components/layout/Sidebar.css";
import { NavLink } from "react-router";
import { BuildingIcon, HomeIcon, UsersIcon } from "../ui/Icons";

export function Sidebar() {
  return (
    <div className="sidebar">
      <div className="sidebar-brand">
        <div className="sidebar-brand-mark" />
        <span className="sidebar-brand-name">ESS</span>
      </div>
      <div className="sidebar-navigation">
        <NavLink
          to="/dashboard"
          end
          className={({ isActive }) =>
            `sidebar-nav-item ${isActive ? "sidebar-nav-item-active" : ""}`
          }
        >
          <HomeIcon width={18} height={18} />
          Dashboard
        </NavLink>
        <NavLink
          to="/dashboard/properties"
          className={({ isActive }) =>
            `sidebar-nav-item ${isActive ? "sidebar-nav-item-active" : ""}`
          }
        >
          <BuildingIcon width={18} height={18} />
          Properties
        </NavLink>
        <NavLink
          to="/dashboard/bookings"
          className={({ isActive }) =>
            `sidebar-nav-item ${isActive ? "sidebar-nav-item-active" : ""}`
          }
        >
          <UsersIcon width={18} height={18} />
          Bookings
        </NavLink>
      </div>
    </div>
  );
}
